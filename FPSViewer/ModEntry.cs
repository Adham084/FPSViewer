using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace FPSViewer
{
    public class ModEntry : Mod
    {
        private ModConfig _config;
        private ConfigUI _configUI;

        private readonly TimeSpan _oneSec = TimeSpan.FromSeconds(1.0);
        private GameTimeGetter gameTimeGetter;

		private TimeSpan _elapsedTime;
		private int _frameRate;
		private int _frameCounter;

        public override void Entry(IModHelper helper)
        {
            _config = helper.ReadConfig<ModConfig>();

            helper.Events.GameLoop.GameLaunched += OnGameLaunched;
            helper.Events.GameLoop.UpdateTicking += OnUpdateTicking;
            helper.Events.Display.RenderingHud += OnRenderingHud;

            gameTimeGetter = new GameTimeGetter(GameRunner.instance);
            GameRunner.instance.Components.Add(gameTimeGetter);
        }

        private void OnGameLaunched(object sender, GameLaunchedEventArgs e)
        {
            _configUI = new ConfigUI();
            _configUI.Init(_config, ModManifest, Helper);
        }

        private void OnUpdateTicking(object sender, UpdateTickingEventArgs e)
        {
            if (_config.Enable)
            {
                _elapsedTime += gameTimeGetter.GameTime.ElapsedGameTime;

                if (_elapsedTime >= _oneSec)
                {
                    _elapsedTime -= _oneSec;
                    _frameRate = _frameCounter;
                    _frameCounter = 0;
                }
            }
        }

        private void OnRenderingHud(object sender, RenderingHudEventArgs e)
        {
            if (_config.Enable)
            {
                _frameCounter++;
                string text = "fps: " + _frameRate;

                if (_config.DrawShadow)
                    e.SpriteBatch.DrawString(Game1.dialogueFont, text, _config.TextPos + Vector2.One, Color.Black, 0, Vector2.Zero, _config.TextSize, SpriteEffects.None, 0);

                e.SpriteBatch.DrawString(Game1.dialogueFont, text, _config.TextPos, _config.TextColor, 0, Vector2.Zero, _config.TextSize, SpriteEffects.None, 0);
            }
        }
    }
}