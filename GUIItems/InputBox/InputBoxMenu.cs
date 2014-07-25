using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MineLib.GraphicClient.GUIItems.InputBox
{
    public enum InputBoxMenuPosition
    {
        Top,
        Top2,
        Top3,
        Top4,

        Center,

        Bottom4,
        Bottom3,
        Bottom2,
        Bottom,
    }

    public class InputBoxMenu : GUIInputBox
    {
        //Vector2 InputBoxSize = new Vector2(400, 44);
        Vector2 InputBoxSize = new Vector2(404, 44); // Vanilla settings
        Vector2 FrameSize = new Vector2(2);
     
        public InputBoxMenu(GameClient gameClient, InputBoxMenuPosition pos)
        {
            GameClient = gameClient;
            Position = pos;
        }

        private Vector2 GetPosition(InputBoxMenuPosition pos)
        {
            float topX = ScreenRectangle.Center.X;

            switch (pos)
            {
                case InputBoxMenuPosition.Top:
                    return new Vector2(topX,        ScreenRectangle.Center.Y * 0.0f);

                case InputBoxMenuPosition.Top2:
                    return new Vector2(topX,        ScreenRectangle.Center.Y * 0.25f);

                case InputBoxMenuPosition.Top3:
                    return new Vector2(topX,        ScreenRectangle.Center.Y * 0.5f);

                case InputBoxMenuPosition.Top4:
                    return new Vector2(topX,        ScreenRectangle.Center.Y * 0.75f);

                case InputBoxMenuPosition.Center:
                    return new Vector2(topX,        ScreenRectangle.Center.Y  - InputBoxSize.Y * 0.5f);

                case InputBoxMenuPosition.Bottom4:
                    return new Vector2(topX,        ScreenRectangle.Center.Y * 1f);

                case InputBoxMenuPosition.Bottom3:
                    return new Vector2(topX,        ScreenRectangle.Center.Y * 1.25f);

                case InputBoxMenuPosition.Bottom2:
                    return new Vector2(topX,        ScreenRectangle.Center.Y * 1.5f);

                case InputBoxMenuPosition.Bottom:
                    return new Vector2(topX,        ScreenRectangle.Center.Y * 1.75f);

                default:
                    return new Vector2(0);
            }    
        }

        public override void LoadContent()
        {
            Vector2 inputBoxPosition = GetPosition(Position);

            InputBoxRectangle = new Rectangle((int)(inputBoxPosition.X - InputBoxSize.X * 0.5f), (int)(inputBoxPosition.Y), (int)InputBoxSize.X, (int)InputBoxSize.Y);

            WhiteFrameTopRectangle = new Rectangle((int)(inputBoxPosition.X - InputBoxSize.X * 0.5f), (int)(inputBoxPosition.Y), (int)InputBoxSize.X, (int)FrameSize.Y);

            WhiteFrameBottomRectangle = new Rectangle((int)(inputBoxPosition.X - InputBoxSize.X * 0.5f), (int)(inputBoxPosition.Y + InputBoxSize.Y - FrameSize.Y), (int)InputBoxSize.X, (int)FrameSize.Y);

            WhiteFrameLeftRectangle = new Rectangle((int)(inputBoxPosition.X - InputBoxSize.X * 0.5f), (int)(inputBoxPosition.Y), (int)FrameSize.X, (int)InputBoxSize.Y);

            WhiteFrameRightRectangle = new Rectangle((int)(inputBoxPosition.X + InputBoxSize.X * 0.5f - FrameSize.X), (int)(inputBoxPosition.Y), (int)FrameSize.X, (int)InputBoxSize.Y);

            BlackTexture = new Texture2D(GameClient.GraphicsDevice, 1, 1);
            BlackTexture.SetData(new[] { new Color(0, 0, 0, 255) });

            WhiteFrameTexture = new Texture2D(GameClient.GraphicsDevice, 1, 1);
            WhiteFrameTexture.SetData(new[] { Color.LightGray });

            TextVector = new Vector2(InputBoxRectangle.X + 5, InputBoxRectangle.Y);
            TextShadowVector = new Vector2(TextVector.X + 1, TextVector.Y + 1);

            Vector2 size = ButtonFont.MeasureString(InputBoxText + " ");

            float xScale = (InputBoxRectangle.Width / size.X);
            float yScale = (InputBoxRectangle.Height / size.Y);

            // Taking the smaller scaling value will result in the text always fitting in the boundaries.
            TextScale = Math.Min(xScale, yScale);
        }

        public override void Update(GameTime gameTime)
        {
            if (IsSelected)
            {
                if (CycleCount > CycleNumb)
                {
                    ShowInput = !ShowInput;
                    CycleCount = 0;
                }
                CycleCount++;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointWrap, null, null);

            SpriteBatch.Draw(BlackTexture, InputBoxRectangle, Rectangle.Empty, Color.White);

            SpriteBatch.Draw(WhiteFrameTexture, WhiteFrameTopRectangle, new Rectangle(0, 0, (int) InputBoxSize.X, (int) FrameSize.Y), Color.White);
            SpriteBatch.Draw(WhiteFrameTexture, WhiteFrameBottomRectangle, new Rectangle(0, 0, (int) InputBoxSize.X, (int) FrameSize.Y), Color.White);
            SpriteBatch.Draw(WhiteFrameTexture, WhiteFrameLeftRectangle, new Rectangle(0, 0, (int) InputBoxSize.Y, (int) FrameSize.X), Color.White);
            SpriteBatch.Draw(WhiteFrameTexture, WhiteFrameRightRectangle, new Rectangle(0, 0, (int) InputBoxSize.Y, (int) FrameSize.X), Color.White);

            SpriteBatch.DrawString(ButtonFont, InputBoxText, TextShadowVector, TextShadowColor, 0.0f, Vector2.Zero, TextScale, SpriteEffects.None, 0.0f);
            SpriteBatch.DrawString(ButtonFont, InputBoxText, TextVector, TextColor, 0.0f, Vector2.Zero, TextScale, SpriteEffects.None, 0.0f);

            if (IsSelected && ShowInput)
            {
                SpriteBatch.DrawString(ButtonFont, InputBoxText + "_", TextShadowVector, TextShadowColor, 0.0f, Vector2.Zero, TextScale, SpriteEffects.None, 0.0f);
                SpriteBatch.DrawString(ButtonFont, InputBoxText + "_", TextVector, TextColor, 0.0f, Vector2.Zero, TextScale, SpriteEffects.None, 0.0f);
            }

            SpriteBatch.End();
        }
    }
}
