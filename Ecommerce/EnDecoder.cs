using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce
{
    class EnDecoder
    {

        /// <summary>
        /// This function takes in a string which has an OrderOjbect "encoded" into it, and decodes it back into an OrderObject.
        /// </summary>
        /// <param name="orderStr">The "encoded" OrderObject string to decode into an OrderObject.</param>
        /// <returns>The OderObject that was "decoded" from the "encoded" data passed in.</returns>
        public static OrderObject Decode(string orderStr) {
            //"decode" the "encoded" data:
            string[] orderData = orderStr.Split(' ');

            //Create a new orderObject to set up with the "decoded" data:
            OrderObject orderObj = new OrderObject();

            //Set up orderObject based on data in the encoded string:
            orderObj.setSenderID(orderData[0]);                         //Set the Sender ID
            orderObj.setReceiverID(orderData[1]);                       //Set the receiver ID
            orderObj.setCardNo(Convert.ToInt32(orderData[2]));          //Set the Card Number
            orderObj.setAmount(Convert.ToDecimal(orderData[3]));        //Set the Amount
            orderObj.setUnitPrice(Convert.ToDecimal(orderData[4]));     //Set the Unit Price
            orderObj.setIsValid(Convert.ToBoolean(orderData[5]));       //Set the Order Validator
            orderObj.setRoomsOrdered(Convert.ToInt32(orderData[6]));
            //Return the newly orderObject that has been created from the "decoded" data:
            return orderObj;
        }

        /// <summary>
        /// This function takes in an OrderObject and "encodes" it into a string format.
        /// </summary>
        /// <param name="orderObj">The OrderObject to encode into a string.</param>
        /// <returns>The "encoded" string representation of the OrderObject passed in.</returns>
        public static String Encode(OrderObject orderObj) {
            //Generate the new string to be the "encoded" orderObject:
            string orderObj_toString = orderObj.getSenderID();
            orderObj_toString += (" " + orderObj.getReceiverID());
            orderObj_toString += (" " + orderObj.getCardNo());
            orderObj_toString += (" " + orderObj.getAmount());
            orderObj_toString += (" " + orderObj.getUnitPrice());
            orderObj_toString += (" " + orderObj.isValid());
            orderObj_toString += (" " + orderObj.getNumberOfRoomsOrdered());
            //Return the encoded String of the orderObject passed in:
            return orderObj_toString;
        }

    }
}
