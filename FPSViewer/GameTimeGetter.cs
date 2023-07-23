using System;
using Microsoft.Xna.Framework;

namespace FPSViewer
{
    public class GameTimeGetter : GameComponent
    {
        public GameTime GameTime { get; private set; }

        public GameTimeGetter(Game game) : base(game) { }

        public override void Update(GameTime gameTime)
        {
            GameTime = gameTime;
        }
    }
}
