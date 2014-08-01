using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MineLib.GraphicClient.Misc;

namespace MineLib.GraphicClient.GUIItems.Button
{
    /// <summary>
    /// Controlls many buttons as one entity.
    /// Adds GamePad support.
    /// </summary>
    public class GUIButtonMultiController : GUIItem
    {
        readonly List<GUIButton> _buttons = new List<GUIButton>();

        public void AddGUIButton(GUIButton button)
        {
            button.LoadContent();
            _buttons.Add(button);
        }

        public override void UnloadContent()
        {
            foreach (GUIButton guiButton in _buttons)
            {
                guiButton.UnloadContent();
            }
        }

        public override void HandleInput(InputManager input)
        {
            foreach (GUIButton guiButton in _buttons)
            {
                guiButton.HandleInput(input);
            }

            #region DPad handler

            // Don't blame me for this. Mah brain is broken.
            if (input.IsOncePressed(Buttons.DPadUp) && input.CurrentGamePadState.IsButtonUp(Buttons.LeftTrigger))
            {
                for (int i = _buttons.Count - 1; i >= 0; i--)
                {
                    // if button was found
                    if (_buttons[i].IsSelected)
                    {
                        // find the previous active button
                        for (int j = i; j >= 0; j--)
                        {
                            // if button was found, make next button active and previous button manually selected
                            if (_buttons[j].IsActive)
                            {
                                _buttons[i].ToActive();
                                _buttons[j].ToSelected();
                                return;
                            }
                        }
                    }

                    // if button wasn't found
                    else if (i == 0)
                    {
                        // find last active button and make it manually selected
                        for (int j = _buttons.Count - 1; j >= 0; j--)
                        {
                            if (_buttons[j].IsActive)
                            {
                                _buttons[j].ToSelected();
                                return;
                            }
                        }
                    }
                }
            }

            // Don't blame me for this. Mah brain is broken.
            if (input.IsOncePressed(Buttons.DPadDown) && input.CurrentGamePadState.IsButtonUp(Buttons.LeftTrigger))
            {
                // check if any button is manually selected
                for (int i = 0; i < _buttons.Count; i++)
                {
                    // if button was found
                    if (_buttons[i].IsSelected)
                    {
                        // find the next active button
                        for (int j = i; j < _buttons.Count; j++)
                        {
                            // if button was found, make previous button active and next button manually selected
                            if (_buttons[j].IsActive)
                            {
                                _buttons[i].ToActive();
                                _buttons[j].ToSelected();
                                return;
                            }
                        }
                    }

                    // if button wasn't found
                    else if (i == _buttons.Count - 1)
                    {
                        // find first active button and make it manually selected
                        for (int j = 0; j < _buttons.Count; j++)
                        {
                            if (_buttons[j].IsActive)
                            {
                                _buttons[j].ToSelected();
                                return;
                            }
                        }
                    }
                }
            }

            #endregion

            if (input.IsOncePressed(Buttons.A) && input.CurrentGamePadState.IsButtonUp(Buttons.LeftTrigger) && input.CurrentGamePadState.ThumbSticks.Left == Vector2.Zero)
            {
                foreach (GUIButton guiButton in _buttons)
                {
                    if (guiButton.IsSelected)
                        guiButton.PressButton();
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            foreach (GUIButton guiButton in _buttons)
            {
                guiButton.Update(gameTime);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (GUIButton guiButton in _buttons)
            {
                guiButton.Draw(gameTime);
            }
        }
    }
}
