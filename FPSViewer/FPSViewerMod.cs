using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;

namespace FPSViewer
{
    public class FPSViewerMod : Mod
    {
        private ModConfig _config;
        private ConfigUI _configUI;

        private readonly TimeSpan _oneSec = TimeSpan.FromSeconds(1.0);

        private TimeSpan _elapsedTime;
        private int _frameRate;
        private int _frameCounter;

        public override void Entry(IModHelper helper)
        {
            _config = helper.ReadConfig<ModConfig>();

            helper.Events.GameLoop.GameLaunched += OnGameLaunched;
            helper.Events.GameLoop.UpdateTicked += OnUpdateTicked;
            helper.Events.Display.Rendered += OnRendered;
        }

        private void OnGameLaunched(object sender, GameLaunchedEventArgs e)
        {
            _configUI = new ConfigUI();
            _configUI.Init(_config, ModManifest, Helper);
        }

        private void OnUpdateTicked(object sender, UpdateTickedEventArgs e)
        {
            if (!_config.Enable)
                return;
                
            _elapsedTime += Game1.currentGameTime.ElapsedGameTime;

            if (_elapsedTime >= _oneSec)
            {
                _elapsedTime -= _oneSec;
                _frameRate = _frameCounter;
                _frameCounter = 0;
            }
        }

        private void OnRendered(object sender, RenderedEventArgs e)
        {
            if (!_config.Enable)
                return;
                
            _frameCounter++;
            string text = "FPS: " + _frameRate;

            Vector2 textPos = _config.TextPos;
            float textScale = Game1.options.uiScale * _config.TextSize;

            if (_config.DrawBackground)
            {
                Vector2 backgroundSize = (Game1.dialogueFont.MeasureString(text) - new Vector2(0, 12)) * textScale + new Vector2(35);

                IClickableMenu.drawTextureBox(e.SpriteBatch, (int)textPos.X, (int)textPos.Y, (int)backgroundSize.X, (int)backgroundSize.Y, Color.White);

                textPos += new Vector2(15);
            }

            if (_config.DrawShadow)
                e.SpriteBatch.DrawString(Game1.dialogueFont, text, textPos + Vector2.One, Color.Black, 0, Vector2.Zero, textScale, SpriteEffects.None, 0);

            e.SpriteBatch.DrawString(Game1.dialogueFont, text, textPos, _config.TextColor, 0, Vector2.Zero, textScale, SpriteEffects.None, 0);
        }
    }
}