using Microsoft.Xna.Framework;

namespace FPSViewer
{
	public class ModConfig
	{
        public bool Enable { get; set; } = true;
        public Vector2 TextPos  = new(10);
        public Color TextColor  = Color.White;
        public float TextSize { get; set; } = 1.0f;
		public bool DrawShadow { get; set; } = true;
	}
}
