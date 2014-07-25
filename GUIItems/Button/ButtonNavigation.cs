using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MineLib.GraphicClient.GUIItems.Button
{
    public enum ButtonNavigationPosition
    {
        LeftTop,        Top,        RightTop,
        LeftBottom,     Bottom,     RightBottom,
    }

    public sealed class ButtonNavigation : GUIButton
    {
        Vector2 ButtonSize = new Vector2(250, 35);
        //Vector2 ButtonSize = new Vector2(400, 40); // Vanilla settings

        public ButtonNavigation(GameClient gameClient, string text, ButtonNavigationPosition pos)
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

        Vector2 GetPosition(ButtonNavigationPosition pos)
        {
            float top       = (ScreenRectangle.Height - 128) + 128 * 0.25f - ButtonSize.Y * 0.5f;
            float bottom    = (ScreenRectangle.Height - 128) + 128 * 0.75f - ButtonSize.Y * 0.5f;

            float leftTopX  = (ScreenRectangle.Width - ButtonSize.X) * 0.25f;
            float topX      = ScreenRectangle.Center.X;
            float rightTopX = ScreenRectangle.Width - (ScreenRectangle.Width - ButtonSize.X) * 0.25f;

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
