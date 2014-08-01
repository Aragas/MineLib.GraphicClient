using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MineLib.GraphicClient.Misc
{
    public enum AnalogStickDirection { None, Up, UpRight, Right, DownRight, Down, DownLeft, Left, UpLeft }


    /// <summary>
    /// Helper for reading input from keyboard, gamepad and mouse. This public class tracks
    /// the current and previous state of both input devices, and implements query
    /// properties for high level input actions such as "move up through the menu"
    /// or "pause the game".
    /// </summary>
    /// <remarks>
    /// This public class is similar to one in the GameStateManagement sample.
    /// </remarks>
    public class InputManager
    {
        #region Fields

        public MouseState CurrentMouseState;
        public KeyboardState CurrentKeyboardState;
        public GamePadState CurrentGamePadState;

        public MouseState LastMouseState;
        public KeyboardState LastKeyboardState;
        public GamePadState LastGamePadState;

        #endregion

        #region Properties

        public bool MouseScrollUp
        {
            get
            {
                return CurrentMouseState.ScrollWheelValue > LastMouseState.ScrollWheelValue;
            }
        }

        public bool MouseScrollDown
        {
            get
            {
                return CurrentMouseState.ScrollWheelValue < LastMouseState.ScrollWheelValue;
            }
        }

        public bool MenuUp
        {
            get
            {
                return (CurrentGamePadState.DPad.Up == ButtonState.Pressed &&
                       LastGamePadState.DPad.Up == ButtonState.Released) ||
                       (CurrentKeyboardState.IsKeyUp(Keys.Up) &&
                       LastKeyboardState.IsKeyDown(Keys.Up));
            }
        }

        public bool MenuDown
        {
            get
            {
                return (CurrentGamePadState.DPad.Down == ButtonState.Pressed &&
                       LastGamePadState.DPad.Down == ButtonState.Released) ||
                       (CurrentKeyboardState.IsKeyUp(Keys.Down) &&
                       LastKeyboardState.IsKeyDown(Keys.Down));
            }
        }

        public bool GUIMenuLeft
        {
            get
            {
                return CurrentGamePadState.Buttons.LeftShoulder == ButtonState.Pressed &&
                       LastGamePadState.Buttons.LeftShoulder == ButtonState.Released;
            }
        }

        public bool GUIMenuRight
        {
            get
            {
                return CurrentGamePadState.Buttons.RightShoulder == ButtonState.Pressed &&
                       LastGamePadState.Buttons.RightShoulder == ButtonState.Released;
            }
        }

        #endregion

        #region Methods

        public void Update()
        {
            LastMouseState = CurrentMouseState;
            LastKeyboardState = CurrentKeyboardState;
            LastGamePadState = CurrentGamePadState;

            CurrentMouseState = Mouse.GetState();
            CurrentKeyboardState = Keyboard.GetState();
            CurrentGamePadState = GamePad.GetState(PlayerIndex.One);
        }

        public bool IsOncePressed(Keys key)
        {
            return CurrentKeyboardState.IsKeyDown(key) && LastKeyboardState.IsKeyUp(key);
        }

        public bool IsOncePressed(Buttons button)
        {
            return CurrentGamePadState.IsButtonDown(button) && LastGamePadState.IsButtonUp(button);
        }


        public bool IsOncePressed(Buttons firstButton, Buttons secondButton)
        {
            bool b1 = LastGamePadState.IsButtonUp(firstButton);
            bool b2 = LastGamePadState.IsButtonUp(secondButton);
            bool b3 = CurrentGamePadState.IsButtonDown(secondButton);
            if (LastGamePadState.IsButtonUp(firstButton) && LastGamePadState.IsButtonUp(secondButton))
                return false;

            return LastGamePadState.IsButtonDown(firstButton) && (LastGamePadState.IsButtonDown(secondButton) && CurrentGamePadState.IsButtonUp(secondButton));
        }

        #endregion

        #region AnalogThumbStick stuff

        const float PI = (float)Math.PI;

        // You could make this user-configurable.
        const float Deadzone = 0.8f;
        const float AliveAngle = PI / 4 - (PI / 16);

        // Remember PI = 180 degrees
        const float AliveAngle1Start = PI * 0;
        const float AliveAngle1End = PI * 0.5f - AliveAngle;

        const float AliveAngle2Start = PI * 0.5f + AliveAngle;
        const float AliveAngle2End = PI * 1f - AliveAngle;

        const float AliveAngle3Start = PI * 1f + AliveAngle;
        const float AliveAngle3End = PI * 1.5f - AliveAngle;

        const float AliveAngle4Start = PI * 1.5f + AliveAngle;
        const float AliveAngle4End = PI * 2f - AliveAngle;

        public static AnalogStickDirection GetAnalogStickDirection(Vector2 gamepadThumbStick)
        {
            // Get the length and prevent something from happening
            // if it's in our deadzone.
            var length = gamepadThumbStick.Length();
            if (length < Deadzone)
                return AnalogStickDirection.None;

            // Find the angle that the stick is at. see: http://en.wikipedia.org/wiki/File:Atan2_60.svg
            var angle = (float)Math.Atan2(gamepadThumbStick.Y, gamepadThumbStick.X);
            if (angle < 0)
                angle += PI * 2; // Simpify our checks.

            if (angle > AliveAngle4End)
                return AnalogStickDirection.Right;

            if (angle > AliveAngle4Start)
                return AnalogStickDirection.DownRight;

            if (angle > AliveAngle3End)
                return AnalogStickDirection.Down;

            if (angle > AliveAngle3Start)
                return AnalogStickDirection.DownLeft;

            if (angle > AliveAngle2End)
                return AnalogStickDirection.Left;

            if (angle > AliveAngle2Start)
                return AnalogStickDirection.UpLeft;

            if (angle > AliveAngle1End)
                return AnalogStickDirection.Up;

            if (angle > AliveAngle1Start)
                return AnalogStickDirection.UpRight;

            return AnalogStickDirection.Right;
        }

        #endregion
    }
}