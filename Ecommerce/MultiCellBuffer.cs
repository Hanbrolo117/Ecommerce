using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;

    namespace Ecommerce
    {
        public class Cell
        {
            string encodedStr;
            int id;

            public Cell()
            {
                this.encodedStr = "";
                this.id = -1;
            }

            public string getString()
            {
                return this.encodedStr;
            }

            public int getID()
            {
                return this.id;
            }

            public void setCell(string message)
            {
                this.encodedStr = message;
            }
        }
        class MultiCellBuffer
        {
            Semaphore pool = new Semaphore(0, 3);
            Cell[] cellArray;

            public MultiCellBuffer()
            {
                for (int i = 0; i < 3; i++)
                    this.cellArray[i] = new Cell();
            }

            //Cell getOneCell()   { return Cell someCell; }
            //void setOneCell() { cellArray[someCell].encodedStr = someString;}
        }

    }

}
