using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGen.Cells;

namespace WorldGen.General
{
    internal static class tools
    {
        public static Random rnd = new Random();
        public static int[,] gen_grid(int width, int height)
        {
            return new int[width, height];
        }

        public static int[,] swap_buffer(int[,] buffer)
        {
            int width = buffer.GetLength(0);
            int height = buffer.GetLength(1);
            int[,] grid = gen_grid(width, height);
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    grid[i, j] = buffer[i, j];
                }
            }
            return grid;
        }
        public static int[,] randomise_grid(int width, int height)
        {
            int[,] grid = gen_grid(width, height);
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    grid[i, j] = rnd.Next(2);
                }
            }
            return grid;
        }
        public static int mod(int x, int y)
        {
            int rem = x % y;
            return rem + (((x ^ y) < 0) ? y : 0);
        }
        public static string print_grid(int[,] grid)
        {
            char[] glyphs = new char[] { '.', '#' };
            string output = "";
            int width = grid.GetLength(0);
            int height = grid.GetLength(1);
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    output += glyphs[grid[i, j]];
                }
                output += "\n";
            }
            return output;
        }
    }
}
