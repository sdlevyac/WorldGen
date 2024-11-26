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

        private bool rule_saved = false;

        private int _width = 400;
        private int _height = 250;
        private Generator generator;
        private int generation = 0;

        private static Texture2D rect;
        private int _pixelWidth = 2;
        private string colourMode = "regions";
        private Dictionary<string, Action<int, int, int, int, SpriteBatch, Texture2D>> colourModes;

        private int[] rule = rules.conway; //rules.random_rules();
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
            // TODO: Add your initialization logic here
            grid = tools.rectangle(_width, _height); //tools.seed_grid(_width, _width, 0, 12);//randomise_grid(_width, _height);
            generator = new Generator("test");
            generator.set_neighbourhood(neighbourhoods.von_neumann);
            generator.push_rule(rule);
            //generator.push_action(generator.execute_floodfill);
            colourModes = new Dictionary<string, Action<int, int, int, int, SpriteBatch, Texture2D>>();
            colourModes["1bit"] = drawing.draw_cell_1bit;
            colourModes["gradient"] = drawing.draw_cell_gradient;
            colourModes["regions"] = drawing.draw_cell_region;

            TargetElapsedTime = TimeSpan.FromSeconds(1d / 15d);
            _graphics.IsFullScreen = false;
            _graphics.PreferredBackBufferWidth = _width * _pixelWidth;
            _graphics.PreferredBackBufferHeight = _height * _pixelWidth;
            _graphics.ApplyChanges();
            base.Initialize();
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
                if (colourMode == "1bit")
                {
                    colourMode = "gradient";
                }
                else if (colourMode == "gradient")
                {
                    colourMode = "regions";
                }
                else
                {
                    colourMode = "1bit";
                }
            }

            // TODO: Add your update logic here
            generation = (generation + 1) % 5;
            grid = generator.step(_width, _height, grid);
            //generator.push_action(generator.execute_ca); 
            //generator.push_action(generator.execute_floodfill);
            //generator.push_rule(rule);

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
