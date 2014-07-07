using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MineLib.GraphicClient.GUIButtons
{
    public enum ButtonEnum
    {
        LeftTop,        Top,        RightTop,
        LeftTop2,       Top2,       RightTop2,
        LeftTop3,       Top3,       RightTop3,
        LeftTop4,       Top4,       RightTop4,

        LeftBottom4,    Bottom4,    RightBottom4,
        LeftBottom3,    Bottom3,    RightBottom3,
        LeftBottom2,    Bottom2,    RightBottom2,
        LeftBottom,     Bottom,     RightBottom,
    }

    // Some shit is going on here
    // TODO: Rewrite dat stuff
    // Don't blame me, I'm in hell already.
    class Button : GUIButton
    {
        public override event Action OnButtonPressed;

        public Button(Texture2D widgetsTexture, SpriteFont font, string text, Rectangle rectangle,
            ButtonEnum level)
        {
            ButtonTexture = widgetsTexture;

            ButtonFont = font;
            ButtonText = text;

            Vector2 buttonSize = new Vector2(rectangle.Width * 0.33f, (rectangle.Width * 0.33f * ButtonHeight) / ButtonWidth);
            Vector2 buttonPosition = GetPosition(level, rectangle);

            ButtonRectangle = new Rectangle((int)(buttonPosition.X - buttonPosition.X * 0.33f),
                (int) (buttonPosition.Y), (int) buttonSize.X, (int) buttonSize.Y);
        }

        public override void HandleInput(InputState input)
        {
            #region Mouse handling

            MouseState mouse = input.CurrentMouseState;
            Rectangle mouseRectangle = new Rectangle(mouse.X, mouse.Y, 1, 1);

            if (ButtonRectangle.Intersects(mouseRectangle))
            {
                IsDown = true;

                if (mouse.LeftButton == ButtonState.Pressed)
                {
                    IsClicked = true;

                    if (!EventCalled)
                    {
                        EventCalled = true;

                        if (OnButtonPressed != null)
                            OnButtonPressed();
                    }
                    else
                        EventCalled = false;
                }
                else
                    IsClicked = false;
            }
            else
                IsDown = false;
            #endregion
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            #region Preventing too soon clicks on new screen on the same position that the old button was
            if (JustCreated)
            {
                CreationTime = gameTime.TotalGameTime;
                JustCreated = false;
            }

            if (!NoMoreChecks && gameTime.TotalGameTime - CreationTime < CoolDown)
                return;
            else
                NoMoreChecks = true;
            
            #endregion
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            spriteBatch.Draw(ButtonTexture, ButtonRectangle, IsDown ? ButtonPressedPosition : ButtonPosition, Color.White);

            DrawString(spriteBatch, ButtonFont, IsDown ? Color.Yellow : Color.White, ButtonText, ButtonRectangle);
        }
    }
}
