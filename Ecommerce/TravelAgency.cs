using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce
{
    class TravelAgency
    {
        private string agency_id; //Variable N where N = 5 (TravelAgency1 --> TravelAgency5)
        private int credit_card;

        public TravelAgency (string agency_id)
        {
            this.agency_id = agency_id;
            //TODO: OrderProcessing
        }

        public void setCCNumber(int ccNumber)
        {
            credit_card = ccNumber;
        }

        public string getID() {
            return this.agency_id;
        }

        public void hotelPriceBeenCut(decimal current_price, decimal new_price, int available_rooms, string id)
        {
            int rooms_to_order;
            int demand = 0; //Demand is a variable that will be used as a multiplier for how many rooms to order

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

            rooms_to_order = (available_rooms - (10 * demand)); //Take whatever rooms are available, subtract it by 10 * demand
        }

        private void placeOrder(OrderObject order)
        {
            string encryptedOrder = EnDecoder.Encode(order); //encrypt the orderobject into a string

            //TODO : create thread to place order?
        }

        private void confirmOrder(OrderObject orderConfirm)
        {
            string timeStamp = DateTime.Now.ToString(); //Create a time stamp for when order is confirmed
            Console.WriteLine(orderConfirm);
        }
    }
}   