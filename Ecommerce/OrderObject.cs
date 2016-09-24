using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce
{
    class OrderObject
    {
        //OrderObject Attributes:
        //-------------------------------
        private String senderID;
        private String receiverID;
        private int cardNo;
        private decimal amount;
        private decimal unitPrice;
        private Boolean valid_order;
        //-------------------------------


        /// <summary>
        /// This is an no-args constructor for the OrderObject class.
        /// </summary>
        public OrderObject() {
            this.senderID = null;
            this.receiverID = null;
            this.cardNo = 0;
            this.amount = 0;
            this.unitPrice = 0;
            this.valid_order = false;
        }

        //Sender = travel Agency, receiver = hotel
        /// <summary>
        /// This is the args-constructor for the OrderObect class.
        /// </summary>
        /// <param name="sd_id">The id of the Travel Agency that is making the order.</param>
        /// <param name="rcvr_id">The id of the Hotel that is receiving the order from the travel agency.</param>
        /// <param name="cc_number">The credit card number the travel agency will use to pay for the order.</param>
        /// <param name="amt">The amount that will be charged to the account associated with the given credit card number.</param>
        /// <param name="ut_price">The price at which each room ordered was purchased at.</param>
        /// <param name="is_v">A boolean identifier for determining whether this order was successful or not.</param>
        public OrderObject(string sd_id, string rcvr_id, int cc_number, decimal amt, decimal ut_price, Boolean is_v) {
            this.senderID = sd_id;
            this.receiverID = rcvr_id;
            this.cardNo = cc_number;
            this.amount = amt;
            this.unitPrice = ut_price;
            this.valid_order = is_v;
        }

        /// <summary>
        /// Setter for the SenderID.
        /// </summary>
        /// <param name="senderID">Id to set the senderId to.</param>
        public void setSenderID(string senderID)
        {
            this.senderID = senderID;
        }

        /// <summary>
        /// Setter for the ReceiverID.
        /// </summary>
        /// <param name="receiverID">Id to set the receiverId to.</param>
        public void setReceiverID(string receiverID)
        {
            this.receiverID = receiverID;
        }

        /// <summary>
        /// Setter for the credit card number.
        /// </summary>
        /// <param name="cardNo">credit card number to set the credit card number to.</param>
        public void setCardNo(int cardNo)
        {
            this.cardNo = cardNo;
        }

        /// <summary>
        /// Setter for setting the amount.
        /// </summary>
        /// <param name="amount">the amount to set the OrderObject's amount to.</param>
        public void setAmount(decimal amount)
        {
            this.amount = amount;
        }

        /// <summary>
        /// Setter for the unitPrice.
        /// </summary>
        /// <param name="unitPrice">The unitPrice to set the OrderObject's unitPrice to.</param>
        public void setUnitPrice(decimal unitPrice)
        {
            this.unitPrice = unitPrice;
        }

        /// <summary>
        /// Setter for the boolean identifier of the confirmation.
        /// </summary>
        /// <param name="is_v">the boolean identifier to set the valid_order to.</param>
        public void setIsValid(Boolean is_v) {
            this.valid_order = is_v;
        }

        /// <summary>
        /// Getter for the SenderID.
        /// </summary>
        /// <returns>The senderID, aka the TravelAgency ID</returns>
        public string getSenderID()
        {
            return this.senderID;
        }

        /// <summary>
        /// Getter for the receiverID.
        /// </summary>
        /// <returns>The ReceiverID, aka the Hotel ID</returns>
        public string getReceiverID()
        {
            return this.receiverID;
        }

        /// <summary>
        /// Getter for the card Number of the TravelAgency associated with this OrderObject.
        /// </summary>
        /// <returns>The integer representation of the credit card number.</returns>
        public int getCardNo()
        {
            return this.cardNo;
        }

        /// <summary>
        /// Getter for the total amount of this OrderObject.
        /// </summary>
        /// <returns>the total amount of this order.</returns>
        public decimal getAmount()
        {
            return this.amount;
        }

        /// <summary>
        /// Getter for the unitPrice each room was purchased at for this order.
        /// </summary>
        /// <returns>The unitPrice.</returns>
        public decimal getUnitPrice()
        {
            return this.unitPrice;
        }

        /// <summary>
        /// Getter function for the boolean confirmation identifier of this OrderObject.
        /// </summary>
        /// <returns>The boolean confirmation of this OrderObject.</returns>
        public Boolean isValid() {
            return this.valid_order;
        }

    }
}
