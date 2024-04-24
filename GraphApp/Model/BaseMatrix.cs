using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphApp.Model
{
    public abstract class BaseMatrix : ICloneable
    {
        public string[] ColumnsDescription { get; protected set; }

        public string[] RowsDescription { get; protected set; }

        public int[,] Matrix { get; protected set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
