using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;

namespace FPSViewer
{
    public class ModConfig
    {
        public Vector2 TextPos = new(0.02f, 0.15f);
        public Color TextColor = Game1.textColor;
        public bool Enable { get; set; } = true;
        public float TextSize { get; set; } = 1;
        public bool DrawShadow { get; set; } = true;
        public bool DrawBackground { get; set; } = true;
        public SButton EnableKey { get; set; } = SButton.V;
    }
}
