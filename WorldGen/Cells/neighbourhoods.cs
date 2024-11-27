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
        public static int[][] cross = new int[][] //cities
        {
            new int[] { 1, 0 },
            new int[] { 0, 1 },
            new int[] { 0, -1 },
            new int[] { -1, 0 },
            new int[] { 2, 0 },
            new int[] { 0, 2 },
            new int[] { 0, -2 },
            new int[] { -2, 0 },
        };
        public static int[][] test = new int[][] //wetlands
        {
            new int[] { 1, 1 },
            new int[] { 1, -1 },
            new int[] { -1, -1 },
            new int[] { -1, 1 },
            new int[] { 2, 0 },
            new int[] { 0, 2 },
            new int[] { 0, -2 },
            new int[] { -2, 0 },
        };
        public static int[][] test2 = new int[][] //jagged
        {
            new int[] { 1, 0 },
            new int[] { 0, 1 },
            new int[] { 0, -1 },
            new int[] { -1, 0 },
            new int[] { 2, 2 },
            new int[] { -2, 2 },
            new int[] { 2, -2 },
            new int[] { -2, -2 },
        };
        public static int[][] test3 = new int[][] //floodplains
{
            new int[] { 2, 0 },
            new int[] { 0, 2 },
            new int[] { 0, -2 },
            new int[] { -2, 0 },
            new int[] { 2, 2 },
            new int[] { -2, 2 },
            new int[] { 2, -2 },
            new int[] { -2, -2 },
};
    }
}
