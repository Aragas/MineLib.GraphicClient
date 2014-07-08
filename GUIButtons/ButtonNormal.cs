using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MineLib.GraphicClient.GUIButtons
{
    public enum GUIButtonNormalPos
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

    sealed class Button : GUIButton
    {
        public override event Action OnButtonPressed;

        public Button(GameClient gameClient, string text, GUIButtonNormalPos pos)
        {
            GameClient = gameClient;
            ButtonText = text;

            Vector2 buttonSize = new Vector2(ScreenRectangle.Width * 0.33f, (ScreenRectangle.Width * 0.33f * ButtonHeight) / ButtonWidth);
            Vector2 buttonPosition = GetPosition(pos);

            ButtonRectangle = new Rectangle((int)(buttonPosition.X - buttonPosition.X * 0.33f),
                (int) (buttonPosition.Y), (int) buttonSize.X, (int) buttonSize.Y);
        }

        protected override Vector2 GetPosition(GUIButtonNormalPos pos)
        {
            switch (pos)
            {
                case GUIButtonNormalPos.LeftTop:
                    return new Vector2(0,                           ScreenRectangle.Center.Y * 0.0f);
                case GUIButtonNormalPos.Top:
                    return new Vector2(ScreenRectangle.Center.X,    ScreenRectangle.Center.Y * 0.0f);
                case GUIButtonNormalPos.RightTop:
                    return new Vector2(ScreenRectangle.Width,       ScreenRectangle.Center.Y * 0.0f);

                case GUIButtonNormalPos.LeftTop2:
                    return new Vector2(0,                           ScreenRectangle.Center.Y * 0.25f);
                case GUIButtonNormalPos.Top2:
                    return new Vector2(ScreenRectangle.Center.X,    ScreenRectangle.Center.Y * 0.25f);
                case GUIButtonNormalPos.RightTop2:
                    return new Vector2(ScreenRectangle.Width,       ScreenRectangle.Center.Y * 0.25f);

                case GUIButtonNormalPos.LeftTop3:
                    return new Vector2(0,                           ScreenRectangle.Center.Y * 0.5f);
                case GUIButtonNormalPos.Top3:
                    return new Vector2(ScreenRectangle.Center.X,    ScreenRectangle.Center.Y * 0.5f);
                case GUIButtonNormalPos.RightTop3:
                    return new Vector2(ScreenRectangle.Width,       ScreenRectangle.Center.Y * 0.5f);

                case GUIButtonNormalPos.LeftTop4:
                    return new Vector2(0,                           ScreenRectangle.Center.Y * 0.75f);
                case GUIButtonNormalPos.Top4:
                    return new Vector2(ScreenRectangle.Center.X,    ScreenRectangle.Center.Y * 0.75f);
                case GUIButtonNormalPos.RightTop4:
                    return new Vector2(ScreenRectangle.Width,       ScreenRectangle.Center.Y * 0.75f);

                case GUIButtonNormalPos.LeftBottom4:
                    return new Vector2(0,                           ScreenRectangle.Center.Y * 1f);
                case GUIButtonNormalPos.Bottom4:
                    return new Vector2(ScreenRectangle.Center.X,    ScreenRectangle.Center.Y * 1f);
                case GUIButtonNormalPos.RightBottom4:
                    return new Vector2(ScreenRectangle.Width,       ScreenRectangle.Center.Y * 1f);


                case GUIButtonNormalPos.LeftBottom3:
                    return new Vector2(0,                           ScreenRectangle.Center.Y * 1.25f);
                case GUIButtonNormalPos.Bottom3:
                    return new Vector2(ScreenRectangle.Center.X,    ScreenRectangle.Center.Y * 1.25f);
                case GUIButtonNormalPos.RightBottom3:
                    return new Vector2(ScreenRectangle.Width,       ScreenRectangle.Center.Y * 1.25f);

                case GUIButtonNormalPos.LeftBottom2:
                    return new Vector2(0,                           ScreenRectangle.Center.Y * 1.5f);
                case GUIButtonNormalPos.Bottom2:
                    return new Vector2(ScreenRectangle.Center.X,    ScreenRectangle.Center.Y * 1.5f);
                case GUIButtonNormalPos.RightBottom2:
                    return new Vector2(ScreenRectangle.Width,       ScreenRectangle.Center.Y * 1.5f);

                case GUIButtonNormalPos.LeftBottom:
                    return new Vector2(0,                           ScreenRectangle.Center.Y * 1.75f);
                case GUIButtonNormalPos.Bottom:
                    return new Vector2(ScreenRectangle.Center.X,    ScreenRectangle.Center.Y * 1.75f);
                case GUIButtonNormalPos.RightBottom:
                    return new Vector2(ScreenRectangle.Width,       ScreenRectangle.Center.Y * 1.75f);
            }
            return new Vector2(0);
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
            SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointClamp,
                DepthStencilState.None, RasterizerState.CullNone);

            SpriteBatch.Draw(WidgetsTexture, ButtonRectangle, IsSelected ? ButtonPressedPosition : ButtonPosition,
                Color.White);

            DrawString(SpriteBatch, ButtonFont, IsSelected ? Color.Yellow : Color.White, ButtonText, ButtonRectangle);

            SpriteBatch.End();
        }
    }
}
