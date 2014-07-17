using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MineLib.GraphicClient
{
    public class FPSCounterComponent : DrawableGameComponent
    {
        SpriteBatch spriteBatch;
        SpriteFont spriteFont;

        int frameRate = 0;
        int frameCounter = 0;
        TimeSpan elapsedTime = TimeSpan.Zero;


        public FPSCounterComponent(Game game, SpriteBatch Batch, SpriteFont Font)
            : base(game)
        {
            spriteFont = Font;
            spriteBatch = Batch;
        }


        public override void Update(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime;

            if (elapsedTime > TimeSpan.FromSeconds(1))
            {
                elapsedTime -= TimeSpan.FromSeconds(1);
                frameRate = frameCounter;
                frameCounter = 0;
            }
        }


        public override void Draw(GameTime gameTime)
        {
            frameCounter++;

            string fps = string.Format("fps  : {0}", frameRate);
            string mem = string.Format("mem : {0} (KB)", GC.GetTotalMemory(false)/1024);

            DrawString(spriteBatch, spriteFont, Color.Black, fps, new Rectangle(1, 1, Game.Window.ClientBounds.Width, 30));
            DrawString(spriteBatch, spriteFont, Color.White, fps, new Rectangle(0, 0, Game.Window.ClientBounds.Width, 30));

            DrawString(spriteBatch, spriteFont, Color.Black, mem, new Rectangle(1, 31, Game.Window.ClientBounds.Width, 30));
            DrawString(spriteBatch, spriteFont, Color.White, mem, new Rectangle(0, 30, Game.Window.ClientBounds.Width, 30));
        }

        protected static void DrawString(SpriteBatch spriteBatch, SpriteFont font, Color color, string strToDraw, Rectangle boundaries)
        {
            Vector2 size = font.MeasureString(strToDraw);

            float xScale = (boundaries.Width / size.X);
            float yScale = (boundaries.Height / size.Y);

            // Taking the smaller scaling value will result in the text always fitting in the boundaries.
            float scale = Math.Min(xScale, yScale);

            Vector2 position = new Vector2 { X = boundaries.X, Y = boundaries.Y };

            // A bunch of settings where we just want to use reasonable defaults.
            float rotation = 0.0f;
            Vector2 spriteOrigin = new Vector2(0, 0);
            float spriteLayer = 0.0f; // all the way in the front
            SpriteEffects spriteEffects = new SpriteEffects();

            // Draw the string to the sprite batch!
            spriteBatch.DrawString(font, strToDraw, position, color, rotation, spriteOrigin, scale, spriteEffects, spriteLayer);
        }
    }


}
