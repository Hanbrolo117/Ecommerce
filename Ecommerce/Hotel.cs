﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ecommerce
{
    class Hotel
    {
        //HOTEL ATTRIBUTES:
        //----------------------------------------------------------------------------------------------------------------------------------
        //Hotel Defining Attributes:
        private string id;                              //Id of this Hotel
        private decimal price;                          //The current price of rooms for this Hotel
        private int current_number_of_available_rooms;  //The current number of available rooms in this Hotel
        private int current_pricecuts_made;             // Instantiate the current number of price cuts made. 20 is the max.
        private PriceModel price_model;


        //Public constant variables for use by application and other classes (including Hotel of course):
        public const int MAX_PRICE = 500;               //Maximum price of a given room in the Hotel can have.
        public const int MAX_ROOMS = 350;               //Maximum number of rooms a Hotel can have.
        public const int MIN_PRICE = 50;                //Minimum price of a given room in the Hotel can have.
        public const int MAX_PRICECUTS_ALLOWED = 20;    //Maximum number of price cuts of room prices a the Hotel can have.
        
        //Hotel Events:
        public delegate void PriceCut(decimal current_price, decimal new_price, int available_rooms, string id);    //Delegate Defines method signature for the Handlers
        private event PriceCut price_cut_event;                                                                     //A PriceCut event variable for the Hotel to emit to it's subscribers.
        //----------------------------------------------------------------------------------------------------------------------------------



        /// <summary>
        /// This is the args constructor of the Hotel class. 
        /// </summary>
        /// <param name="new_id">The unique string representation of an id for identifying and instance of this class.</param>
        public Hotel(string new_id) {
            this.id = new_id;                                                                       // Initialize the Hotel's ID.
            this.current_number_of_available_rooms = MAX_ROOMS;                                     // Initialize the Hotel's 
            this.current_pricecuts_made = 0;                                                        // Instantiate the current number of price cuts made. 20 is the max.
            this.price_model = new PriceModel(this.price, this.current_number_of_available_rooms);  // Instantiate a new Price Model object for this Hotel.
            this.price = BankService.formatCurrency(((Hotel.MAX_PRICE + Hotel.MIN_PRICE) / 2));     // TODO::Initialize first price value using the price model;

            OrderProcessing.addOrderToProcessListener(orderProcessHandler);

        }


        /// <summary>
        /// This is the main function that will be ran when the main program creates a new thread with it and calls Start().
        /// </summary>        
        public void hotelFunc() {
                             
           //Continue this Price Model Updating until the maximum number of price cuts have been made.
            while (current_pricecuts_made <= MAX_PRICECUTS_ALLOWED) {
                //To add some sense of time passing in the application.
                Thread.Sleep(1500);

                //TODO::UPDATE PRICE MODEL
                this.updateRoomPrice(this.price_model.generateNewPrice());
                
                //A Simple Room availability updater to keep Hotel data changing and fresh, thus adding variability to the ouput from Hotel to Hotel:
                if (this.current_number_of_available_rooms < 350) {
                    
                    //Slowly release room occupants to simulate real world actions in a hotel.
                    Random rand = new Random();
                    this.current_number_of_available_rooms += rand.Next(1,56); 
                    
                    //If more rooms were released than can be, adjust to maximum number of available rooms for the Hotel.
                    this.current_number_of_available_rooms = (this.current_number_of_available_rooms > Hotel.MAX_ROOMS) ? Hotel.MAX_ROOMS : this.current_number_of_available_rooms;
                    
                    //Update Price Model data.
                    this.price_model.setAvailableRooms(this.current_number_of_available_rooms);
                }//END ROOM RELEASE SIMULATOR0.
            }
        }


        /// <summary>
        /// This Function takes a method/function that should be from the TravelAgency, and subscribes it to the price_cut_event in this Hotel.
        /// </summary>
        /// <param name="travel_agency_handler">A function to handle a price cut event when the price cut event is emmited.</param>
        public void subscribeHandlerToPriceCutEvents(Action<decimal,decimal,int,string> travel_agency_handler) {
            //Using the += operator we can add, or rather, "subscribe" an action, or rather, a TravelAgency function/method that has the same method signature as the 
            //Hotels delegate, TO this hotel's delegate. Thus when the delegate "emits" a message it will call all functions that are subscribed to this delegate. This is 
            //essentially the core of what is event driven programming.
            this.price_cut_event += new PriceCut(travel_agency_handler);
        }


        /// <summary>
        /// This Function is subscribed to an OrderToBeProcessed Event and handles it using the orderProcessor function in the OrderProcessing class.
        /// </summary>
        /// <param name="hotel_id">The Id of the hotel that needs to process the respective order that triggered the orderToBeProcessed event</param>
        public void orderProcessHandler(string hotel_id) {
            //If this Hotel matches with this event that emitted this:
            if (this.id == hotel_id) {
                //Get encoded orderObject string from MultiCellBuffer:
                string encoded_order__object_to_process = OrderProcessing.getOrderToProcess(this.id);

                //Process Order via the OrderProcessing class:
                OrderProcessing.orderProcessor(encoded_order__object_to_process);

            }//END IF STATEMENT

        }


        /// <summary>
        /// This function updates the price of the Hotel rooms, and if the new price is lower than the current price, emits an event to any and all subscribers.
        /// </summary>
        /// <param name="new_room_price">The new price of hotel rooms. (as determined by the price model)</param>
        private void updateRoomPrice(decimal new_room_price) {
            decimal current_price = this.price;     //Get the current price before updating the price.
            this.price = new_room_price;            //Update the price of rooms in this Hotel.
            Boolean is_new_price_lower = (new_room_price < current_price) ? true : false;

            //If the new price based on the price model is indeed lower than what the current price was AND there exists AT LEAST one subscriber to our delegate:
            if ((is_new_price_lower) && (this.price_cut_event != null)) {
                Console.WriteLine("\nHotel {0} is having a Price Cut Event!\nPrevious Price: {1}\nNew Price: {2}\n", this.id, current_price, new_room_price);
                //Increment the number of price cuts made.
                this.current_pricecuts_made++;

                //Emit an event to all subsriber(s) for handling with the provided data:
                this.price_cut_event(current_price, new_room_price, this.current_number_of_available_rooms,this.id);
            }
        }


        /// <summary>
        /// PriceModel Class which is used exclusively by the Hotel to determine the next price.
        /// </summary>
        private class PriceModel {

            private decimal current_price;
            private int available_number_of_rooms;
            private int mod_count;

            /// <summary>
            /// This is the args constructor of the PriceModel Class. 
            /// </summary>
            /// <param name="price">The current price of the Hotel's rooms.</param>
            /// <param name="available_rooms">The current number of available rooms in the Hotel.</param>
            public PriceModel(decimal price, int available_rooms) {
                this.current_price = BankService.formatCurrency(price);
                this.available_number_of_rooms = available_rooms;
                this.mod_count = 0;
            }


            /// <summary>
            /// This function is the core of the Price model class. It uses the attributes of the price model to 
            /// </summary>
            /// <returns>The new hotel price of its rooms</returns>
            public decimal generateNewPrice() {
                //Price Modifiers:
                decimal price_increase = 0;//Initial value of the price increase modifier.
                decimal price_discount = 1;//Initial value of the price decrease modifier.
                
                //If the availability of rooms is less than half (less supply), the drive for demand goes up, OR IF IT IS THE WEEKDAY:
                if ((this.available_number_of_rooms < (Hotel.MAX_ROOMS / 3)) || ((mod_count%7) < 5) )
                {
                    //Increase by 20%:
                    price_increase = BankService.formatCurrency((decimal)(this.current_price * 1.02M));
                }

                //If it is the (6 == ) Sunday discount or  wild card discount, apply the discount:
                Random wildcard = new Random();
                int wildCardSpecials = wildcard.Next(0, 7);
                if ((wildCardSpecials > 3) || (((mod_count%7) > 4)))
                {
                    //Set the discount modifier to 1.2 or essentially taking 20% off if there is a price increase, if there is no high demand, take 45% off hotel room price for weekends.
                    price_discount = (price_increase == 0) ? BankService.formatCurrency(1.35M) : BankService.formatCurrency(1.20M);
                }
                mod_count++;


                this.current_price = BankService.formatCurrency(((this.current_price + price_increase) / price_discount));  //The actual Price model function.

                if (this.current_price < Hotel.MIN_PRICE) { this.current_price = BankService.formatCurrency(Hotel.MIN_PRICE); }          //Adjust price if model goes over minimum Hotel price.
                else if (this.current_price > Hotel.MAX_PRICE) { this.current_price = BankService.formatCurrency(Hotel.MAX_PRICE); }    //Adjust price if model goes over maximum Hotel price
                //this.current_price = test.Next(MIN_PRICE,MAX_PRICE+1);
                return this.current_price;                                                                                  //Return the new price.
            }

            /// <summary>
            /// This function updates the value of the available rooms in the Price Model.
            /// </summary>
            /// <param name="available_rooms">The integer representation of the new number of rooms in a hotel that are available.</param>
            public void setAvailableRooms(int available_rooms) {
                this.available_number_of_rooms = available_rooms;
            }
        }
    }
}
