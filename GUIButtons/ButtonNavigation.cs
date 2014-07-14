using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MineLib.GraphicClient.GUIButtons
{
    public enum ButtonNavigationPosition
    {
        LeftTop,        Top,        RightTop,
        LeftBottom,     Bottom,     RightBottom,
    }

    sealed class ButtonNavigation : GUIButton
    {
        public override event Action OnButtonPressed;

        Vector2 ButtonSize = new Vector2(250, 35);
        //Vector2 ButtonSize = new Vector2(350, 35);
        //Vector2 ButtonSize = new Vector2(400, 40); // Vanilla settings

        Rectangle ButtonFirstHalfPosition = new Rectangle(0, 66, 49, 20);
        Rectangle ButtonSecondHalfPosition = new Rectangle(151, 66, 49, 20);

        Rectangle ButtonPressedFirstHalfPosition = new Rectangle(0, 86, 49, 20);
        Rectangle ButtonPressedSecondHalfPosition = new Rectangle(151, 86, 49, 20);

        Rectangle ButtonRectangleFirstHalf;
        Rectangle ButtonRectangleSecondHalf;

        public ButtonNavigation(GameClient gameClient, string text, ButtonNavigationPosition pos)
        {
            GameClient = gameClient;
            ButtonText = text;

            Vector2 buttonPosition = GetPosition(pos);

            ButtonRectangle = new Rectangle((int)(buttonPosition.X - ButtonSize.X * 0.5f),
                (int)(buttonPosition.Y), (int)ButtonSize.X, (int)ButtonSize.Y);

            ButtonRectangleFirstHalf = ButtonRectangle;
            ButtonRectangleFirstHalf.Width -= (int)(ButtonRectangleFirstHalf.Width * 0.5f);

            ButtonRectangleSecondHalf = ButtonRectangle;
            ButtonRectangleSecondHalf.X += (int)(ButtonRectangleSecondHalf.Width * 0.5f);
            ButtonRectangleSecondHalf.Width -= (int)(ButtonRectangleSecondHalf.Width * 0.5f);
        }

        private Vector2 GetPosition(ButtonNavigationPosition pos)
        {
            float top = (ScreenRectangle.Height - 128) + 128 * 0.25f - ButtonSize.Y * 0.5f;
            float bottom = (ScreenRectangle.Height - 128) + 128 * 0.75f - ButtonSize.Y * 0.5f;

            float leftTopX      = (ScreenRectangle.Width - ButtonSize.X) * 0.25f;
            float topX          = ScreenRectangle.Center.X;
            float rightTopX     = ScreenRectangle.Width - (ScreenRectangle.Width - ButtonSize.X) * 0.25f;

            switch (pos)
            {
                case ButtonNavigationPosition.LeftTop:
                    return new Vector2(leftTopX,    top);
                case ButtonNavigationPosition.Top:
                    return new Vector2(topX,        top);
                case ButtonNavigationPosition.RightTop:
                    return new Vector2(rightTopX,   top);


                    case ButtonNavigationPosition.LeftBottom:
                    return new Vector2(leftTopX,    bottom);
                case ButtonNavigationPosition.Bottom:
                    return new Vector2(topX,        bottom);
                case ButtonNavigationPosition.RightBottom:
                    return new Vector2(rightTopX,   bottom);

                default:
                    return new Vector2(0);
            }
            
        }

        public override void HandleInput(InputState input)
        {
            #region Mouse handling

            MouseState mouse = input.CurrentMouseState;

            if (ButtonRectangle.Intersects(new Rectangle(mouse.X, mouse.Y, 1, 1)) && 
                GUIButtonState != GUIButtonState.NonPressable)
            {
                IsSelected = true;

                if (input.CurrentMouseState.LeftButton == ButtonState.Pressed &&
                    input.LastMouseState.LeftButton == ButtonState.Released)
                {
                    if (OnButtonPressed != null)
                        OnButtonPressed();
                }
            }
            else
                IsSelected = false;
            #endregion
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointWrap, null, null);

            SpriteBatch.Draw(WidgetsTexture, ButtonRectangleFirstHalf, IsSelected ? ButtonPressedFirstHalfPosition : ButtonFirstHalfPosition,
                Color.White);
            SpriteBatch.Draw(WidgetsTexture, ButtonRectangleSecondHalf, IsSelected ? ButtonPressedSecondHalfPosition : ButtonSecondHalfPosition,
                Color.White);

            DrawString(SpriteBatch, ButtonFont, IsSelected ? Color.Black : Color.Black, ButtonText, ButtonRectangleShadow);
            DrawString(SpriteBatch, ButtonFont, IsSelected ? Color.Yellow : Color.White, ButtonText, ButtonRectangle);

            SpriteBatch.End();
        }
    }
}
