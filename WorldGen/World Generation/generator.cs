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
        private List<Action> actions;
        private int[] rule;
        private int[][] neighbourhood = neighbourhoods.moore;
        private int[,] buffer;
        private int neighbours;
        public Generator(string name = "unnamed generator")
        {
            _name = name;
            rules = new List<int[]>();
        }
        public void push_rule(int[] _rule)
        {
            rules.Add(_rule);
        }
        public int[,] execute(int _width, int _height, int[,] grid)
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
                        neighbours += grid[tools.mod(i + neighbour[0], _width), tools.mod(j + neighbour[1], _height)];
                    }
                    if (rule[neighbours] != 2)
                    {
                        buffer[i, j] = rule[neighbours];
                    }
                    else
                    {
                        buffer[i, j] = grid[i, j];
                    }
                }
            }
            grid = tools.swap_buffer(buffer);
            return grid;
        }
    }
}
