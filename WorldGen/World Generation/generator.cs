using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGen.Cells;
using WorldGen.General;

namespace WorldGen.World_Generation
{
    internal class Generator
    {
        private string _name;
        private List<int[]> rules;
        private List<Func<int, int, int[,], int[,]>> steps;
        private int[] rule;
        private int[][] neighbourhood = neighbourhoods.moore;
        private int[,] buffer;
        private int neighbours;
        private int neighbourVal;
        private int cutoff = -1;
        public Generator(string name = "unnamed generator")
        {
            _name = name;
            rules = new List<int[]>();
            steps = new List<Func<int, int, int[,], int[,]>>();
        }
        public void push_rule(int[] _rule)
        {
            rules.Add(_rule);
        }
        public void push_action(Func<int, int, int[,], int[,]> function)
        {
            steps.Add(function);
        }
        public int[,] execute_ca(int _width, int _height, int[,] grid)
        {
            rule = rules[0];
            rules.RemoveAt(0);
            buffer = tools.gen_grid(_width, _height);
            for (int i = 0; i < _width; i++)
            {
                for (int j = 0; j < _height; j++)
                {
                    neighbours = 0;
                    for (int n = 0; n < neighbourhood.Length; n++)
                    {
                        int[] neighbour = neighbourhood[n];
                        neighbourVal = grid[tools.mod(i + neighbour[0], _width), tools.mod(j + neighbour[1], _height)];
                        neighbours += neighbourVal != 0 ? 1 : 0;
                    }
                    if (rule[neighbours] != 2 && tools.rnd.Next(10) > cutoff)
                    {
                        buffer[i, j] = rule[neighbours];
                    }
                    else
                    {
                        buffer[i, j] = grid[i, j];
                    }
                }
            }
            //grid = tools.swap_buffer(buffer);
            return buffer;
        }
        public int[,] execute_floodfill(int _width, int _height, int[,] grid)
        {
            buffer = tools.gen_grid(_width, _height);
            List<int> neighbourVals = new List<int>();
            for (int i = 0; i < _width; i++)
            {
                for (int j = 0; j < _height; j++)
                {
                    if (grid[i,j] == 0)
                    {
                        neighbours = 0;
                        neighbourVals = new List<int>();
                        for (int n = 0; n < neighbourhood.Length; n++)
                        {
                            int[] neighbour = neighbourhood[n];
                            neighbourVal = grid[tools.mod(i + neighbour[0], _width), tools.mod(j + neighbour[1], _height)];
                            if (neighbourVal != 0)
                            {
                                neighbourVals.Add(neighbourVal);
                            }
                        }
                        if (neighbourVals.Count != 0 && tools.rnd.Next(0, 10) > 5)
                        {
                            buffer[i, j] = neighbourVals[tools.rnd.Next(0, neighbourVals.Count)];
                        }
                    }
                    else
                    {
                        buffer[i, j] = grid[i, j];
                    }
                }
            }
            //grid = tools.swap_buffer(grid);
            return buffer;
        }
        public int[,] step(int _width, int _height, int[,] grid)
        {
            if (steps.Count == 0)
            {
                return grid;
            }
            int[,] gridNext = steps[0](_width, _height, grid);
            steps.RemoveAt(0);
            return gridNext;
        }
    }
}
