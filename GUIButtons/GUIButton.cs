using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MineLib.GraphicClient.GUIButtons
{
    public abstract class GUIButton
    {
        public bool IsClicked;
        public bool IsDown;

        protected bool JustCreated = true;
        protected bool NoMoreChecks;
        protected bool EventCalled;

        protected TimeSpan CreationTime;
        protected TimeSpan CoolDown = new TimeSpan(0, 0, 0, 0, 200);

        protected Texture2D ButtonTexture;
        protected Rectangle ButtonRectangle;
        protected SpriteFont ButtonFont;
        protected string ButtonText;

        protected Rectangle ButtonPosition = new Rectangle(0, 66, 200, 20);
        protected Rectangle ButtonPressedPosition = new Rectangle(0, 86, 200, 20);

        protected const int ButtonWidth = 200;
        protected const int ButtonHeight = 20;

        public virtual void Update(GameTime gameTime) { }

        public virtual void Draw(SpriteBatch spriteBatch) { }

        // Some function from internet, forgot url.
        protected static void DrawString(SpriteBatch spriteBatch, SpriteFont font, Color color, string strToDraw, Rectangle boundaries)
        {
            Vector2 size = font.MeasureString(strToDraw);

            float xScale = (boundaries.Width / size.X);
            float yScale = (boundaries.Height / size.Y);

            // Taking the smaller scaling value will result in the text always fitting in the boundaries.
            float scale = Math.Min(xScale, yScale);

            // Figure out the location to absolutely-center it in the boundaries rectangle.
            int strWidth = (int)Math.Round(size.X * scale);
            int strHeight = (int)Math.Round(size.Y * scale);
            Vector2 position = new Vector2();
            position.X = (((boundaries.Width - strWidth) / 2) + boundaries.X);
            position.Y = (((boundaries.Height - strHeight) / 2) + boundaries.Y);

            // A bunch of settings where we just want to use reasonable defaults.
            float rotation = 0.0f;
            Vector2 spriteOrigin = new Vector2(0, 0);
            float spriteLayer = 0.0f; // all the way in the front
            SpriteEffects spriteEffects = new SpriteEffects();

            // Draw the string to the sprite batch!
            spriteBatch.DrawString(font, strToDraw, position, color, rotation, spriteOrigin, scale, spriteEffects, spriteLayer);
        }

        protected static Vector2 GetPosition(ButtonEnum level, Rectangle rectangle)
        {
            switch (level)
            {
                case ButtonEnum.LeftTop:
                    return new Vector2(0,                   rectangle.Center.Y * 0.0f);
                case ButtonEnum.Top:
                    return new Vector2(rectangle.Center.X,  rectangle.Center.Y * 0.0f);
                case ButtonEnum.RightTop:
                    return new Vector2(rectangle.Width,     rectangle.Center.Y * 0.0f);

                case ButtonEnum.LeftTop2:
                    return new Vector2(0,                   rectangle.Center.Y * 0.25f);
                case ButtonEnum.Top2:
                    return new Vector2(rectangle.Center.X,  rectangle.Center.Y * 0.25f);
                case ButtonEnum.RightTop2:
                    return new Vector2(rectangle.Width,     rectangle.Center.Y * 0.25f);

                case ButtonEnum.LeftTop3:
                    return new Vector2(0,                   rectangle.Center.Y * 0.5f);
                case ButtonEnum.Top3:
                    return new Vector2(rectangle.Center.X,  rectangle.Center.Y * 0.5f);
                case ButtonEnum.RightTop3:
                    return new Vector2(rectangle.Width,     rectangle.Center.Y * 0.5f);

                case ButtonEnum.LeftTop4:
                    return new Vector2(0,                   rectangle.Center.Y * 0.75f);
                case ButtonEnum.Top4:
                    return new Vector2(rectangle.Center.X,  rectangle.Center.Y * 0.75f);
                case ButtonEnum.RightTop4:
                    return new Vector2(rectangle.Width,     rectangle.Center.Y * 0.75f);

                case ButtonEnum.LeftBottom4:
                    return new Vector2(0,                   rectangle.Center.Y * 1f);
                case ButtonEnum.Bottom4:
                    return new Vector2(rectangle.Center.X,  rectangle.Center.Y * 1f);
                case ButtonEnum.RightBottom4:
                    return new Vector2(rectangle.Width,     rectangle.Center.Y * 1f);


                case ButtonEnum.LeftBottom3:
                    return new Vector2(0,                   rectangle.Center.Y * 1.25f);
                case ButtonEnum.Bottom3:
                    return new Vector2(rectangle.Center.X,  rectangle.Center.Y * 1.25f);
                case ButtonEnum.RightBottom3:
                    return new Vector2(rectangle.Width,     rectangle.Center.Y * 1.25f);

                case ButtonEnum.LeftBottom2:
                    return new Vector2(0,                   rectangle.Center.Y * 1.5f);
                case ButtonEnum.Bottom2:
                    return new Vector2(rectangle.Center.X,  rectangle.Center.Y * 1.5f);
                case ButtonEnum.RightBottom2:
                    return new Vector2(rectangle.Width,     rectangle.Center.Y * 1.5f);

                case ButtonEnum.LeftBottom:
                    return new Vector2(0,                   rectangle.Center.Y * 1.75f);
                case ButtonEnum.Bottom:
                    return new Vector2(rectangle.Center.X,  rectangle.Center.Y * 1.75f);
                case ButtonEnum.RightBottom:
                    return new Vector2(rectangle.Width,     rectangle.Center.Y * 1.75f);
            }
            return new Vector2(0);
        }

        public virtual event Action OnButtonPressed;
    }
}
