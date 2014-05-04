using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MineLib.GraphicClient.MonoGame.Screens
{
    enum ButtonPosition
    {
        LeftTop,        Top,        RightTop,
        LeftTop2,       Top2,       RightTop2,
        LeftTop3,       Top3,       RightTop3,
        LeftCenter,     Center,     RightCenter,
        LeftBottom3,    Bottom3,    RightBottom3,
        LeftBottom2,    Bottom2,   RightBottom2,
        LeftBottom,     Bottom,     RightBottom,
    }

    // Some shit is going on here
    class Button
    {
        public delegate void ButtonPressed();
        public event ButtonPressed OnButtonPressed;

        public bool IsClicked;
        public bool IsDown;

        bool _eventCalled;

        Texture2D _texture;
        Texture2D _texturePressed;
        SpriteFont _font;
        string _text;

        Color _buttonColor = new Color(255, 255, 255, 255);
        Color _fontColor = new Color(255, 255, 255, 255);
        Color _fontColorPressed = new Color(255, 255, 156, 255);


        Vector2 _imageCenter;

        Vector2 _buttonPosition;
        Vector2 _buttonSize;
        Rectangle _buttonRectangle;

        Vector2 _textSize;

        Rectangle _screen;

        public Button(Texture2D button, Texture2D buttonPressed, Rectangle rectangle, ButtonPosition level)
        {
            _texture = button;
            _texturePressed = buttonPressed;

            _imageCenter = new Vector2(_texture.Width/2f, _texture.Height/2f);
            _buttonPosition = GetPosition(level, rectangle);
        }
        public Button(Texture2D button, Texture2D buttonPressed, SpriteFont newFont, string newText, Rectangle rectangle,
            ButtonPosition level)
        {
            _texture = button;
            _texturePressed = buttonPressed;
            _font = newFont;
            _text = newText;

            _textSize = _font.MeasureString(_text);

            _screen = rectangle;

            _buttonSize = new Vector2(_screen.Width/3, (_screen.Width/3*_texture.Height)/_texture.Width);
            _imageCenter = new Vector2(_buttonSize.X/2f, _buttonSize.Y/2f);
            _buttonPosition = GetPosition(level, _screen);
        }

        static Vector2 GetPosition(ButtonPosition level, Rectangle rectangle)
        {
            switch (level)
            {
                case ButtonPosition.LeftTop:
                    return new Vector2(0,                   0);
                case ButtonPosition.Top:
                    return new Vector2(rectangle.Center.X,  0);
                case ButtonPosition.RightTop:
                    return new Vector2(rectangle.Width,     0);

                case ButtonPosition.LeftTop2:
                    return new Vector2(0,                   rectangle.Center.Y / 2);
                case ButtonPosition.Top2:
                    return new Vector2(rectangle.Center.X,  rectangle.Center.Y / 2);
                case ButtonPosition.RightTop2:
                    return new Vector2(rectangle.Width,     rectangle.Center.Y / 2);

                case ButtonPosition.LeftTop3:
                    return new Vector2(0,                   rectangle.Center.Y);
                case ButtonPosition.Top3:
                    return new Vector2(rectangle.Center.X,  rectangle.Center.Y);
                case ButtonPosition.RightTop3:
                    return new Vector2(rectangle.Width,     rectangle.Center.Y);


                case ButtonPosition.LeftCenter:
                    return new Vector2(0,                   rectangle.Center.Y);
                case ButtonPosition.Center:
                    return new Vector2(rectangle.Center.X,  rectangle.Center.Y);
                case ButtonPosition.RightCenter:
                    return new Vector2(rectangle.Width,     rectangle.Center.Y);


                case ButtonPosition.LeftBottom3:
                    return new Vector2(0,                   rectangle.Center.Y + rectangle.Center.Y / 2);
                case ButtonPosition.Bottom3:
                    return new Vector2(rectangle.Center.X,  rectangle.Center.Y + rectangle.Center.Y / 2);
                case ButtonPosition.RightBottom3:
                    return new Vector2(rectangle.Width,     rectangle.Center.Y + rectangle.Center.Y / 2);

                case ButtonPosition.LeftBottom2:
                    return new Vector2(0,                   rectangle.Center.Y + rectangle.Center.Y);
                case ButtonPosition.Bottom2:
                    return new Vector2(rectangle.Center.X,  rectangle.Center.Y + rectangle.Center.Y);
                case ButtonPosition.RightBottom2:
                    return new Vector2(rectangle.Width,     rectangle.Center.Y + rectangle.Center.Y);

                case ButtonPosition.LeftBottom:
                    return new Vector2(0,                   rectangle.Center.Y + rectangle.Center.Y + rectangle.Center.Y / 2);
                case ButtonPosition.Bottom:
                    return new Vector2(rectangle.Center.X,  rectangle.Center.Y + rectangle.Center.Y + rectangle.Center.Y / 2);
                case ButtonPosition.RightBottom:
                    return new Vector2(rectangle.Width,     rectangle.Center.Y + rectangle.Center.Y + rectangle.Center.Y / 2);
            }
            return new Vector2(0);
        }

        public void Update(MouseState mouse)
        {
            _buttonRectangle = new Rectangle((int) (_buttonPosition.X - _buttonPosition.X/3),
                (int)(_buttonPosition.Y - _buttonPosition.Y / 3), (int)_buttonSize.X, (int)_buttonSize.Y);

            Rectangle mouseRectangle = new Rectangle(mouse.X, mouse.Y, 1, 1);

            if (mouseRectangle.Intersects(_buttonRectangle))
            {
                IsDown = true;
                if (mouse.LeftButton == ButtonState.Pressed)
                {
                    IsClicked = true;

                    if (!_eventCalled)
                    {
                        OnButtonPressed();
                        _eventCalled = true;
                    }
                }
                else
                    _eventCalled = false;
                
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
            if (IsDown && _texturePressed != null)
                spriteBatch.Draw(_texturePressed, _buttonRectangle, _buttonColor);
            else
                spriteBatch.Draw(_texture, _buttonRectangle, _buttonColor);

            if (_font != null)
            {
                Vector2 textPosition = new Vector2
                {
                    X = _buttonRectangle.Center.X - _textSize.X/2,
                    Y = _buttonRectangle.Y + _imageCenter.Y/2
                };

                // 800 480 - minimal size.
                float scale = ((float) _screen.Width/(float) _screen.Height) - ((float) 800/(float) 480) + 1f;

                if (IsDown && _texturePressed != null)
                    spriteBatch.DrawString(_font, _text, textPosition, _fontColorPressed, 0f, new Vector2(0), scale,
                        SpriteEffects.None, 0f);
                else
                    spriteBatch.DrawString(_font, _text, textPosition, _fontColor, 0f, new Vector2(0), scale,
                        SpriteEffects.None, 0f);
            }
        }
    }
}
