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
        string encodedStr;

        public Cell()
        {
            this.encodedStr = "";
        }

        public string getEncodedString()
        {
            return this.encodedStr;
        }

        public void setEncodedString(string message)
        {
            this.encodedStr = message;
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
        Semaphore pool = new Semaphore(0, 3);
        Cell[] cellArray = new Cell[3];

        //Contructor - Initializing cells
        public MultiCellBuffer()
        {
            for (int i = 0; i < 3; i++)
                this.cellArray[i] = new Cell();
        }

        /*
            * If a semaphore indicate free resorce, setOneCell will TryEnter each cell
            * until it sees a free cell and will lock that cell until it finish writting
            */        
        void setOneCell(string encodedMessage)
        {
            //wait for a resource to be free
            pool.WaitOne();

            //once there is a resource freeing up
            //try enter each cell to find an available cell
            for (int i = 0; i < 3; i++)
            {
                if (Monitor.TryEnter(cellArray[i]))          //if a cell in locked, move on to the next one
                {
                    cellArray[i].setEncodedString(encodedMessage);
                    Monitor.Exit(cellArray[i]);          //unlock the object after writing
                    pool.Release(1);
                    i = 4;
                }
            }
        }

        /*
            * If a semaphore indicate free resorce, setOneCell will TryEnter each cell
            * until it sees a free cell and will lock that cell until it finish reading
            * returns empty string if cell is free but doesn't have value in it
            */
        string getOneCell()
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
                    //if the cell have an order in it
                    if (cellArray[i].getEncodedString().Length != 0)
                    {
                        result = cellArray[i].getEncodedString();
                        cellArray[i].setEncodedString("");
                        pool.Release(1);
                        Monitor.Exit(cellArray[i]);
                        i = 4;
                    }
                    else
                    {
                        Monitor.Exit(cellArray[i]);
                        pool.Release(1);
                        i = 4;
                    }
                }
            }
            return result;
        }
    }
}
