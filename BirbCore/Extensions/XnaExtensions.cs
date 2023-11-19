using System.Reflection;
using BirbCore.Attributes;
using Microsoft.Xna.Framework;

namespace BirbCore.Extensions;
public static class XnaExtensions
{
    public static Color ToColor(this string str)
    {
        Color? namedColor = (Color)typeof(Color).GetProperty(str, BindingFlags.Public | BindingFlags.Static | BindingFlags.IgnoreCase)?.GetValue(null);
        if (namedColor is not null)
        {
            return (Color)namedColor;
        }

        string[] rgba = str.Split(',');
        if (rgba.Length >= 3)
        {
            if (str.Contains('.'))
            {
                float r = float.Parse(rgba[0]);
                float g = float.Parse(rgba[1]);
                float b = float.Parse(rgba[2]);
                float a = rgba.Length >= 4 ? float.Parse(rgba[3]) : 1.0f;
                return new Color(r, g, b, a);
            }
            else
            {
                int r = int.Parse(rgba[0]);
                int g = int.Parse(rgba[1]);
                int b = int.Parse(rgba[2]);
                int a = rgba.Length >= 4 ? int.Parse(rgba[3]) : 255;
                return new Color(r, g, b, a);
            }
        }

        if (uint.TryParse(str, out uint colorUint))
        {
            return new Color(colorUint);
        }

        Log.Error("Could not parse Color from string");
        return Color.White;
    }
}
