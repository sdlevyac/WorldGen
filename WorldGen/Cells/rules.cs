using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGen.General;

namespace WorldGen.Cells
{
    internal static class rules
    {
        //2 state cellular automata
        //neighbours are counted (based on a specific neighbourhood)
        //and the neighbour count is used to index the rule array
        //0 -> become 0
        //1 -> become 1
        //2 -> no change
        public static int[] conway = new int[] { 0, 0, 2, 1, 0, 0, 0, 0, 0 };

        //testing...
        public static int[] flood = new int[] { 2, 1, 1, 1, 1, 1, 1, 1, 1 };

        public static int[] random_rules()
        {
            //in 1984...
            return new int[9].Select(i => tools.rnd.Next(3)).ToArray();
        }
    }
}
