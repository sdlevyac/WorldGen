using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGen.General;

namespace WorldGen.Drawing
{
    internal static class drawing
    {
        public static void draw_cell_1bit(int i, int j, int value, int _pixelWidth, SpriteBatch _spriteBatch, Texture2D rect)
        {
            _spriteBatch.Draw(rect, new Rectangle(i * _pixelWidth, j * _pixelWidth, _pixelWidth, _pixelWidth), colours.base_colours[tools.clamp_1bit(value)]);
        }
        public static void draw_cell_gradient(int i, int j, int value, int _pixelWidth, SpriteBatch _spriteBatch, Texture2D rect)
        {
            _spriteBatch.Draw(rect, new Rectangle(i * _pixelWidth, j * _pixelWidth, _pixelWidth, _pixelWidth), new Color(0, value * 10, 0));

        }
        public static void draw_cell_region(int i, int j, int value, int _pixelWidth, SpriteBatch _spriteBatch, Texture2D rect)
        {
            if (value == -1)
            {
                _spriteBatch.Draw(rect, new Rectangle(i * _pixelWidth, j * _pixelWidth, _pixelWidth, _pixelWidth), colours.ocean_colours[0]);
            }
            else if (value < -1)
            {
                value = tools.mod(value, colours.lake_colours.Length - 1);
                _spriteBatch.Draw(rect, new Rectangle(i * _pixelWidth, j * _pixelWidth, _pixelWidth, _pixelWidth), colours.lake_colours[value]);
            }
            else
            {
                value = value == 0 ? colours.region_colours.Length - 1 : tools.mod(value, colours.region_colours.Length - 1);
                _spriteBatch.Draw(rect, new Rectangle(i * _pixelWidth, j * _pixelWidth, _pixelWidth, _pixelWidth), colours.region_colours[value]);
            }
        }
    }
}
