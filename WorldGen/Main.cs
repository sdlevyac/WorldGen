using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using System.Linq;
using WorldGen.Cells;
using WorldGen.General;

namespace WorldGen
{
    public class Main : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private KeyboardState _currentKeyboardState;
        private KeyboardState _previousKeyboardState;

        private int _width = 400;
        private int _height = 250;

        private static Texture2D rect;
        private int _pixelWidth = 2;
        private static Color[] colours = new Color[]
        {
            Color.Black,
            Color.White
        };

        private int[] rule = rules.random_rules();//conway;
        private int[][] neighbourhood = neighbourhoods.moore;
        private int[,] grid;
        private int[,] buffer;

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
            grid = tools.randomise_grid(_width, _height);

            TargetElapsedTime = TimeSpan.FromSeconds(1d / 15d);
            _graphics.IsFullScreen = false;
            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 500;
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

            // TODO: Add your update logic here
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
                    _spriteBatch.Draw(rect, new Rectangle(i * _pixelWidth, j * _pixelWidth, _pixelWidth, _pixelWidth), colours[grid[i,j]]);
                }
            }
            _spriteBatch.End();
            base.Draw(gameTime);
        }
        private void randomise_grid()
        {
            grid = tools.randomise_grid(_width, _height);
        }
        private void randomise_rule()
        {
            rule = rules.random_rules();
            Debug.Write("[");
            for (int i = 0; i < rule.Length; i++)
            {
                Debug.Write($"{rule[i]},");
            }
            Debug.WriteLine("]");
        }

        private bool IsKeyPressed(Keys key)
        {
            return _currentKeyboardState.IsKeyDown(key) && !_previousKeyboardState.IsKeyDown(key);
        }
    }
}
