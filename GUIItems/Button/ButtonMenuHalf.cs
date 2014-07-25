using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MineLib.GraphicClient.GUIItems.Button
{
    public enum ButtonMenuHalfPosition
    {
        LeftTop,        RightTop,
        LeftTop2,       RightTop2,
        LeftTop3,       RightTop3,
        LeftTop4,       RightTop4,

        LeftBottom4,    RightBottom4,
        LeftBottom3,    RightBottom3,
        LeftBottom2,    RightBottom2,
        LeftBottom,     RightBottom,
    }

    // We need to split texture render, resize didn't properly work.
    public sealed class ButtonMenuHalf : GUIButton
    {
        Vector2 ButtonSize = new Vector2(170, 35);
        //Vector2 ButtonSize = new Vector2(196, 40); // Vanilla settings

        public ButtonMenuHalf(GameClient gameClient, string text, ButtonMenuHalfPosition pos)
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

        Vector2 GetPosition(ButtonMenuHalfPosition pos)
        {
            float leftTopX  = ScreenRectangle.Center.X - 5 - ButtonSize.X * 0.5f;
            float rightTopX = ScreenRectangle.Center.X + 5 + ButtonSize.X * 0.5f;

            switch (pos)
            {
                case ButtonMenuHalfPosition.LeftTop:
                    return new Vector2(leftTopX,   ScreenRectangle.Center.Y * 0.0f);
                case ButtonMenuHalfPosition.RightTop:
                    return new Vector2(rightTopX,  ScreenRectangle.Center.Y * 0.0f);

                case ButtonMenuHalfPosition.LeftTop2:
                    return new Vector2(leftTopX,   ScreenRectangle.Center.Y * 0.25f);
                case ButtonMenuHalfPosition.RightTop2:
                    return new Vector2(rightTopX,  ScreenRectangle.Center.Y * 0.25f);

                case ButtonMenuHalfPosition.LeftTop3:
                    return new Vector2(leftTopX,   ScreenRectangle.Center.Y * 0.5f);
                case ButtonMenuHalfPosition.RightTop3:
                    return new Vector2(rightTopX,  ScreenRectangle.Center.Y * 0.5f);

                case ButtonMenuHalfPosition.LeftTop4:
                    return new Vector2(leftTopX,   ScreenRectangle.Center.Y * 0.75f);
                case ButtonMenuHalfPosition.RightTop4:
                    return new Vector2(rightTopX,  ScreenRectangle.Center.Y * 0.75f);

                case ButtonMenuHalfPosition.LeftBottom4:
                    return new Vector2(leftTopX,   ScreenRectangle.Center.Y * 1f);
                case ButtonMenuHalfPosition.RightBottom4:
                    return new Vector2(rightTopX,  ScreenRectangle.Center.Y * 1f);

                    case ButtonMenuHalfPosition.LeftBottom3:
                    return new Vector2(leftTopX,   ScreenRectangle.Center.Y * 1.25f);
                case ButtonMenuHalfPosition.RightBottom3:
                    return new Vector2(rightTopX,  ScreenRectangle.Center.Y * 1.25f);

                case ButtonMenuHalfPosition.LeftBottom2:
                    return new Vector2(leftTopX,   ScreenRectangle.Center.Y * 1.5f);
                case ButtonMenuHalfPosition.RightBottom2:
                    return new Vector2(rightTopX,  ScreenRectangle.Center.Y * 1.5f);

                case ButtonMenuHalfPosition.LeftBottom:
                    return new Vector2(leftTopX,   ScreenRectangle.Center.Y * 1.75f);
                case ButtonMenuHalfPosition.RightBottom:
                    return new Vector2(rightTopX,  ScreenRectangle.Center.Y * 1.75f);

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
                // Drawing without shadows
                DrawString(SpriteBatch, ButtonFont, ButtonUnavailableColor, ButtonText, ButtonRectangle);
            }

            if (IsSelected)
            {
                SpriteBatch.Draw(WidgetsTexture, ButtonRectangleFirstHalf, ButtonPressedFirstHalfPosition, Color.White);
                SpriteBatch.Draw(WidgetsTexture, ButtonRectangleSecondHalf, ButtonPressedSecondHalfPosition, Color.White); 
                DrawString(SpriteBatch, ButtonFont, ButtonPressedShadowColor, ButtonText, ButtonRectangleShadow);
                DrawString(SpriteBatch, ButtonFont, ButtonPressedColor, ButtonText, ButtonRectangle);
            }

            if (IsActive)
            {
                SpriteBatch.Draw(WidgetsTexture, ButtonRectangleFirstHalf, ButtonFirstHalfPosition, Color.White);
                SpriteBatch.Draw(WidgetsTexture, ButtonRectangleSecondHalf, ButtonSecondHalfPosition, Color.White); 
                DrawString(SpriteBatch, ButtonFont, ButtonShadowColor, ButtonText, ButtonRectangleShadow);
                DrawString(SpriteBatch, ButtonFont, ButtonColor, ButtonText, ButtonRectangle);
            }

            SpriteBatch.End();
        }
    }
}
