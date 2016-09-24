using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecommerce.decryptionService;

namespace Ecommerce
{
    class BankService
    {
        private static Hashtable clients = new Hashtable();


        /// <summary>
        /// This function adds a new client to the banking system. This function should be called when a potential client applies for a card.
        /// </summary>
        /// <param name="clientID">The name of the client's account.</param>
        /// <param name="init_amount_to_deposit">The initial deposit the client would like to make to their new account with this banking system.</param>
        public static int addClient(String clientID, decimal init_amount_to_deposit) {
            int new_card_number = generateCreditcardNumber();
            Client new_client = new Client(clientID, new_card_number, init_amount_to_deposit);
            clients.Add(new_card_number, new_client);
            return new_card_number;
        }


        /// <summary>
        /// This function is for use of External Services. Provided a credit card number and an amount to charge to the credit card number, this function verifies the card, and the amount to charge.
        /// That is, provided the card exists, and the client associated to that card has sufficient funds, the client will be charged the specified amount, and the function will return a confirmation
        /// to the calling service.
        /// </summary>
        /// <param name="credit_card_number">The credit card number to charge.</param>
        /// <param name="amt_to_charge">the amount to charge to the account associated to the specified credit card number.</param>
        /// <returns>Returns a "valid" confirmation if the account was successfully charged, and "not valid" if the account was not succesfully charged.</returns>
        public static Boolean confirmCreditCard(String credit_card_number, String amt_to_charge) {
            //Decrypt Received data:
            String cc_decrypted_data = decryptReceivedData(credit_card_number);
            String amt_decrypted_data = decryptReceivedData(amt_to_charge);

            //Convert decrypted String data to integer representation of credit card number:
            Int32 cc_number = Convert.ToInt32(cc_decrypted_data);
            decimal amount_to_charge_cc = BankService.formatCurrency(Convert.ToDecimal(amt_decrypted_data));

            //Confirm Valid account with provided credit card number:
            if (clients.ContainsKey(cc_number)) {
                //If Client has sufficient funds, charge the client's account and return a valid confirmation:
                if (withdrawFromClientAmount(amount_to_charge_cc, cc_number)) {
                    //Return valid confirmation:
                    return true;
                }                
            }

            //Else return not valid confirmation:
            return false;
        }

        /// <summary>
        /// This function uses the decryption service in the ASU repository to decrypt data that has been received by some other service in the E-Commerce.
        /// </summary>
        /// <param name="data_to_decrypt">The encrypted string received by some other service in the E-Commerce ecosystem.</param>
        /// <returns>The decrypted string of the encrypted string that was passed in.</returns>
        private static String decryptReceivedData(String data_to_decrypt) {
            //decrpyt Data received using the decryption service in the ASU repository. Return decrypted String data
            Service ds = new Service();
            return ds.Decrypt(data_to_decrypt);
        }


        /// <summary>
        /// This function is used to maintain consistency of currency format throughout currency processes in the banking system, from amount checking, to amount manipulation. 
        /// </summary>
        /// <param name="amount_to_format">The decimal of an amount to format</param>
        /// <returns>formatted decimal value of the decimal amount passed in.</returns>
        public static decimal formatCurrency(decimal amount_to_format) {
            return decimal.Round(amount_to_format, 2, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// This Function generates a unique credit Card Number for new Clients to be added to the banking system.
        /// </summary>
        /// <returns>A new credit card number for a new client in the banking system.</returns>
        private static int generateCreditcardNumber()
        {
            Random rando = new Random();
            int new_card_number = -1;
            do {
                new_card_number = rando.Next(100000000,1000000000);//Attempt to assign new credit card number
            }
            while (clients.ContainsKey(new_card_number));//Keep looping until a unique card number for the new client is successfully generated

            return new_card_number;//return newly generated credit card number.
        }

        /// <summary>
        /// This Function takes in the amount to deposit into a Client's account that resides in the Bank System. Client is specified by a clientID.
        /// </summary>
        /// <param name="amount_to_deposit">The amount to deposit to a Client's account in the Bank's System.</param>
        /// <param name="cc_number">The valid credit card number to specify which account in the banking system to deposit the amount to.</param>
        public static Boolean depositToClientAmount(decimal amount_to_deposit, int cc_number) {
            //Get client associated with the given credit card number, to deposit to:
            Client client_to_deposit = (Client)clients[cc_number];

            //Deposit amount to client's account:
            client_to_deposit.setClientAmount((client_to_deposit.getClientAmount() + amount_to_deposit));

            //Return true to signal a successful deposit:
            return true;
        }

        
        /// <summary>
        /// This Function takes in the amount to bewithdrawn from a Client, specified by its clientID.
        /// </summary>
        /// <param name="amount_to_withdrawal">The amount to withdraw from a Client in the Bank System.</param>
        /// <param name="cc_number">A valid credit card number that resides in the Bank System.</param>
        /// <returns>True if the withdrawal process was a success. False if it failed.</returns>
        public static Boolean withdrawFromClientAmount(decimal amount_to_withdrawal, int cc_number) {
            //Get client associated with the credit card number:
            Client client_to_withdrawal = (Client)clients[cc_number];

            if (amount_to_withdrawal < client_to_withdrawal.getClientAmount())
            {
                //Withdrawal from account:
                client_to_withdrawal.setClientAmount((client_to_withdrawal.getClientAmount() - amount_to_withdrawal));
                return true;
            }
            //Else, return false. Withdrawal failed due to insufficient funds.
            return false; 
        }





        /// <summary>
        /// This private class is for use of the Bank Service class only. This Client is for use of the bank to track it's current Customers/clients and their current standing with the Bank
        /// from their valid credit card number, to the amount that they have stored in the bank system.
        /// </summary>
        private class Client {
            private String clientID;
            private int cardNo;
            private decimal amount;


            /// <summary>
            /// This is an args constructor of the private Client class in the BankService class. This function constructs a new Client, provided a new credit card number, client ID, and an initial amount.
            /// </summary>
            /// <param name="cId">The new Client's ID that the banking system uses to identify the customer/client with.</param>
            /// <param name="cNumber">The new client's credit card number, which will be used for various transactions.</param>
            /// <param name="amt">The currecny amount that the client has in the banking system.</param>
            public Client(String cId, int cNumber, decimal amt) {
                this.clientID = cId;
                this.cardNo = cNumber;
                this.amount = BankService.formatCurrency(amt);
            }


            /// <summary>
            /// This Getter function returns the client Identification String of a client to this bank.
            /// </summary>
            /// <returns>This String format of a Client ID.</returns>
            public String getClientID() {
                return this.clientID;
            }


            /// <summary>
            /// This Getter Function returns the credit card number of a Client to this bank.
            /// </summary>
            /// <returns>The integer format of a credit card number of a Client to this bank.</returns>
            public int getClientCardNo() {
                return this.cardNo;
            }


            /// <summary>
            /// This Getter function returns the total amount a Client has in the Bank in the proper format (to 2 decimal places).
            /// </summary>
            /// <returns>The decimal amount a Client has in the bank.</returns>
            public decimal getClientAmount() {
                return BankService.formatCurrency(this.amount);//properly round decimal value for accuracy whilst handling currency.
            }

            /// <summary>
            /// This Setter Function updates the Client's Identifier.
            /// </summary>
            /// <param name="id">String representation of the new Client ID.</param>
            public void setClientID(String id) {
                this.clientID = id;
            }

            /// <summary>
            /// This Setter Function updates the client's credit card number.
            /// </summary>
            /// <param name="cNumber">integer representation of the new credit card number.</param>
            public void setClientCardNo(int cNumber) {
                this.cardNo = cNumber;
            }


            /// <summary>
            /// This Setter Function updates the decimal amount of a clients monetary value in the bank.
            /// </summary>
            /// <param name="amt">New decimal value to set the Client's amount attribute to.</param>
            public void setClientAmount(decimal amt) {
                this.amount = decimal.Round(amt, 2, MidpointRounding.AwayFromZero);
            }
        }


    }


}

