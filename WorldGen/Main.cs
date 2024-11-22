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

        private int[] rule = rules.flood; //rules.conway; //rules.random_rules();
        private int[][] neighbourhood = neighbourhoods.moore;
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
            grid = tools.seed_grid(_width, _height, 1, 5);//tools.randomise_grid(_width, _height);
            generator = new Generator("test");
            generator.push_rule(rule);

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

            // TODO: Add your update logic here
            generation = (generation + 1) % 5;
            if (generation == 4)
            {
                rule = rules.random_rules();
            }
            grid = generator.execute_ca(_width, _height, grid);

            generator.push_action(() => generator.execute_ca(_width, _height, grid));   //testing the idea of a queue of actions instead of a queue of rules
                                                                                        //this would allow different types of actions to be executed
                                                                                        //instead of just 2-state CA rules

            generator.push_rule(rule);

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
                    _spriteBatch.Draw(rect, new Rectangle(i * _pixelWidth, j * _pixelWidth, _pixelWidth, _pixelWidth), colours.base_colours[grid[i,j]]);
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
            grid = tools.seed_grid(_width, _height, 1, 5);
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
