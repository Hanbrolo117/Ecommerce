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
        private const int MAX_PRICECUTS_ALLOWED = 20;
        private string id;
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
        /// This Function 
        /// </summary>
        /// <param name="travel_agency_handler"></param>
        public void subscribeHandlerToPriceCutEvents(Action<decimal,decimal,string> travel_agency_handler) {
            //Using the += operator we can add, or rather, "subscribe" an action, or rather, a TravelAgency function/method that has the same method signature as the 
            //Hotels delegate, TO this hotel's delegate. Thus when the delegate "emits" a message it will call all functions that are subscribed to this delegate. This is 
            //essentially the core of what is event driven programming.
            this.price_cut_event += new PriceCut(travel_agency_handler);
        }

        private class PriceModel {

        }
    }
}
