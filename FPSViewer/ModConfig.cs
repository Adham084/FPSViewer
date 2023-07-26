using Microsoft.Xna.Framework;
using StardewValley;

namespace FPSViewer
{
    public class ModConfig
    {
        public Vector2 TextPos = new(10);
        public Color TextColor = Game1.textColor;
        public bool Enable { get; set; } = true;
        public int TextSize { get; set; } = 1;
        public bool DrawShadow { get; set; } = true;
        public bool DrawBackground { get; set; } = true;
    }
}
