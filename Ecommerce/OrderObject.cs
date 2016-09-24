using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce
{
    class OrderObject
    {

        private String senderID;
        private String receiverID;
        private int cardNo;
        private decimal amount;
        private decimal unitPrice;
        private Boolean valid_order;

        public OrderObject() {
            this.senderID = null;
            this.receiverID = null;
            this.cardNo = 0;
            this.amount = 0;
            this.unitPrice = 0;
            this.valid_order = false;
        }

        //Sender = travel Agency, receiver = hotel
        public OrderObject(string sd_id, string rcvr_id, int cc_number, decimal amt, decimal ut_price, Boolean is_v) {
            this.senderID = sd_id;
            this.receiverID = rcvr_id;
            this.cardNo = cc_number;
            this.amount = amt;
            this.unitPrice = ut_price;
            this.valid_order = is_v;
        }

        public void setSenderID(string senderID)
        {
            this.senderID = senderID;
        }

        public void setReceiverID(string receiverID)
        {
            this.receiverID = receiverID;
        }

        public void setCardNo(int cardNo)
        {
            this.cardNo = cardNo;
        }

        public void setAmount(decimal amount)
        {
            this.amount = amount;
        }

        public void setUnitPrice(decimal unitPrice)
        {
            this.unitPrice = unitPrice;
        }

        public void setIsValid(Boolean is_v) {
            this.valid_order = is_v;
        }

        public string getSenderID()
        {
            return this.senderID;
        }

        public string getReceiverID()
        {
            return this.receiverID;
        }

        public int getCardNo()
        {
            return this.cardNo;
        }

        public decimal getAmount()
        {
            return this.amount;
        }

        public decimal getUnitPrice()
        {
            return this.unitPrice;
        }

        public Boolean isValid() {
            return this.valid_order;
        }

    }
}
