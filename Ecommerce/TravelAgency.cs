using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
namespace Ecommerce
{
    class TravelAgency
    {
        //TravelAgency ATTRIBUTES:
        //----------------------------------------------------------------------------------------------------------------------------------
        //TravelAgency Defining Attributes:
        private string agency_id; //Variable N where N = 5 (TravelAgency1 --> TravelAgency5)
        private int credit_card;  //Variable to hold credit card number
        //----------------------------------------------------------------------------------------------------------------------------------


        /// <summary>
        /// This is the args constructor of the TravelAgency class. 
        /// </summary>
        /// <param name="agency_id">String to be set as the id of this TravelAgency</param>
        public TravelAgency (string agency_id)
        {
            this.agency_id = agency_id;

            OrderProcessing.addOrderToProcessListener(processedOrder);
        }

        /// <summary>
        /// This sets the credit card number taking one parameter, the credit card number
        /// </summary>
        /// /// <param name="cc_number">The new credit card number to set for this TravelAgency</param>
        public void setCCNumber(int cc_number) //Set the Credit Card Number via main method later
        {
            this.credit_card = cc_number;
        }

        /// <summary>
        /// This gets the current agency_ID string
        /// </summary>
        /// <returns>returns a string representation of this TravelAgency</returns>
        public string getID() { //Get the agency ID (e.g. Agency1, Agency2, Agency4...)
            return this.agency_id;
        }

        /// <summary>
        /// This is the main function that handles hotel price cuts via PriceCut delegate created in Hotel.cs
        /// </summary>
        /// <param name="current_price">the current price of rooms in the Hotel.</param>
        /// <param name="new_price">the new price of rooms in the Hotel.</param>
        /// <param name="available_rooms">the current number of available rooms in the Hotel.</param>
        /// <param name="hotel_id">The string id of the Hotel.</param>
        public void hotelPriceBeenCut(decimal current_price, decimal new_price, int available_rooms, string hotel_id) //Using delegate in hotel.cs
        {
            Console.WriteLine("PriceCut Event successfully emitted. TravelAgency {0}",this.agency_id);
            int rooms_to_order;
            int demand = 0; //Demand is a variable that will be used as a multiplier for how many rooms to order
            int sum = Hotel.MAX_PRICE + Hotel.MIN_PRICE;
            int avg = sum / 2;
            Boolean do_buy = (new_price > (decimal)(1.15 * avg)) ? false : true;

            if (available_rooms < (Hotel.MAX_ROOMS*.20))
            { //If current availability is less than 20% of max rooms
                demand++; //Increase demand
            } else

            if(current_price < new_price)
            {
                demand--; //If the new price is higher than the old price, decrease demand
            } else
            {
                demand++; //If the new price is lower than the older price, increase demand
            }

            if (do_buy)
            {
                rooms_to_order = (available_rooms - (10 * demand)); //Take whatever rooms are available, subtract it by 10 * demand
                                                                    //Calculate the subTotal:
                decimal amount = BankService.formatCurrency(rooms_to_order * new_price);
                //create a new order Object:
                OrderObject new_order_object = new OrderObject(this.agency_id, hotel_id, this.credit_card, rooms_to_order, amount, new_price, false);
                //Place Order:
                placeOrder(new_order_object);
            }
        }

        /// <summary>
        /// This takes an OrderObject and encrypts it into a string.
        /// </summary>
        /// <param name="order">The OrderObject to place an order with.</param>
        private void placeOrder(OrderObject order)
        {

            //Get the ID of the hotel who will receive this order for processing:
            string hotel_id = order.getReceiverID();

            //encrypt the orderobject into a string:
            string encrypted_order = EnDecoder.Encode(order); 

            
            //Create a new OrderProcessing thread to handle the unprocessed order:
            Thread place_order_thread = new Thread(() => OrderProcessing.submitOrderToProcess(encrypted_order, hotel_id));

            //Start the thread:
            place_order_thread.Start();
        }

        /// <summary>
        /// This takes an OrderObject to be confirmed, and creates a timestamp of when this is called as well as writing to Console
        /// </summary>
        /// <param name="order_confirm">The OrderObject retreived from the ProcessedOrder MultiCellBuffer.</param>
        private void confirmOrder(OrderObject order_confirm)
        {
            string timeStamp = DateTime.Now.ToString(); //Create a time stamp for when order is confirmed
            
            //Print Order Confirmation received message:
            Console.WriteLine("Travel Agency {0} received a new order confirmation:", this.agency_id);

            //Print header and Body of Order Confirmation Receipt:
            Console.WriteLine("---------------------------------");
            Console.WriteLine("Order Confirmation:");
            Console.WriteLine("Agency ID: {0}", order_confirm.getSenderID());
            Console.WriteLine("Hotel ID: {0}", order_confirm.getReceiverID());
            Console.WriteLine("Rooms Ordered: {0}", order_confirm.getNumberOfRoomsOrdered());
            Console.WriteLine("Room Price: {0}", order_confirm.getUnitPrice());
            Console.WriteLine("Total(+tax): {0}", order_confirm.getAmount());
            if (order_confirm.isValid()){
                Console.WriteLine("Order Confirmation: VALID");
            }
            else {
                Console.WriteLine("Order Confirmation: NOT VALID");
            }

            //Footer of the Order Confirmation Receipt:
            Console.WriteLine("TimeStamp: {0}",timeStamp);
            Console.WriteLine("---------------------------------");
            Console.WriteLine();        
        }

        /// <summary>
        /// This takes the ID of the TravelAgency it will be processing for and checks for correct destination. It then processes the order and confirms it.
        /// </summary>
        /// <param name="travel_agency_id">The travelAgency id that was emitted </param>
        public void processedOrder(string travel_agency_id) //Method to check if order was meant for specific travel agency and confirm it
        {
            if (travel_agency_id  == this.agency_id) //Basic check for correct agency ID
            {
                string encryptedOrder = OrderProcessing.getProcessedOrderObject(this.agency_id); //Encrypted order is retrieved from OrderProcessing

                OrderObject newOrder = EnDecoder.Decode(encryptedOrder); //decrypt order via decode method in EnDecoder

                confirmOrder(newOrder); //confirm the order of the neworder that was created (and also holds timestamp!)
            }
        }
    }
}   