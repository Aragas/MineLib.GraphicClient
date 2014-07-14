using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MineLib.GraphicClient.GUIItems.Buttons
{
    public enum ButtonMenuPosition
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

    sealed class ButtonMenu : GUIButton
    {
        public override event Action OnButtonPressed;

        Vector2 ButtonSize = new Vector2(350, 35);
        //Vector2 ButtonSize = new Vector2(400, 40); // Vanilla settings

        public ButtonMenu(GameClient gameClient, string text, ButtonMenuPosition pos)
        {
            GameClient = gameClient;
            ButtonText = text;

            Vector2 buttonPosition = GetPosition(pos);

            ButtonRectangle = new Rectangle((int)(buttonPosition.X - ButtonSize.X * 0.5f),
                (int)(buttonPosition.Y), (int)ButtonSize.X, (int)ButtonSize.Y);
        }

        private Vector2 GetPosition(ButtonMenuPosition pos)
        {
            float leftTopX = (ScreenRectangle.Width - ButtonSize.X) * 0.25f;
            float topX = ScreenRectangle.Center.X;
            float rightTopX = ScreenRectangle.Width - (ScreenRectangle.Width - ButtonSize.X) * 0.25f; ;

            switch (pos)
            {
                case ButtonMenuPosition.LeftTop:
                    return new Vector2(leftTopX,    ScreenRectangle.Center.Y * 0.0f);
                case ButtonMenuPosition.Top:
                    return new Vector2(topX,        ScreenRectangle.Center.Y * 0.0f);
                case ButtonMenuPosition.RightTop:
                    return new Vector2(rightTopX,   ScreenRectangle.Center.Y * 0.0f);

                case ButtonMenuPosition.LeftTop2:
                    return new Vector2(leftTopX,    ScreenRectangle.Center.Y * 0.25f);
                case ButtonMenuPosition.Top2:
                    return new Vector2(topX,        ScreenRectangle.Center.Y * 0.25f);
                case ButtonMenuPosition.RightTop2:
                    return new Vector2(rightTopX,   ScreenRectangle.Center.Y * 0.25f);

                case ButtonMenuPosition.LeftTop3:
                    return new Vector2(leftTopX,    ScreenRectangle.Center.Y * 0.5f);
                case ButtonMenuPosition.Top3:
                    return new Vector2(topX,        ScreenRectangle.Center.Y * 0.5f);
                case ButtonMenuPosition.RightTop3:
                    return new Vector2(rightTopX,   ScreenRectangle.Center.Y * 0.5f);

                case ButtonMenuPosition.LeftTop4:
                    return new Vector2(leftTopX,    ScreenRectangle.Center.Y * 0.75f);
                case ButtonMenuPosition.Top4:
                    return new Vector2(topX,        ScreenRectangle.Center.Y * 0.75f);
                case ButtonMenuPosition.RightTop4:
                    return new Vector2(rightTopX,   ScreenRectangle.Center.Y * 0.75f);

                case ButtonMenuPosition.LeftBottom4:
                    return new Vector2(leftTopX,    ScreenRectangle.Center.Y * 1f);
                case ButtonMenuPosition.Bottom4:
                    return new Vector2(topX,        ScreenRectangle.Center.Y * 1f);
                case ButtonMenuPosition.RightBottom4:
                    return new Vector2(rightTopX,   ScreenRectangle.Center.Y * 1f);


                case ButtonMenuPosition.LeftBottom3:
                    return new Vector2(leftTopX,    ScreenRectangle.Center.Y * 1.25f);
                case ButtonMenuPosition.Bottom3:
                    return new Vector2(topX,        ScreenRectangle.Center.Y * 1.25f);
                case ButtonMenuPosition.RightBottom3:
                    return new Vector2(rightTopX,   ScreenRectangle.Center.Y * 1.25f);

                case ButtonMenuPosition.LeftBottom2:
                    return new Vector2(leftTopX,    ScreenRectangle.Center.Y * 1.5f);
                case ButtonMenuPosition.Bottom2:
                    return new Vector2(topX,        ScreenRectangle.Center.Y * 1.5f);
                case ButtonMenuPosition.RightBottom2:
                    return new Vector2(rightTopX,   ScreenRectangle.Center.Y * 1.5f);

                case ButtonMenuPosition.LeftBottom:
                    return new Vector2(leftTopX,    ScreenRectangle.Center.Y * 1.75f);
                case ButtonMenuPosition.Bottom:
                    return new Vector2(topX,        ScreenRectangle.Center.Y * 1.75f);
                case ButtonMenuPosition.RightBottom:
                    return new Vector2(rightTopX,   ScreenRectangle.Center.Y * 1.75f);

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

            SpriteBatch.Draw(WidgetsTexture, ButtonRectangle, IsSelected ? ButtonPressedPosition : ButtonPosition,
                Color.White);

            DrawString(SpriteBatch, ButtonFont, IsSelected ? ButtonPressedShadowColor : ButtonShadowColor, ButtonText, ButtonRectangleShadow);
            DrawString(SpriteBatch, ButtonFont, IsSelected ? ButtonPressedColor : ButtonColor, ButtonText, ButtonRectangle);

            SpriteBatch.End();
        }
    }
}
