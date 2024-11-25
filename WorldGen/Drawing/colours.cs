using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WorldGen.Drawing
{
    internal static class colours
    {
        public static Color[] base_colours = new Color[]
        {
            Color.Black,
            Color.White,
            Color.Red,
        };

        public static Color[] region_colours = new Color[]
        {
            Color.Black,
            Color.Red,
            Color.Blue,
            Color.Green,
            Color.Yellow,
            Color.Purple,
            Color.Orange,
            Color.Pink,
        };
    }
}
