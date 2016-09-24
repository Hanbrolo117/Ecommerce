using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecommerce.decryptionService;
using System.Threading;

namespace Ecommerce
{
    class OrderProcessing
    {

        private delegate void addOrderToProcess(string hotel_id);
        private delegate void orderHasBeenProcessed(string travel_agency_id);

        private static event addOrderToProcess add_order_to_process_emitter;
        private static event orderHasBeenProcessed order_has_been_processed_emitter;

        private static Ecommerce.MultiCellBuffer order_object_to_process = new Ecommerce.MultiCellBuffer();
        private static Ecommerce.MultiCellBuffer processed_order_objects = new Ecommerce.MultiCellBuffer();

        private const decimal SALES_TAX = 0.08M;

        public static void submitProcessedOrderObject(string encoded_order ,string travel_agency_id)
        {
            //TODO::Add processed order to the processed order multicellbuffer:
            //processed_order_objects.addObjectwithID(encoded_order, travel_agency_id);

            if (order_has_been_processed_emitter != null) {
                order_has_been_processed_emitter(travel_agency_id);//Notify TravelAgency Listener/Handler that a processed order has been added to the multicellBuffer.
            }
        }


        public static void submitOrderToProcess(string encoded_order, string hotel_id) {
            //TODO::Add processed order to the processed order multicellbuffer:
            //order_object_to_process.addObjectWithId(encoded_order, hotel_id);

            if (add_order_to_process_emitter != null) {
                add_order_to_process_emitter(hotel_id);//Notify Hotel Listener/Handler(s) that an order that needs to be processed has been added to the multiCellBuffer.
            }
        }


        public static void addOrderToProcessListener(Action<string> order_to_process_listener) {
            add_order_to_process_emitter += new addOrderToProcess(order_to_process_listener);
        }

        public static void addOrderBeenProcessedListener(Action<string> order_been_processed_listener) {
            order_has_been_processed_emitter += new orderHasBeenProcessed(order_been_processed_listener);
        }

        public static string orderProcessor(string encoded_order_object) {
            
            //Decode string into an object:
            OrderObject new_order_object = EnDecoder.Decode(encoded_order_object);
            
            //Update total amount to account for Sales tax:
            decimal total = BankService.formatCurrency((new_order_object.getAmount() + (new_order_object.getAmount() * SALES_TAX)));
            new_order_object.setAmount(total);

            //Create an encryption service via ASU's Repo:
            Service encryption = new Service();

            //Encrypt the credit card number:
            string encrypted_cc_number = encryption.Encrypt(Convert.ToString(new_order_object.getCardNo()));
            
            //encrypt the total amount to charge the account:
            string encrypted_amount = encryption.Encrypt(Convert.ToString(new_order_object.getAmount()));

            //Have Bank validate the Account charge given the encrypted credit card number and amount to charge:
            new_order_object.setIsValid( BankService.confirmCreditCard(encrypted_cc_number, encrypted_amount));

            //Get the Travel_agencies ID:
            string travel_agency_id = new_order_object.getSenderID();

            //Encode the Processed Order Object:
            string encoded_processed_order = EnDecoder.Encode(new_order_object);

            //Create a new thread to handle the processed order:
            Thread process_order_thread = new Thread(() => OrderProcessing.submitProcessedOrderObject(encoded_processed_order, travel_agency_id) );

            return "";//TODO::Implement.
        }


        public static string getProcessedOrderObject(string travel_agency_id) {
            //return (string)processed_order_objects.getOrderById(travel_agency_id);
        }

        public static string getOrderToProcess(string hotel_id) {
            //return (string)order_object_to_process.getOrderById(hotel_id);
        }
    }
}
