using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphApp.Model
{
    public class Pair
    {
        public object Item1 { get; private set; }

        public object Item2 { get; private set; }

        public Pair(object item1, object item2)
        {
            Item1 = item1;
            Item2 = item2;
        }
    }
}
