using Microsoft.Xna.Framework;

namespace FPSViewer
{
	public class ModConfig
	{
        private Vector2 _textPos = new(10);
        private Color _textColor = Color.White;

        public bool Enable { get; set; } = true;
        public ref Vector2 TextPos { get => ref _textPos; }
        public ref Color TextColor { get => ref _textColor; }
        public float TextSize { get; set; } = 1.0f;
		public bool DrawShadow { get; set; } = true;
	}
}
