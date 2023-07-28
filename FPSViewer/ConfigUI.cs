using System;
using GenericModConfigMenu;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Menus;

namespace FPSViewer
{
    public static class ConfigUI
    {
        private static ModConfig _config;
        private static IManifest _modManifest;
        private static IGenericModConfigMenuApi _configMenu;

        private static Color _currentColor;

        public static void Initialize(FPSViewerMod mod)
        {
            _config = mod._config;
            _modManifest = mod.ModManifest;
            _currentColor = _config.TextColor;

            _configMenu = mod.Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");

            if (_configMenu is null)
                return;

            _configMenu.Register(
                mod.ModManifest,
                () =>
                {
                    _config = new ModConfig();
                    mod._config = _config;
                    _currentColor = _config.TextColor;
                    mod.Helper.WriteConfig(_config);
                },
                () => mod.Helper.WriteConfig(_config)
            );

            BuildConfigUI(_configMenu);

            _configMenu.OnFieldChanged(_modManifest, OnFieldChanged);
        }

        private static void OnFieldChanged(string fieldId, object value)
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

        private static void BuildConfigUI(IGenericModConfigMenuApi configMenu)
        {
            configMenu.AddSectionTitle(_modManifest, () => "General");

            configMenu.AddBoolOption(
                _modManifest,
               () => _config.Enable,
               value => _config.Enable = value,
               () => "Enable",
               () => "Enable the FPS counter view."
            );

            configMenu.AddKeybind(
                _modManifest,
                () => _config.EnableKey,
                value => _config.EnableKey = value,
                () => "Enable Key",
                () => "Keybind to enable and disable the FPS counter view."
            );

            configMenu.AddSectionTitle(_modManifest, () => "Position");

            configMenu.AddNumberOption(
                _modManifest,
                () => _config.TextPos.X,
                value => _config.TextPos.X = value,
                () => "X Position %",
                () => "How far the text from left edge as a percentage of screen width.",
                0f,
                1f,
                0.001f,
                value => (value * 100f) + "%"
            );

            configMenu.AddNumberOption(
                _modManifest,
                () => _config.TextPos.Y,
                value => _config.TextPos.Y = value,
                () => "Y Position %",
                () => "How far the text from top edge as a percentage of screen height.",
                0f,
                1f,
                0.001f,
                value => (value * 100f) + "%"
            );

            configMenu.AddSectionTitle(_modManifest, () => "Appearance");

            configMenu.AddNumberOption(
                _modManifest,
                () => _config.TextSize,
                value => _config.TextSize = value > 0 ? value : 1f / (2 - value),
                () => "Text Size",
                () => "Text Scale factor.",
                -2,
                4,
                1,
                value => "x" + (value > 0 ? value : 1f / (2 - value))
            );

            configMenu.AddBoolOption(
                _modManifest,
                () => _config.DrawBackground,
                value => _config.DrawBackground = value,
                () => "Draw Background",
                () => "Draw background for the text."
            );

            configMenu.AddBoolOption(
                _modManifest,
                () => _config.DrawShadow,
                value => _config.DrawShadow = value,
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
                value => _config.TextColor.R = Convert.ToByte(value),
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
                value => _config.TextColor.G = Convert.ToByte(value),
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
                value => _config.TextColor.B = Convert.ToByte(value),
                () => "Blue",
                () => "Blue component of the color.",
                0,
                255,
                1,
                null,
                "B"
            );
        }

        private static void DrawColorPreview(SpriteBatch spriteBatch, Vector2 pos)
        {
            IClickableMenu.drawTextureBox(spriteBatch, (int)pos.X, (int)pos.Y, 150, 50, Color.White);
            spriteBatch.Draw(Game1.staminaRect, new Rectangle((int)pos.X + 12, (int)pos.Y + 12, 126, 26), _currentColor);
        }
    }
}
