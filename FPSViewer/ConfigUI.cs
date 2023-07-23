using System;
using GenericModConfigMenu;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Menus;

namespace FPSViewer
{
	public class ConfigUI
	{
        private ModConfig _config;
        private IManifest _modManifest;
        private IGenericModConfigMenuApi _configMenu;

        private Color _currentColor;

        public void Init(ModConfig config, IManifest modManifest, IModHelper helper)
		{
            _config = config;
            _modManifest = modManifest;
            _currentColor = config.TextColor;

            _configMenu = helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
            
            if (_configMenu is null)
                return;

            _configMenu.Register(
                modManifest,
                () => config = new ModConfig(),
                () => helper.WriteConfig(config)
            );

            BuildConfigUI(_configMenu);

            _configMenu.OnFieldChanged(modManifest, OnFieldChanged);
        }

		private void OnFieldChanged(string fieldId, object value)
		{
			switch (fieldId)
			{
                case "R":
                    _currentColor.R = Convert.ToByte(value);
                    break;

                case "G":
                    _currentColor.G = Convert.ToByte(value);
                    break;

                case "B":
                    _currentColor.B = Convert.ToByte(value);
                    break;
            }
		}

		private void BuildConfigUI(IGenericModConfigMenuApi configMenu)
        {
            configMenu.AddSectionTitle(_modManifest, () => "General");

            configMenu.AddBoolOption(_modManifest,
               () => _config.Enable,
               (value) => _config.Enable = value,
               () => "Enable",
               () => "Enable FPS counter view."
            );

            configMenu.AddSectionTitle(_modManifest, () => "Position");

            configMenu.AddNumberOption(
                _modManifest,
                () => (int)_config.TextPos.X,
                (value) => _config.TextPos.X = value,
                () => "X Position",
                () => "How far the text from left edge.",
                0,
                90,
                1
            );

            configMenu.AddNumberOption(
                _modManifest,
                () => (int)_config.TextPos.Y,
                (value) => _config.TextPos.Y = value,
                () => "Y Position",
                () => "How far the text from top edge.",
                0,
                90,
                1
            );

            configMenu.AddNumberOption(
                _modManifest,
                () => _config.TextSize * 5,
                (value) => _config.TextSize = value / 5,
                () => "Text Size",
                () => "Text Size.",
                1,
                10,
                1
            );

            configMenu.AddSectionTitle(_modManifest, () => "Appearance");

            configMenu.AddBoolOption(_modManifest,
                () => _config.DrawShadow,
                (value) => _config.DrawShadow = value,
                () => "Draw Shadow",
                () => "Draw shadow for the text."
            );

            configMenu.AddComplexOption(
                _modManifest,
                () => "Text Color Preview",
                DrawColorPreview,
                () => "Text display color."
            );

            configMenu.AddNumberOption(
                _modManifest,
                () => _config.TextColor.R,
                (value) => _config.TextColor.R = Convert.ToByte(value),
                () => "Red",
                () => "Red component of the color.",
                0,
                255,
                1,
                null,
                "R"
            );

            configMenu.AddNumberOption(
                _modManifest,
                () => _config.TextColor.G,
                (value) => _config.TextColor.G = Convert.ToByte(value),
                () => "Green",
                () => "Green component of the color.",
                0,
                255,
                1,
                null,
                "G"
            );

            configMenu.AddNumberOption(
                _modManifest,
                () => _config.TextColor.B,
                (value) => _config.TextColor.B = Convert.ToByte(value),
                () => "Blue",
                () => "Blue component of the color.",
                0,
                255,
                1,
                null,
                "B"
            );
        }

        private void DrawColorPreview(SpriteBatch spriteBatch, Vector2 pos)
        {
			IClickableMenu.drawTextureBox(spriteBatch, (int)pos.X, (int)pos.Y, 150, 50, Color.White);
			spriteBatch.Draw(Game1.staminaRect, new Rectangle((int)pos.X + 12, (int)pos.Y + 12, 126, 26), _currentColor);
		}
    }
}
