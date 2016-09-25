using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections;
using Ecommerce;

namespace Ecommerce
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            const int NUM_OF_HOTELS = 3;
                const int NUM_OF_TRAVEL_AGENCIES = 5;

                //Print Out the E-Commerce ecosystem setup:
                //--------------------------------------------------------------------------------
                Console.WriteLine("E-Commerce System:");
                Console.WriteLine("Number of Hotels: {0}", NUM_OF_HOTELS);
                Console.WriteLine("Number of Travel Agencies: {0}", NUM_OF_TRAVEL_AGENCIES);
                Console.WriteLine();
                //--------------------------------------------------------------------------------

                Console.WriteLine("Simply press the enter key to run ecommerce simulation.\nEnter \"esc\" to cancel start of ecommerce");
                Console.Write(": ");
                string input = Console.ReadLine();
                if (input == "esc")
                {
                    Console.WriteLine("Exiting E-Commerce Simulation Program...");  //Notify User that application is exiting.
                    System.Environment.Exit(0);                                     //Safely Exit main program/function.
                }
                //Otherwise continue on with the Simulation:

                ArrayList travel_agencies = new ArrayList();
                ArrayList hotel_threads = new ArrayList();

                Random rand = new Random();                     //Create a random number generator to randomly select the amount of money a travel agency with set up their bank account with.

                //Generate all of the Travel Agencies. have them apply for a new account with the bank, and finally, store them in the travel agency list:
                for (int i = 0; i < NUM_OF_TRAVEL_AGENCIES; i++)
                {

                    //Create a new travel agency, giving it it's own unique id:
                    TravelAgency new_agency = new TravelAgency("ta_" + i);

                    //Have the new travel agency apply for a new credit card number with the bank. The bank will create an account for the travel agency with the amount they deposited and
                    //issue them a new credit card number. Set the new agency's credit card number to this issued value returned by the Bnak's addClient function.:
                    new_agency.setCCNumber(BankService.addClient(new_agency.getID(), rand.Next(30000, 80001)));

                    //Finally, store the new agency in the travel agenct arraylist:
                    travel_agencies.Add(new_agency);
                }//END FOR LOOP.


                //Generate all of the Hotels, and have each of the travel agencies subscribe to each of the Hotel's priceCut events, finally store each hotel as a new thread in the hotels arraylist:
                for (int i = 0; i < NUM_OF_HOTELS; i++)
                {
                    //Create a new hotel giving it a unique id:
                    Hotel new_hotel = new Hotel("ht_" + i);

                    //For each travel agency, have it subscribe to this new hotel's price cut event:
                    for (int t = 0; t < NUM_OF_TRAVEL_AGENCIES; t++)
                    {
                        new_hotel.subscribeHandlerToPriceCutEvents(((TravelAgency)travel_agencies[t]).hotelPriceBeenCut);
                    }

                    //Store the new Hotel as a new thread in the arraylist:
                    hotel_threads.Add(new Thread(new_hotel.hotelFunc));
                }//END FOR LOOP.

                //Once Everything has been initialized:

                //Start all of the Hotel Threads that were setup and stored in an arraylist earlier:
                for (int i = 0; i < hotel_threads.Count; i++)
                {
                    Console.WriteLine("Starting hotel thread id: {0}",i);
                    ((Thread)hotel_threads[i]).Start();//Call the thread's Start() function.
                }


                //Finally, as the Threads finish, join:
                for (int i = 0; i < hotel_threads.Count; i++)
                {
                    ((Thread)hotel_threads[i]).Join();//Call the thread's Join() function.
                }

                //Give a good enough to for all the threads to finish up and close out:
                Thread.Sleep(1000);
                Console.WriteLine("Program Terminating...");
            }
    }
}
