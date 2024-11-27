using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using System.Linq;
using WorldGen.Cells;
using WorldGen.General;
using WorldGen.World_Generation;
using WorldGen.Drawing;
using System.Collections.Generic;

namespace WorldGen
{
    public class Main : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private KeyboardState _currentKeyboardState;
        private KeyboardState _previousKeyboardState;
        private bool button_pressed;

        private bool rule_saved = false;

        private int _width = 250;
        private int _height = 250;
        private Generator generator;
        private int generation = 0;

        private static Texture2D rect;
        private int _pixelWidth;// = 16;
        private string colourMode = "regions";
        private Dictionary<string, Action<int, int, int, int, SpriteBatch, Texture2D>> colourModes;

        private int[] rule = rules.caves;//rules.conway; //rules.random_rules();
        //private int[][] neighbourhood = neighbourhoods.moore;
        private int[,] grid;

        private int neighbours;

        public Main()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _pixelWidth = 2;
            button_pressed = false;
            // TODO: Add your initialization logic here
            //tools.seed_grid(_width, _width, 0, 12);//randomise_grid(_width, _height);
            generator = new Generator("test");
            generator.set_neighbourhood(neighbourhoods.moore);
            //generator.push_rule(rule);

            colourModes = new Dictionary<string, Action<int, int, int, int, SpriteBatch, Texture2D>>();
            colourModes["1bit"] = drawing.draw_cell_1bit;
            colourModes["gradient"] = drawing.draw_cell_gradient;
            colourModes["regions"] = drawing.draw_cell_region;

            //basic_rectangle_flood();
            //testing_circle_flood();
            // random fill on screen
            // apply gap of 0s about 5-ish wide around the edge
            // apply "cave" stabilising rule (like [0,0,0,0,1,2,2,2,2])
            // one equilibrium is reached - flood fill from top left to find "ocean"
            // find "shoreline" and add each cell to queue
            // flood-fill-slope from edge of "islands"
            grid = tools.randomise_grid(_width, _height);
            grid = tools.add_border(_width, _height, grid, 25, 0);
            generator.push_rule(rule);
            generator.push_action(generator.execute_ca);

            TargetElapsedTime = TimeSpan.FromSeconds(1d / 30d);
            _graphics.IsFullScreen = false;
            _graphics.PreferredBackBufferWidth = _width * _pixelWidth;
            _graphics.PreferredBackBufferHeight = _height * _pixelWidth;
            _graphics.ApplyChanges();
            base.Initialize();
        }
        private void basic_rectangle_flood()
        {
            grid = tools.rectangle(_width, _height);
            generator.push_action(generator.execute_traditional_floodfill);
            int x = 0; //_width / 2; // 0;
            int y = 0; // _height / 2; //0;
            grid[x, y] = -1;
            generator.queue.Add(new int[] { x, y });
        }
        private void testing_circle_flood()
        {
            grid = tools.circle(_width, _height);
            generator.push_action(generator.execute_traditional_floodfill);
            int x = 0;
            int y = 0;
            grid[x,y] = -1;
            generator.queue.Add(new int[] { x, y });
        }
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            rect = new Texture2D(GraphicsDevice, 1, 1);
            rect.SetData(new[] { Color.White });
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _currentKeyboardState = Keyboard.GetState();

            // Check if "R" was just pressed
            if (IsKeyPressed(Keys.G))
            {
                randomise_grid();
            }
            if (IsKeyPressed(Keys.R))
            {
                randomise_rule();
            }
            if (IsKeyPressed(Keys.E))
            {
                seed_grid();
            }
            if (IsKeyPressed(Keys.S) && !rule_saved)
            {
                save_rule();
            }
            if (IsKeyPressed(Keys.D))
            {
                if (IsKeyPressed(Keys.D1))
                {
                    colourMode = "1bit";
                }
                else if (IsKeyPressed(Keys.D2))
                {
                    colourMode = "regions";
                }
                else if (IsKeyPressed(Keys.D3))
                {
                    colourMode = "gradient";
                }
                //if (colourMode == "1bit")
                //{
                //    colourMode = "gradient";
                //}
                //else if (colourMode == "gradient")
                //{
                //    colourMode = "regions";
                //}
                //else
                //{
                //    colourMode = "1bit";
                //}
            }

            // TODO: Add your update logic here
            generation = (generation + 1) % 5;
            grid = generator.step(_width, _height, grid);
            //generator.push_action(generator.execute_ca);


            //do this until islands are formed, then find the shore and execute flood fill w slope solver
            generator.push_rule(rule);
            generator.push_action(generator.execute_ca);
            Debug.WriteLine($"equilibrium for {generator.gens_eq} gens");

            //generator.push_action(generator.execute_traditional_floodfill);
            //if (generator.gens_eq > 10)
            //{               
            //    for (int i = 0; i < _width; i++)
            //    {
            //        for (int j = 0; j < _width; j++)
            //        {
            //            if (grid[i, j] == 1)
            //            {
            //                generator.queue.Add(new int[] { i, j });

            //            }
            //        }
            //    }
            //    generator.push_action(generator.execute_traditional_floodfill);
            //}           
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();
            // TODO: Add your drawing code here
            for (int i = 0; i < _width; i++)
            {
                for (int j = 0; j < _height; j++)
                {
                    colourModes[colourMode](i, j, grid[i, j], _pixelWidth, _spriteBatch, rect); //draw_cell_1bit(i, j, grid[i, j]);
                }
            }
            _spriteBatch.End();
            base.Draw(gameTime);
        }
        private void randomise_grid()
        {
            grid = tools.randomise_grid(_width, _height);
        }
        private void seed_grid()
        {
            grid = tools.seed_grid(_width, _height, 0, 50);
        }
        private void randomise_rule()
        {
            rule = rules.random_rules();
            rule_saved = false;
        }
        private void save_rule()
        {
            tools.save_rule(rule);
            rule_saved = true;
        }
        private bool IsKeyPressed(Keys key)
        {
            return _currentKeyboardState.IsKeyDown(key) && !_previousKeyboardState.IsKeyDown(key);
        }
    }
}
