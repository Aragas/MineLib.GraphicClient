using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MineLib.GraphicClient.GUIItems.Buttons
{
    public enum ButtonNavigationHalfPosition
    {
        LeftTopLeft,    LeftTopRight,       LeftTop,    RightTop,       RightTopLeft,       RightTopRight,
        LeftBottomLeft, LeftBottomRight,    LeftBottom, RightBottom,    RightBottomLeft,    RightBottomRight,
    }

    sealed class ButtonNavigationHalf : GUIButton
    {
        public override event Action OnButtonPressed;

        Vector2 ButtonSize = new Vector2(170, 35);
        //Vector2 ButtonSize = new Vector2(196, 40); // Vanilla settings

        Rectangle ButtonFirstHalfPosition = new Rectangle(0, 66, 49, 20);
        Rectangle ButtonSecondHalfPosition = new Rectangle(151, 66, 49, 20);

        Rectangle ButtonPressedFirstHalfPosition = new Rectangle(0, 86, 49, 20);
        Rectangle ButtonPressedSecondHalfPosition = new Rectangle(151, 86, 49, 20);

        Rectangle ButtonRectangleFirstHalf;
        Rectangle ButtonRectangleSecondHalf;

        public ButtonNavigationHalf(GameClient gameClient, string text, ButtonNavigationHalfPosition pos)
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

        private Vector2 GetPosition(ButtonNavigationHalfPosition pos)
        {
            float top = (ScreenRectangle.Height - 192) + 192 * 0.25f - ButtonSize.Y * 0.5f;
            float bottom = (ScreenRectangle.Height - 192) + 192 * 0.75f - ButtonSize.Y * 0.5f;

            float leftTopLeft   = ScreenRectangle.Width * 0.25f - 5 - ButtonSize.X;
            float leftTopRight  = ScreenRectangle.Width * 0.25f + 5;

            float leftTopX      = ScreenRectangle.Center.X - 5 - ButtonSize.X * 0.5f;
            float rightTopX     = ScreenRectangle.Center.X + 5 + ButtonSize.X * 0.5f;

            float rightTopLeft  = ScreenRectangle.Width * 0.75f - 5;
            float rightTopRight = ScreenRectangle.Width * 0.75f + 5 + ButtonSize.X;

            switch (pos)
            {
                case ButtonNavigationHalfPosition.LeftTopLeft:
                    return new Vector2(leftTopLeft,     top);
                case ButtonNavigationHalfPosition.LeftTopRight:
                    return new Vector2(leftTopRight,    top);
                case ButtonNavigationHalfPosition.LeftTop:
                    return new Vector2(leftTopX,        top);
                case ButtonNavigationHalfPosition.RightTop:
                    return new Vector2(rightTopX,       top);
                case ButtonNavigationHalfPosition.RightTopLeft:
                    return new Vector2(rightTopLeft,    top);
                case ButtonNavigationHalfPosition.RightTopRight:
                    return new Vector2(rightTopRight,   top);

                case ButtonNavigationHalfPosition.LeftBottomLeft:
                    return new Vector2(leftTopLeft,     bottom);
                case ButtonNavigationHalfPosition.LeftBottomRight:
                    return new Vector2(leftTopRight,    bottom);
                case ButtonNavigationHalfPosition.LeftBottom:
                    return new Vector2(leftTopX,        bottom);
                case ButtonNavigationHalfPosition.RightBottom:
                    return new Vector2(rightTopX,       bottom);
                case ButtonNavigationHalfPosition.RightBottomLeft:
                    return new Vector2(rightTopLeft,    bottom);
                case ButtonNavigationHalfPosition.RightBottomRight:
                    return new Vector2(rightTopRight,   bottom);

                default:
                    return new Vector2(0);
            }
            
        }

        public override void HandleInput(InputState input)
        {
            #region Mouse handling

            MouseState mouse = input.CurrentMouseState;

            if (ButtonRectangle.Intersects(new Rectangle(mouse.X, mouse.Y, 1, 1)) &&
                GUIItemState != GUIItemState.NonPressable)
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
