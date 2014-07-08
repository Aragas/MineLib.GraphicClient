using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MineLib.GraphicClient
{
    /// <summary>
    /// Helper for reading input from keyboard, gamepad and mouse. This public class tracks
    /// the current and previous state of both input devices, and implements query
    /// properties for high level input actions such as "move up through the menu"
    /// or "pause the game".
    /// </summary>
    /// <remarks>
    /// This public class is similar to one in the GameStateManagement sample.
    /// </remarks>
    public class InputState
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
            return (CurrentKeyboardState.IsKeyDown(key) &&
                    LastKeyboardState.IsKeyUp(key));
        }

        #endregion
    }
}