using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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


        public static void submitProcessedOrderObject(string encoded_order ,string travel_agency_id)
        {
            //TODO::Add processed order to the processed order multicellbuffer:
            //processed_order_objects.addObjectwithID(encoded_order, travel_agency_id);

            if (order_has_been_processed_emitter != null) {
                order_has_been_processed_emitter(travel_agency_id);//Notify TravelAgency Listener/Handler that a processed order has been added to the multicellBuffer.
            }
        }


        public void addOrderToProcessListener(Action<string> order_to_process_listener) {
            add_order_to_process_emitter += new addOrderToProcess(order_to_process_listener);
        }

        public void addOrderBeenProcessedListener(Action<string> order_been_processed_listener) {
            order_has_been_processed_emitter += new orderHasBeenProcessed(order_been_processed_listener);
        }

        public string orderProcessor(OrderObject orderObject) {
            return "";//TODO::Implement.
        }


        public static string getProcessedOrderObject(string travel_agency_id) {
            //return (string)processed_order_objects.getOrderByID(travel_agency_id);
        }
    }
}
