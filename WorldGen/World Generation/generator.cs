﻿using Microsoft.Xna.Framework.Graphics;
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
        private int[][] neighbourhood; // = neighbourhoods.moore;
        private int[,] buffer;
        private int neighbours;
        private int neighbourVal;
        private int cutoff = -1;
        public bool eq = false; // is the system in equilibrium? after each step this will be evaluated
        public int gens_eq = 0;
        public List<int[]> queue;
        public List<string> visited;
        public Generator(string name = "unnamed generator")
        {
            _name = name;
            rules = new List<int[]>();
            steps = new List<Func<int, int, int[,], int[,]>>();
            queue = new List<int[]>();
            visited = new List<string>();
        }
        public void push_rule(int[] _rule)
        {
            rules.Add(_rule);
        }
        public void push_action(Func<int, int, int[,], int[,]> function)
        {
            steps.Add(function);
        }
        public void set_neighbourhood(int[][] _neighbourhood) 
        {
            neighbourhood = _neighbourhood;
        }
        public void clear_steps()
        {
            steps.Clear();
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
        public int[,] execute_ca_floodfill(int _width, int _height, int[,] grid)
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
                        if (neighbourVals.Count != 0 && neighbourVals.Sum() > 0 && tools.rnd.Next(0, 10) > 5)
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
        public int[,] execute_traditional_floodfill(int _width, int _height, int[,] grid)
        {
            int[,] buffer = tools.gen_grid(_width, _height);
            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    buffer[x, y] = grid[x, y];
                }
            }
            if (queue.Count == 0)
            {
                return grid;
            }
            int steps_this_turn = queue.Count;
            for (int c = 0; c < steps_this_turn; c++)
            {
                int[] coords = queue[0];
                int i = coords[0];
                int j = coords[1];
                queue.RemoveAt(0);
                visited.Add($"{i},{j}");
                foreach (int[] neighbour in neighbourhood)
                {
                    int ni = i + neighbour[0];
                    int nj = j + neighbour[1];
                    if (ni >= 0 && ni < _width && nj >= 0 && nj < _height && !visited.Contains($"{ni},{nj}"))
                    {
                        
                        if (grid[i, j] < 0 && buffer[ni,nj] == 0)
                        {
                            buffer[ni, nj] = grid[i, j];
                            queue.Add(new int[] { ni, nj });
                        }
                        else if (buffer[ni, nj] > 0 && !tools.ListContains(queue, new int[] { ni, nj }) && grid[i,j] >= buffer[ni,nj])
                        {
                            //buffer[i, j] = grid[i, j];
                            //buffer[ni, nj] = grid[i, j] + tools.rnd.Next(0,3);
                            //queue.Add(new int[] { ni, nj });
                            if (tools.rnd.Next(0, 100) > 25)
                            {
                                visited.Add($"{ni},{nj}");
                                buffer[i, j] = grid[i, j];
                                buffer[ni, nj] = grid[i, j] +  tools.rnd.Next(0, 3);
                                //targets.Add(buffer[ni, nj]);
                                queue.Add(new int[] { ni, nj });
                            }
                            else
                            {
                                if (!visited.Contains($"{i},{j}"))
                                {
                                    queue.Add(new int[] { i, j });
                                }
                            }
                        }
                        //else if (tools.rnd.Next(0, 10) > -1)
                        //{
                        //    buffer[ni, nj] = grid[i, j] < 0 ? grid[i,j] : Math.Max(0, grid[i, j] + tools.rnd.Next(-1,3));
                        //    //targets.Add(buffer[ni, nj]);
                        //    queue.Add(new int[] { ni, nj });
                        //}

                    }
                }
            }

            
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
            any_changes(_width, _height, grid, gridNext);
            return gridNext;
        }
        private void any_changes(int _width, int _height, int[,] grid, int[,] gridNext)
        {
            int changes = 0;
            for (int i = 0; i < _width; i++)
            {
                for (int j = 0; j < _height; j++)
                {
                    if (grid[i,j] != gridNext[i,j])
                    {
                        changes++;
                        //gens_eq = 0;
                        //return;
                    }
                }
            }
            if (changes < 1)
            {
                gens_eq++;
            }
            else
            {
                gens_eq = 0;
            }
        }

        public void populate_queue_for_slope_fill(int _width, int _height, int[,] grid)
        {
            int neighbours;
            for (int i = 0; i < _width; i++)
            {
                for (int j = 0; j < _height; j++)
                {
                    neighbours = 0;
                    foreach (int[] neighbour in neighbourhood)
                    {
                        int ni = i + neighbour[0];
                        int nj = j + neighbour[1];
                        if (ni >= 0 && ni < _width &&
                            nj >= 0 && nj < _height)
                        {
                            if (grid[ni,nj] < 0)//== -1)//!= grid[i,j])
                            {
                                neighbours++;
                            }
                        }
                    }
                    if (neighbours != 0)
                    {
                        queue.Add(new int[] { i, j });                       
                    }
                }
            }
        }
        public int[,] mark_as_unsolved(int _width, int _height, int[,] grid)
        {
            int[,] buffer = tools.gen_grid(_width, _height);
            for (int i = 0; i < _width; i++)
            {
                for (int j = 0; j < _height; j++)
                {
                    if (grid[i,j] == 1)
                    {
                        if (tools.ListContains(queue, new int[] { i, j }))
                        {
                            buffer[i, j] = grid[i, j];
                        }
                        else
                        {
                            buffer[i, j] = 0;
                        }
                    }
                    else
                    {
                        buffer[i, j] = grid[i, j];
                    }
                }
            }
            return buffer;
        }
    }
}
