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
        private int neighbourVal;
        private int neighbourMax;
        public Generator(string name = "unnamed generator")
        {
            _name = name;
            rules = new List<int[]>();
        }
        public void push_rule(int[] _rule)
        {
            rules.Add(_rule);
        }
        public void push_action(Action a)
        {
            actions.Add(a);
        }
        public int[,] execute_ca(int _width, int _height, int[,] grid, int cutoff = -1)
        {
            rule = rules[0];
            rules.RemoveAt(0);
            buffer = tools.gen_grid(_width, _height);
            for (int i = 0; i < _width; i++)
            {
                for (int j = 0; j < _height; j++)
                {
                    neighbours = 0;
                    neighbourMax = 0;
                    for (int n = 0; n < neighbourhood.Length; n++)
                    {
                        int[] neighbour = neighbourhood[n];
                        neighbourVal = grid[tools.mod(i + neighbour[0], _width), tools.mod(j + neighbour[1], _height)];
                        neighbours += neighbourVal != 0 ? 1 : 0;
                        neighbourMax = Math.Max(neighbourMax, neighbourVal);
                    }
                    if (rule[neighbours] != 2 && tools.rnd.Next(10) > cutoff)
                    {
                        buffer[i, j] = rule[neighbours] * neighbourMax;
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
