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
            Color.DarkOliveGreen,
            Color.Olive,
            Color.OliveDrab,
            Color.Black,
        };
        public static Color[] lake_colours = new Color[]
        {
            //Color.CadetBlue,
            //Color.CornflowerBlue,
            Color.Coral,
            Color.LightCoral,
            //Color.LightCyan,
            Color.LightGoldenrodYellow,
            Color.LightGreen,
            Color.LightPink,
            Color.LightSalmon,
            //Color.LightSeaGreen,
            //Color.LightSkyBlue,
            Color.LightYellow,
            Color.Crimson,
            Color.DarkSalmon,
            //Color.DarkSeaGreen,
            //Color.DarkSlateBlue,
            //Color.DarkTurquoise,
            Color.DarkViolet,
            //Color.DeepSkyBlue,
            Color.Firebrick,
            //Color.ForestGreen,
            Color.PaleGoldenrod,
            Color.PaleGreen,
            Color.PaleTurquoise,
            Color.PaleVioletRed,
            //Color.PowderBlue,
            Color.Teal,
            Color.Thistle,
            Color.Tomato,
        };
        public static Color[] ocean_colours = new Color[]
        {
            Color.LightBlue,
        };
    }
}
