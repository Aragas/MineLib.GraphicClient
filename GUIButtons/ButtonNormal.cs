using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MineLib.GraphicClient.GUIButtons
{
    enum ButtonNormalPosition
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
    class ButtonNormal
    {
        public delegate void ButtonPressed();
        public event ButtonPressed OnButtonPressed;

        public bool IsClicked;
        public bool IsDown;

        bool _eventCalled;
        bool _justCreated = true;

        Texture2D _texture;
        int buttonWidth = 200, buttonHeight = 20;
        Rectangle _buttonPosition = new Rectangle(0, 66, 200, 20);
        Rectangle _buttonPressedPosition = new Rectangle(0, 86, 200, 20);

        SpriteFont _font;
        string _text;

        Rectangle _buttonRectangle;

        Stopwatch _stopwatch;

        public ButtonNormal(GUITextures guiTextures, SpriteFont font, string text, Rectangle rectangle,
            ButtonNormalPosition level)
        {
            _texture = guiTextures.Widgets;

            _font = font;
            _text = text;

            Vector2 buttonSize = new Vector2(rectangle.Width / 3, (rectangle.Width / 3 * buttonHeight) / buttonWidth);
            Vector2 buttonPosition = GetPosition(level, rectangle);

            _buttonRectangle = new Rectangle((int)(buttonPosition.X - buttonPosition.X / 3),
                (int)(buttonPosition.Y), (int)buttonSize.X, (int)buttonSize.Y);
        }

        static Vector2 GetPosition(ButtonNormalPosition level, Rectangle rectangle)
        {
            switch (level)
            {
                case ButtonNormalPosition.LeftTop:
                    return new Vector2(0,                   0);
                case ButtonNormalPosition.Top:
                    return new Vector2(rectangle.Center.X,  0);
                case ButtonNormalPosition.RightTop:
                    return new Vector2(rectangle.Width,     0);

                case ButtonNormalPosition.LeftTop2:
                    return new Vector2(0,                   rectangle.Center.Y * 0.25f);
                case ButtonNormalPosition.Top2:
                    return new Vector2(rectangle.Center.X,  rectangle.Center.Y * 0.25f);
                case ButtonNormalPosition.RightTop2:
                    return new Vector2(rectangle.Width,     rectangle.Center.Y * 0.25f);

                case ButtonNormalPosition.LeftTop3:
                    return new Vector2(0,                   rectangle.Center.Y * 0.5f);
                case ButtonNormalPosition.Top3:
                    return new Vector2(rectangle.Center.X,  rectangle.Center.Y * 0.5f);
                case ButtonNormalPosition.RightTop3:
                    return new Vector2(rectangle.Width,     rectangle.Center.Y * 0.5f);

                case ButtonNormalPosition.LeftTop4:
                    return new Vector2(0,                   rectangle.Center.Y * 0.75f);
                case ButtonNormalPosition.Top4:
                    return new Vector2(rectangle.Center.X,  rectangle.Center.Y * 0.75f);
                case ButtonNormalPosition.RightTop4:
                    return new Vector2(rectangle.Width,     rectangle.Center.Y * 0.75f);

                case ButtonNormalPosition.LeftBottom4:
                    return new Vector2(0,                   rectangle.Center.Y * 1f);
                case ButtonNormalPosition.Bottom4:
                    return new Vector2(rectangle.Center.X,  rectangle.Center.Y * 1f);
                case ButtonNormalPosition.RightBottom4:
                    return new Vector2(rectangle.Width,     rectangle.Center.Y * 1f);


                case ButtonNormalPosition.LeftBottom3:
                    return new Vector2(0,                   rectangle.Center.Y * 1.25f);
                case ButtonNormalPosition.Bottom3:
                    return new Vector2(rectangle.Center.X,  rectangle.Center.Y * 1.25f);
                case ButtonNormalPosition.RightBottom3:
                    return new Vector2(rectangle.Width,     rectangle.Center.Y * 1.25f);

                case ButtonNormalPosition.LeftBottom2:
                    return new Vector2(0,                   rectangle.Center.Y * 1.5f);
                case ButtonNormalPosition.Bottom2:
                    return new Vector2(rectangle.Center.X,  rectangle.Center.Y * 1.5f);
                case ButtonNormalPosition.RightBottom2:
                    return new Vector2(rectangle.Width,     rectangle.Center.Y * 1.5f);

                case ButtonNormalPosition.LeftBottom:
                    return new Vector2(0,                   rectangle.Center.Y * 1.75f);
                case ButtonNormalPosition.Bottom:
                    return new Vector2(rectangle.Center.X,  rectangle.Center.Y * 1.75f);
                case ButtonNormalPosition.RightBottom:
                    return new Vector2(rectangle.Width,     rectangle.Center.Y * 1.75f);
            }
            return new Vector2(0);
        }

        public void Update(MouseState mouse)
        {
            Rectangle mouseRectangle = new Rectangle(mouse.X, mouse.Y, 1, 1);

            #region Preventing too soon clicks on new screen on the same position that the old button was
            if (_justCreated)
            {
                _stopwatch = new Stopwatch();
                _stopwatch.Start();
                _justCreated = false;
            }

            if (_stopwatch.ElapsedMilliseconds < 200)
                return;
            else
                _stopwatch.Stop();
            #endregion

            if (mouseRectangle.Intersects(_buttonRectangle))
            {
                IsDown = true;

                if (mouse.LeftButton == ButtonState.Pressed)
                {
                    IsClicked = true;

                    if (!_eventCalled)
                    {
                        if (OnButtonPressed != null)
                            OnButtonPressed();
                        _eventCalled = true;
                    }
                }
                else
                {
                    IsClicked = false;
                    _eventCalled = false;
                }

            }
            else
            {
                IsDown = false;
                IsClicked = false;
                _eventCalled = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (_texture != null)
            {
                if (IsDown)
                    spriteBatch.Draw(_texture, _buttonRectangle, _buttonPressedPosition, Color.White);
                else
                    spriteBatch.Draw(_texture, _buttonRectangle, _buttonPosition, Color.White);
            }

            if (_font != null)
            {
                if (IsDown)
                    DrawString(spriteBatch, _font, _text, _buttonRectangle);
                else
                    DrawString(spriteBatch, _font, _text, _buttonRectangle);
            }
        }

        // Some function from internet, forgot url.
        static void DrawString(SpriteBatch spriteBatch, SpriteFont font, string strToDraw, Rectangle boundaries)
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
            spriteBatch.DrawString(font, strToDraw, position, Color.White, rotation, spriteOrigin, scale, spriteEffects, spriteLayer);
        }
    }
}
