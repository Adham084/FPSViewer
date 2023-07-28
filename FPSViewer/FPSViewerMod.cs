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
        internal ModConfig _config;

        private readonly TimeSpan _oneSec = TimeSpan.FromSeconds(1.0);

        private TimeSpan _elapsedTime;
        private int _frameRate;
        private int _frameCounter;

        public override void Entry(IModHelper helper)
        {
            _config = helper.ReadConfig<ModConfig>();

            helper.Events.GameLoop.GameLaunched += OnGameLaunched;
            helper.Events.GameLoop.UpdateTicked += OnUpdateTicked;
            helper.Events.Display.RenderedHud += OnRenderedHud;
            helper.Events.Display.RenderedActiveMenu += OnRenderedActiveMenu;
        }
        
        private void OnGameLaunched(object sender, GameLaunchedEventArgs e)
        {
            ConfigUI.Initialize(this);
        }

        private void OnUpdateTicked(object sender, UpdateTickedEventArgs e)
        {
            if (Helper.Input.GetState(_config.EnableKey) == SButtonState.Pressed)
            {
                _config.Enable = !_config.Enable;
                Helper.WriteConfig(_config);
            }
            
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

        private void OnRenderedActiveMenu(object sender, RenderedActiveMenuEventArgs e)
        {
            if (!_config.Enable)
                return;
                
            DrawFps(e.SpriteBatch);
        }

        private void OnRenderedHud(object sender, RenderedHudEventArgs e)
        {
            if (!_config.Enable || Game1.activeClickableMenu != null)
                return;

            DrawFps(e.SpriteBatch);
        }

        private void DrawFps(SpriteBatch spriteBatch)
        {
            _frameCounter++;
            string text = "FPS: " + _frameRate;

            Vector2 textPos = new(Game1.uiViewport.Width * Game1.options.uiScale * _config.TextPos.X,
                Game1.uiViewport.Height * Game1.options.uiScale * _config.TextPos.Y);
            
            float textScale = Game1.options.uiScale * _config.TextSize;

            if (_config.DrawBackground)
            {
                Vector2 backgroundSize = (Game1.dialogueFont.MeasureString(text) - new Vector2(0, 12)) * textScale + new Vector2(35);
                IClickableMenu.drawTextureBox(spriteBatch, Game1.menuTexture, new Rectangle(0, 256, 60, 60), (int)textPos.X, (int)textPos.Y, (int)backgroundSize.X, (int)backgroundSize.Y, Color.White, drawShadow: false);
                textPos += new Vector2(15);
            }

            if (_config.DrawShadow)
                spriteBatch.DrawString(Game1.dialogueFont, text, textPos + Vector2.One, Color.Black, 0, Vector2.Zero, textScale, SpriteEffects.None, 0);
            
            spriteBatch.DrawString(Game1.dialogueFont, text, textPos, _config.TextColor, 0, Vector2.Zero, textScale, SpriteEffects.None, 0);
        }
    }
}