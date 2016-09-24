using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ecommerce
{
    class Hotel
    {
        //Hotel Defining Attributes:
        private string id;                              //Id of this Hotel
        private decimal price;                          //The current price of rooms for this Hotel
        private int current_number_of_available_rooms;  //The current number of available rooms in this Hotel

        private const int MAX_PRICE = 500;
        private const int MIN_PRICE = 50;
        private const int MAX_PRICECUTS_ALLOWED = 20;

        private event PriceCut price_cut_event;

        public delegate void PriceCut(decimal current_price, decimal new_price, string id);

        public void HotelFunc(string hotel_id) {
            this.id = hotel_id;                 // Set up Hotel ID
            int current_pricecuts_made = 0;     // Instantiate the current number of price cuts made. 20 is the max.

            while (current_pricecuts_made <= MAX_PRICECUTS_ALLOWED) {
                Thread.Sleep(500);//To add some sense of time passing in the application.

                //TODO::UPDATE PRICE MODEL

                //TODO::CHECK t
            }



        }


        /// <summary>
        /// This Function takes a method/function that should be from the TravelAgency, and subscribes it to the price_cut_event in this Hotel.
        /// </summary>
        /// <param name="travel_agency_handler">A function to handle a price cut event when the price cut event is emmited.</param>
        public void subscribeHandlerToPriceCutEvents(Action<decimal,decimal,string> travel_agency_handler) {
            //Using the += operator we can add, or rather, "subscribe" an action, or rather, a TravelAgency function/method that has the same method signature as the 
            //Hotels delegate, TO this hotel's delegate. Thus when the delegate "emits" a message it will call all functions that are subscribed to this delegate. This is 
            //essentially the core of what is event driven programming.
            this.price_cut_event += new PriceCut(travel_agency_handler);
        }

        private decimal updateNewPrice() {
            //TODO::Implement with the PriceModel inner class.h
        }

        private class PriceModel {

        }
    }
}
