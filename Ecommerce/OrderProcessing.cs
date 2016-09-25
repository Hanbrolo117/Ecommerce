using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecommerce.decryptionService;
using System.Threading;
using Ecommerce;

namespace Ecommerce
{
    class OrderProcessing
    {
        //OrderProcessing Attributes:
        //-------------------------------------------------------------------------------------------------------
        private delegate void addOrderToProcess(string hotel_id);
        private delegate void orderHasBeenProcessed(string travel_agency_id);

        private static event addOrderToProcess add_order_to_process_emitter;
        private static event orderHasBeenProcessed order_has_been_processed_emitter;

        private static MultiCellBuffer order_object_to_process = new MultiCellBuffer();
        private static MultiCellBuffer processed_order_objects = new MultiCellBuffer();

        private const decimal SALES_TAX = 0.08M;
        //-------------------------------------------------------------------------------------------------------


        /// <summary>
        /// This function submits an encoded OrderObject that has been processed, with the Travel Agency's id as it's key pair to be added to this OrderProcessed MultiCellBuffer.
        /// </summary>
        /// <param name="encoded_order">The string encoded order object.</param>
        /// <param name="travel_agency_id">the travelAgency's id to be mapped to the encoded string with.</param>
        public static void submitProcessedOrderObject(string encoded_order ,string travel_agency_id)
        {
            //Add processed order to the processed order multicellbuffer:
            processed_order_objects.setOneCell(encoded_order, travel_agency_id);

            //Notify TravelAgency Listener/Handler that a processed order has been added to the multicellBuffer:
            order_has_been_processed_emitter?.Invoke(travel_agency_id);
        }


        /// <summary>
        /// This function submits an encoded OrderObject that needs to be processed, with the Hotel id as it's key pair to be added to this toBeOrderedProcess MultiCellBuffer.
        /// </summary>
        /// <param name="encoded_order">The string encoded OrderObject.</param>
        /// <param name="hotel_id">the Hotel's id to be mapped to the encoded string with.</param>
        public static void submitOrderToProcess(string encoded_order, string hotel_id) {
           
            //Add processed order to the processed order multicellbuffer:
            order_object_to_process.setOneCell(encoded_order, hotel_id);

            Console.WriteLine("Processing OrderObject for Hotel {0}", hotel_id);
            if (add_order_to_process_emitter == null) {
                Console.WriteLine("\nUH-OH\n");
            }
            //Notify Hotel Listener/Handler(s) that an order that needs to be processed has been added to the multiCellBuffer:
            add_order_to_process_emitter?.Invoke(hotel_id);
        }


        /// <summary>
        /// Add a Listener/Handler to the subscription list of the addOrderToProcess delegate.
        /// </summary>
        /// <param name="order_to_process_listener">the listener/Handler function to subscribe to addOrderToProcess delegate.</param>
        public static void addOrderToProcessListener(Action<string> order_to_process_listener) {
            add_order_to_process_emitter += new addOrderToProcess(order_to_process_listener);//Subscribe!!
        }


        /// <summary>
        /// Add a Listener/Handler to the subscription list of the orderHasBeenProcessed delegate.
        /// </summary>
        /// <param name="order_been_processed_listener">the listener/Handler function to subscribe to orderHasBeenProcessed delegate.</param>
        public static void addOrderBeenProcessedListener(Action<string> order_been_processed_listener) {
            order_has_been_processed_emitter += new orderHasBeenProcessed(order_been_processed_listener);//Subscribe!!
        }


        /// <summary>
        /// The Function that actually processes an order, Validates Order with the BankService and returns a confirmation.
        /// </summary>
        /// <param name="encoded_order_object">The encoded OrderObject string to process as an OrderObject, once it is decoded of course.</param>
        public static void orderProcessor(string encoded_order_object) {
            
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
            Thread processed_order_thread = new Thread(() => OrderProcessing.submitProcessedOrderObject(encoded_processed_order, travel_agency_id) );

            //Start the thread:
            processed_order_thread.Start();
        }


        /// <summary>
        /// This function accesses the processedOrders MultiCellBuffer and attempts to retrieve an encoded OrderObject that is mapped to a key
        /// that is the same value as the travel agency's id.
        /// </summary>
        /// <param name="travel_agency_id">The travelAgency's ID to find the associated encoded OrderObject string in the ProcessedOrders MultiCellBuffer.</param>
        /// <returns>The string of the encoded OrderObject that is mapped to the passed in TravelAgency Id.</returns>
        public static string getProcessedOrderObject(string travel_agency_id) {
            return processed_order_objects.getOneCell(travel_agency_id);
        }


        /// <summary>
        /// This function accesses the rdersToBeProcessed MultiCellBuffer and attempts to retrieve an encoded OrderObject that is mapped to a key
        /// that is the same value as the Hotel's ID.
        /// </summary>
        /// <param name="hotel_id">The string of the encoded OrderObject that is mapped to the passed in Hotel Id.</param>
        /// <returns></returns>
        public static string getOrderToProcess(string hotel_id) {
            return order_object_to_process.getOneCell(hotel_id);
        }
    }
}
