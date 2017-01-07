using System;
using Microsoft.Xna.Framework;

namespace SoundVisualizer3D.MonoGame.Utils
{
    static class ColorUtils
    {
        private static readonly Random Random;

        static ColorUtils()
        {
            Random = new Random();
        }

        /// <summary>
        /// Brush colours only vary from given mix
        /// </summary>
        public static Color GenerateRandomColor(Color mix)
        {
            int red = Random.Next(256);
            int green = Random.Next(256);
            int blue = Random.Next(256);

            red = (red + mix.R) / 2;
            green = (green + mix.G) / 2;
            blue = (blue + mix.B) / 2;

            Color color = new Color(red, green, blue);
            return color;
        }

        /// <summary>
        /// Brush colours only vary from shades of green and blue
        /// </summary>
        public static Color GenerateRandomBrushColour()
        {
            return new Color(0, Random.Next(128, 256), Random.Next(128, 256));
        }
    }
}