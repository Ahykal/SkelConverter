using System.Drawing;

namespace SkelConverter
{
    public static class Util
    {
        public static string ColorToHex(float r, float g, float b, float a)
        {
            return ColorToHex(r, g, b) + Convert.ToString((int)(a * 255), 16);
        }

        public static string ColorToHex(float r, float g, float b)
        {
            int _r = (int)(r * 255);
            int _g = (int)(g * 255);
            int _b = (int)(b * 255);
            return Convert.ToString(Color.FromArgb(_r, _g, _b).ToArgb(), 16).Substring(2);
        }
    }
}