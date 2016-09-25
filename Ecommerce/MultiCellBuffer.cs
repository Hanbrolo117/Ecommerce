using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Ecommerce
{
    /**
        *  Cell
        *  Fields: encoded string and an id number
        *  Functions: getters and setters
        **/
    public class Cell
    {
        private string ID;
        private string message;

        public Cell()
        {
            this.message = "";
        }

        public string getMessage()
        {
            return this.message;
        }

        public void setMessage(string message)
        {
            this.message = message;
        }

        public string getID()
        {
            return this.ID;
        }

        public void setID(string ID)
        {
            this.ID = ID;
        }
    }

    /**
        * MultiCellBuffer
        * Fields:
        *  +Semaphore pool: keep track of available cell to be read or written
        *  +cellArray: array of 3 cells
        * Fucntions:
        *  +void setOneCell(string encodedMessage): set cell's encoded message
        *  +string getOneCell(): get cell cencoded message
        */
    public class MultiCellBuffer
    {
        Semaphore pool = new Semaphore(3, 3);
        Cell[] cellArray = new Cell[3];

        //Contructor - Initializing cells
        public MultiCellBuffer()
        {
            for (int i = 0; i < 3; i++)
                 cellArray[i] = new Cell();
        }

        /*
            * If a semaphore indicate free resorce, setOneCell will TryEnter each cell
            * until it sees a free cell and will lock that cell until it finish writting
            */        
        public void setOneCell(string message, string ID)
        {
            //wait for a resource to be free
            pool.WaitOne();
            
            //once there is a resource freeing up
            //try enter each cell to find an available cell
            for (int i = 0; i < 3; i++)
            {
                if (Monitor.TryEnter(cellArray[i]))          //if a cell in locked, move on to the next one
                {
                    cellArray[i].setID(ID);
                    cellArray[i].setMessage(message);
                    Monitor.Exit(cellArray[i]);          //unlock the object after writing
                    pool.Release(1);
                    break;
                }
            }
            Console.WriteLine("\nFinished Writing: {0} \nFor cell for ID: {1}\n",message, ID);
            Thread.Sleep(30000);
        }

        /*
            * If a semaphore indicate free resorce, setOneCell will TryEnter each cell
            * until it sees a free cell and will lock that cell until it finish reading
            * returns empty string if cell is free but doesn't have value in it
            */
        public string getOneCell(string ID)
        {
            string result = "";
            //wait for a resource to be free
            pool.WaitOne();

            //once there is a resource freeing up
            //try enter each cell to find an available cell
            for (int i = 0; i < 3; i++)
            {
                if (Monitor.TryEnter(cellArray[i]))
                {
                    //if the cell have the same ID
                    if (cellArray[i].getID().CompareTo(ID) == 0) 
                    {
                        result = cellArray[i].getMessage();
                        cellArray[i].setID("");
                        cellArray[i].setMessage("");
                        pool.Release(1);
                        Monitor.Exit(cellArray[i]);
                        break;
                    }
                    else
                    {
                        Monitor.Exit(cellArray[i]);
                        pool.Release(1);
                        break;
                    }
                }
            }
            return result;
        }
    }
}
