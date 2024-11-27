using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGen.Cells;
using System.Diagnostics;

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
        public static int[,] add_border(int width, int height, int[,] grid, int border, int borderVal)
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (i < border || i > width - border ||
                        j < border || j > height - border)
                    {
                        grid[i, j] = borderVal;
                    }
                }
            }
            return grid;
        }
        public static int[,] seed_grid(int width, int height, int seed, int seeds)
        {
            int[,] grid = gen_grid(width, height);
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    grid[i, j] = 0;
                }
            }
            int si, sj;
            for (int i = 0; i < seeds; i++)
            {
                si = rnd.Next(width);
                sj = rnd.Next(height);
                
                grid[si, sj] = seed == 1 ? 1 : i;
            }
            return grid;
        }
        public static int[,] rectangle(int width, int height)
        {
            int[,] grid = gen_grid(width, height);
            int width_border = width / 10;
            int height_border = height / 10;
            for (int i = width_border; i <= width - width_border; i++)
            {
                grid[i, height_border] = 1;
                grid[i, height - height_border] = 1;
            }
            for (int j = height_border; j <= height - height_border; j++)
            {
                grid[width_border, j] = 1;
                grid[width - width_border, j] = 1;
            }
            return grid;
        }
        public static int[,] circle(int width, int height)
        {
            int[,] grid = gen_grid(width, height);
            int width_mid = width / 2;
            int height_mid = height / 2;
            float border = 0.45f;
            int radius = (int)(width * border);
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    int di = width_mid - i;
                    int dj = height_mid - j;
                    if (Math.Pow(di,2) + Math.Pow(dj,2) >= Math.Pow(radius, 2) - (width * border) &&
                        Math.Pow(di, 2) + Math.Pow(dj, 2) <= Math.Pow(radius, 2) + (width * border))
                    {
                        grid[i, j] = 1;
                    }
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
        public static void save_rule(int[] rule)
        {
            string output = "[";
            for (int i = 0; i < 9; i++)
            {
                output += $"{rule[i]},";
            }
            output = output[..^1] + "]";
            using (StreamWriter w = File.AppendText("../../../Output/myFile.txt"))
            {
                w.WriteLine(output);
            }
        }
        public static int clamp_1bit(int value)
        {
            return value == 0 ? 0 : 1;
        }
    }
}
