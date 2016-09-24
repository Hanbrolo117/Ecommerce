using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce
{
    class TravelAgency
    {
        int agency_id; //Variable N where N = 5 (TravelAgency1 --> TravelAgency5)

        public TravelAgency (int agency_id)
        {
            this.agency_id = agency_id;

            //TODO: Apply for credit card number
            //TODO: OrderProcessing
        }
        public void hotelPriceBeenCut(decimal current_price, decimal new_price, string id)
        {
            int rooms_to_order;
            bool demand = false; //If there is high demand, demand = TRUE

            if (Hotel.current_number_of_rooms_available < 70)
            { //If current availability is less than 20% of max rooms
                demand = true;
            } else
            {
                demand = false;
            }

        }

        private void placeOrder(OrderObject order)
        {
            string encryptedOrder = EnDecoder.Encode(order); //encrypt the orderobject into a string

            //TODO : create thread to place order?
        }

        private decimal 
    }
}   