using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldGen.Cells
{
    internal static class neighbourhoods
    {
        public static int[][] moore = new int[][] {
            new int[] { 1, 1 },
            new int[] { 1, 0 },
            new int[] { 1, -1 },
            new int[] { 0, 1 },
            new int[] { 0, -1 },
            new int[] { -1, 1 },
            new int[] { -1, 0 },
            new int[] { -1, -1 }
        };
        public static int[][] von_neumann = new int[][] {
            new int[] { 1, 0 },
            new int[] { 0, 1 },
            new int[] { 0, -1 },
            new int[] { -1, 0 },
        };
    }
}
