using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MineLib.GraphicClient.GUIItems.Button
{
    public enum ButtonNavigationHalfPosition
    {
        LeftTopLeft,    LeftTopRight,       LeftTop,    RightTop,       RightTopLeft,       RightTopRight,
        LeftBottomLeft, LeftBottomRight,    LeftBottom, RightBottom,    RightBottomLeft,    RightBottomRight,
    }

    // We need to split texture render, resize didn't properly work.
    sealed class ButtonNavigationHalf : GUIButton
    {
        Vector2 ButtonSize = new Vector2(170, 35);
        //Vector2 ButtonSize = new Vector2(196, 40); // Vanilla settings

        public ButtonNavigationHalf(GameClient gameClient, string text, ButtonNavigationHalfPosition pos)
        {
            GameClient = gameClient;
            ButtonText = text;

            Vector2 buttonPosition = GetPosition(pos);

            ButtonRectangle = new Rectangle((int)(buttonPosition.X - ButtonSize.X * 0.5f), (int)(buttonPosition.Y), (int)ButtonSize.X, (int)ButtonSize.Y);

            ButtonRectangleFirstHalf = ButtonRectangle;
            ButtonRectangleFirstHalf.Width -= (int)(ButtonRectangleFirstHalf.Width * 0.5f);

            ButtonRectangleSecondHalf = ButtonRectangle;
            ButtonRectangleSecondHalf.X += (int)(ButtonRectangleSecondHalf.Width * 0.5f);
            ButtonRectangleSecondHalf.Width -= (int)(ButtonRectangleSecondHalf.Width * 0.5f);
        }

        public ButtonNavigationHalf(GameClient gameClient, string text, ButtonNavigationHalfPosition pos, Action action)
        {
            GameClient = gameClient;
            ButtonText = text;

            OnButtonPressed += action;

            Vector2 buttonPosition = GetPosition(pos);

            ButtonRectangle = new Rectangle((int)(buttonPosition.X - ButtonSize.X * 0.5f), (int)(buttonPosition.Y), (int)ButtonSize.X, (int)ButtonSize.Y);

            ButtonRectangleFirstHalf = ButtonRectangle;
            ButtonRectangleFirstHalf.Width -= (int)(ButtonRectangleFirstHalf.Width * 0.5f);

            ButtonRectangleSecondHalf = ButtonRectangle;
            ButtonRectangleSecondHalf.X += (int)(ButtonRectangleSecondHalf.Width * 0.5f);
            ButtonRectangleSecondHalf.Width -= (int)(ButtonRectangleSecondHalf.Width * 0.5f);
        }

        Vector2 GetPosition(ButtonNavigationHalfPosition pos)
        {
            float top           = (ScreenRectangle.Height - 192) + 192 * 0.25f - ButtonSize.Y * 0.5f;
            float bottom        = (ScreenRectangle.Height - 192) + 192 * 0.75f - ButtonSize.Y * 0.5f;

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

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointWrap, null, null);

            if (IsNonPressable)
            {
                SpriteBatch.Draw(WidgetsTexture, ButtonRectangleFirstHalf, ButtonUnavailableFirstHalfPosition, Color.White);
                SpriteBatch.Draw(WidgetsTexture, ButtonRectangleSecondHalf, ButtonUnavailableSecondHalfPosition, Color.White);
                DrawString(SpriteBatch, MainFont, ButtonUnavailableColor, ButtonText, ButtonRectangle);
            }

            if (IsSelected || IsSelectedMouseHover)
            {
                SpriteBatch.Draw(WidgetsTexture, ButtonRectangleFirstHalf, ButtonPressedFirstHalfPosition, Color.White);
                SpriteBatch.Draw(WidgetsTexture, ButtonRectangleSecondHalf, ButtonPressedSecondHalfPosition, Color.White);
                DrawString(SpriteBatch, MainFont, ButtonPressedShadowColor, ButtonText, ButtonRectangleShadow);
                DrawString(SpriteBatch, MainFont, ButtonPressedColor, ButtonText, ButtonRectangle);
            }

            if (IsActive)
            {
                SpriteBatch.Draw(WidgetsTexture, ButtonRectangleFirstHalf, ButtonFirstHalfPosition, Color.White);
                SpriteBatch.Draw(WidgetsTexture, ButtonRectangleSecondHalf, ButtonSecondHalfPosition, Color.White);
                DrawString(SpriteBatch, MainFont, ButtonShadowColor, ButtonText, ButtonRectangleShadow);
                DrawString(SpriteBatch, MainFont, ButtonColor, ButtonText, ButtonRectangle);
            }

            SpriteBatch.End();
        }
    }
}
