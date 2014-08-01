using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MineLib.GraphicClient.Misc;

namespace MineLib.GraphicClient.GUIItems.GamePad
{
    struct GamePadKeyboardEntry
    {
        public Vector2 ButtonVector;

        public char[] Chars;

        public bool Selected;
    }

     /// <summary>
    /// Daisywheel implementation from Steam Big Picture
     /// </summary>
    class GamePadDaisywheel : GUIItem
    {
        public delegate void GamePadKeyboardEventHandler(char character);
        public event GamePadKeyboardEventHandler OnCharReceived;

        public event Action OnCharDeleted;

        GamePadKeyboardEntry[] _buttonEntries;

        Texture2D _mainCircleTexture;
        Vector2 _mainCircleVector;

        Texture2D _gamePadButtonsTexture;

        int _buttonScale;
        float _scale;

         Color _color = new Color(255, 255, 255, 200);

        public GamePadDaisywheel(GameClient gameClient)
        {
            GameClient = gameClient;
        }

        GamePadKeyboardEntry[] SteamDefault()
        {
            int indent = 10;
            int buttonSize = (int)Math.Min((ScreenRectangle.Width * 0.25f), (ScreenRectangle.Height * 0.25f));

            GamePadKeyboardEntry[] buttonEntries = new GamePadKeyboardEntry[8];

            // Top
            buttonEntries[0] = new GamePadKeyboardEntry();
            buttonEntries[0].ButtonVector = new Vector2(ScreenRectangle.Center.X - buttonSize * 0.5f, ScreenRectangle.Center.Y - _mainCircleTexture.Height * 0.5f + indent);
            buttonEntries[0].Chars = new[] {'a', 'b', 'c', 'd'};
            
            // Right Top
            buttonEntries[1] = new GamePadKeyboardEntry();
            buttonEntries[1].ButtonVector = new Vector2(ScreenRectangle.Center.X + _mainCircleTexture.Height * 0.25f - buttonSize * 0.5f, ScreenRectangle.Center.Y - _mainCircleTexture.Height * 0.25f - buttonSize * 0.5f);
            buttonEntries[1].Chars = new[] { 'e', 'f', 'g', 'h' };

            // Right
            buttonEntries[2] = new GamePadKeyboardEntry();
            buttonEntries[2].ButtonVector = new Vector2(ScreenRectangle.Center.X + _mainCircleTexture.Width * 0.5f - buttonSize - indent, ScreenRectangle.Center.Y - buttonSize * 0.5f);
            buttonEntries[2].Chars = new[] { 'i', 'j', 'k', 'l' };

            // Right Bottom
            buttonEntries[3] = new GamePadKeyboardEntry();
            buttonEntries[3].ButtonVector = new Vector2(ScreenRectangle.Center.X + _mainCircleTexture.Height * 0.25f - buttonSize * 0.5f, ScreenRectangle.Center.Y + _mainCircleTexture.Height * 0.25f - buttonSize * 0.5f);
            buttonEntries[3].Chars = new[] { 'm', 'n', 'o', 'p' };
            
            // Bottom
            buttonEntries[4] = new GamePadKeyboardEntry();
            buttonEntries[4].ButtonVector = new Vector2(ScreenRectangle.Center.X - buttonSize * 0.5f, ScreenRectangle.Center.Y + _mainCircleTexture.Height * 0.5f - buttonSize - indent);
            buttonEntries[4].Chars = new[] { 'q', 'r', 's', 't' };
            
            // Left Bottom
            buttonEntries[5] = new GamePadKeyboardEntry();
            buttonEntries[5].ButtonVector = new Vector2(ScreenRectangle.Center.X - _mainCircleTexture.Height * 0.25f - buttonSize * 0.5f, ScreenRectangle.Center.Y + _mainCircleTexture.Height * 0.25f - buttonSize * 0.5f);
            buttonEntries[5].Chars = new[] { 'u', 'v', 'w', 'x' };
            
            // Left
            buttonEntries[6] = new GamePadKeyboardEntry();
            buttonEntries[6].ButtonVector = new Vector2(ScreenRectangle.Center.X - _mainCircleTexture.Width * 0.5f + indent, ScreenRectangle.Center.Y - buttonSize * 0.5f);
            buttonEntries[6].Chars = new[] { 'y', 'z', ',', '.' };
            
            // Left Top
            buttonEntries[7] = new GamePadKeyboardEntry();
            buttonEntries[7].ButtonVector = new Vector2(ScreenRectangle.Center.X - _mainCircleTexture.Height * 0.25f - buttonSize * 0.5f, ScreenRectangle.Center.Y - _mainCircleTexture.Height * 0.25f - buttonSize * 0.5f);
            buttonEntries[7].Chars = new[] { ':', '/', '@', '-' };

            return buttonEntries;
        }

        public override void LoadContent()
        {
            _mainCircleTexture = CreateCircle(GraphicsDevice, (int)(Math.Min(ScreenRectangle.Width, ScreenRectangle.Height) * 0.5f), new Color(25, 45, 60, 255));
            _mainCircleVector = new Vector2(ScreenRectangle.Center.X - _mainCircleTexture.Width * 0.5f, ScreenRectangle.Center.Y - _mainCircleTexture.Height * 0.5f);
            
            _gamePadButtonsTexture = Content.Load<Texture2D>("XboxControllerRightButtons");

            _buttonEntries = SteamDefault();

            #region Scale

            _buttonScale = (int)Math.Min((ScreenRectangle.Width * 0.25f), (ScreenRectangle.Height * 0.25f));

            Vector2 size = MainFont.MeasureString("a");

            float xScale = ((_buttonScale * 0.5f) / size.X);
            float yScale = ((_buttonScale * 0.5f) / size.Y);

            // Taking the smaller scaling value will result in the text always fitting in the boundaries.
            _scale = Math.Min(xScale, yScale);

            #endregion

        }

        public override void HandleInput(InputManager input)
        {

            #region AnalogThumbStick

            Vector2 thumbStick = input.CurrentGamePadState.ThumbSticks.Left;

            _buttonEntries[0].Selected = InputManager.GetAnalogStickDirection(thumbStick) == AnalogStickDirection.Up;

            _buttonEntries[1].Selected = InputManager.GetAnalogStickDirection(thumbStick) == AnalogStickDirection.UpRight;

            _buttonEntries[2].Selected = InputManager.GetAnalogStickDirection(thumbStick) == AnalogStickDirection.Right;

            _buttonEntries[3].Selected = InputManager.GetAnalogStickDirection(thumbStick) == AnalogStickDirection.DownRight;

            _buttonEntries[4].Selected = InputManager.GetAnalogStickDirection(thumbStick) == AnalogStickDirection.Down;

            _buttonEntries[5].Selected = InputManager.GetAnalogStickDirection(thumbStick) == AnalogStickDirection.DownLeft;

            _buttonEntries[6].Selected = InputManager.GetAnalogStickDirection(thumbStick) == AnalogStickDirection.Left;

            _buttonEntries[7].Selected = InputManager.GetAnalogStickDirection(thumbStick) == AnalogStickDirection.UpLeft;

            #endregion

            #region RightButtons

            if (input.IsOncePressed(Buttons.X) && input.CurrentGamePadState.ThumbSticks.Left != Vector2.Zero)
                foreach (GamePadKeyboardEntry buttonEntry in _buttonEntries)
                {
                    if (buttonEntry.Selected && OnCharReceived != null)
                        OnCharReceived(buttonEntry.Chars[0]);
                }

            if (input.IsOncePressed(Buttons.Y) && input.CurrentGamePadState.ThumbSticks.Left != Vector2.Zero)
                foreach (GamePadKeyboardEntry buttonEntry in _buttonEntries)
                {
                    if (buttonEntry.Selected && OnCharReceived != null)
                        OnCharReceived(buttonEntry.Chars[1]);
                }

            if (input.IsOncePressed(Buttons.B) && input.CurrentGamePadState.ThumbSticks.Left != Vector2.Zero)
                foreach (GamePadKeyboardEntry buttonEntry in _buttonEntries)
                {
                    if (buttonEntry.Selected && OnCharReceived != null)
                        OnCharReceived(buttonEntry.Chars[2]);
                }

            if (input.IsOncePressed(Buttons.A) && input.CurrentGamePadState.ThumbSticks.Left != Vector2.Zero)
                foreach (GamePadKeyboardEntry buttonEntry in _buttonEntries)
                {
                    if (buttonEntry.Selected && OnCharReceived != null)
                        OnCharReceived(buttonEntry.Chars[3]);
                }

            #endregion

            if (input.IsOncePressed(Buttons.X) && input.CurrentGamePadState.ThumbSticks.Left == Vector2.Zero)
            {
                OnCharDeleted();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.LinearClamp, null, null);

            SpriteBatch.Draw(_mainCircleTexture, _mainCircleVector, _color);

            foreach (GamePadKeyboardEntry buttonEntry in _buttonEntries)
            {
                if (buttonEntry.Selected)
                    SpriteBatch.Draw(_gamePadButtonsTexture, new Rectangle((int)buttonEntry.ButtonVector.X, (int)buttonEntry.ButtonVector.Y, _buttonScale, _buttonScale), _color);

                // Left
                Rectangle leftChar = new Rectangle((int)(buttonEntry.ButtonVector.X - 25 * _scale), (int)(buttonEntry.ButtonVector.Y + 110 * _scale), (int)(300 * _scale), (int)(300 * _scale));
                DrawString(SpriteBatch, MainFont, _color, buttonEntry.Chars[0].ToString(), leftChar);
                // Top
                Rectangle topChar = new Rectangle((int)(buttonEntry.ButtonVector.X + 135 * _scale), (int)(buttonEntry.ButtonVector.Y - 55 * _scale), (int)(300 * _scale), (int)(300 * _scale));
                DrawString(SpriteBatch, MainFont, _color, buttonEntry.Chars[1].ToString(), topChar);
                // Right
                Rectangle rightChar = new Rectangle((int)(buttonEntry.ButtonVector.X + 310 * _scale), (int)(buttonEntry.ButtonVector.Y + 110 * _scale), (int)(300 * _scale), (int)(300 * _scale));
                DrawString(SpriteBatch, MainFont, _color, buttonEntry.Chars[2].ToString(), rightChar);
                // Bottom
                Rectangle bottomChar = new Rectangle((int)(buttonEntry.ButtonVector.X + 135 * _scale), (int)(buttonEntry.ButtonVector.Y + 280 * _scale), (int)(300 * _scale), (int)(300 * _scale));
                DrawString(SpriteBatch, MainFont, _color, buttonEntry.Chars[3].ToString(), bottomChar);
            }

            SpriteBatch.End();
        }

        // http://stackoverflow.com/questions/5641579/xna-draw-a-filled-circle
        static Texture2D CreateCircle(GraphicsDevice importedGraphicsDevice, int radius, Color color)
        {
            int outerRadius = radius * 2 + 2; // So circle doesn't go out of bounds
            Texture2D texture = new Texture2D(importedGraphicsDevice, outerRadius, outerRadius);

            Color[] data = new Color[outerRadius * outerRadius];

            // Colour the entire texture transparent first.
            for (int i = 0; i < data.Length; i++)
                data[i] = Color.Transparent;

            // Work out the minimum step necessary using trigonometry + sine approximation.
            double angleStep = 1f / radius;

            for (double angle = 0; angle < Math.PI * 2; angle += angleStep)
            {
                // Use the parametric definition of a circle: http://en.wikipedia.org/wiki/Circle#Cartesian_coordinates
                int x = (int)Math.Round(radius + radius * Math.Cos(angle));
                int y = (int)Math.Round(radius + radius * Math.Sin(angle));

                data[y * outerRadius + x + 1] = color;
            }

            //width
            for (int i = 0; i < outerRadius; i++)
            {
                int yStart = -1;
                int yEnd = -1;


                //loop through height to find start and end to fill
                for (int j = 0; j < outerRadius; j++)
                {

                    if (yStart == -1)
                    {
                        if (j == outerRadius - 1)
                        {
                            //last row so there is no row below to compare to
                            break;
                        }

                        //start is indicated by Color followed by Transparent
                        if (data[i + (j * outerRadius)] == color && data[i + ((j + 1) * outerRadius)] == Color.Transparent)
                        {
                            yStart = j + 1;
                            continue;
                        }
                    }
                    else if (data[i + (j * outerRadius)] == color)
                    {
                        yEnd = j;
                        break;
                    }
                }

                //if we found a valid start and end position
                if (yStart != -1 && yEnd != -1)
                {
                    //height
                    for (int j = yStart; j < yEnd; j++)
                    {
                        data[i + (j * outerRadius)] = color;
                    }
                }
            }

            texture.SetData(data);
            return texture;
        }
    }
}
