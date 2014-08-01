using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MineLib.GraphicClient.GUIItems.Button;
using MineLib.GraphicClient.GUIItems.GamePad;
using MineLib.GraphicClient.Misc;

namespace MineLib.GraphicClient.GUIItems.InputBox
{
    /// <summary>
    /// Controlls many inputBoxes as one entity.
    /// Adds more Keyboard and GamePad support.
    /// </summary>
    public class GUIInputBoxMultiController : GUIItem
    {
        readonly List<GUIInputBox> _inputBoxes = new List<GUIInputBox>();

        GamePadDaisywheel _daisywheel;

        public GUIInputBoxMultiController(GameClient client)
        {
            GameClient = client;
        }

        public void AddGUIInputBox(GUIInputBox inputBox)
        {
            inputBox.LoadContent();
            _inputBoxes.Add(inputBox);
        }

        /// <summary>
        /// Link GUIButtons to GUIInputBox.
        /// </summary>
        /// <param name="inputBox">GUIInputBox</param>
        /// <param name="button">GUIButtons</param>
        public void AddGUIInputBox(GUIInputBox inputBox, IEnumerable<GUIButton> button)
        {
            inputBox.LoadContent();

            foreach (GUIButton guiButton in button)
            {
                inputBox.OnEmpty += guiButton.ToNonPressable;
                inputBox.OnNonEmpty += () => { if (guiButton.IsNonPressable) guiButton.ToActive(); }; // Check if other compilers handle it correct
            }
            
            _inputBoxes.Add(inputBox);
        }

        public override void LoadContent()
        {
            
        }

        public override void UnloadContent()
        {
            foreach (GUIInputBox inputBox in _inputBoxes)
            {
                inputBox.UnloadContent();
            }
        }


        void OnKeyboardCharReceived(char character)
        {
            foreach (GUIInputBox inputBox in _inputBoxes)
            {
                if(inputBox.IsSelected)
                    inputBox.InputBoxText += character;
            }
        }

        void OnKeyboardCharDelete()
        {
            foreach (GUIInputBox inputBox in _inputBoxes)
            {
                if (inputBox.IsSelected && inputBox.InputBoxText.Length > 0)
                    inputBox.InputBoxText = inputBox.InputBoxText.Remove(inputBox.InputBoxText.Length - 1, 1);
            }
        }


        public override void HandleInput(InputManager input)
        {
            foreach (GUIInputBox inputBox in _inputBoxes)
            {
                inputBox.HandleInput(input);
            }

            #region DPad handler

            // Don't blame me for this. Mah brain is broken.
            if (input.IsOncePressed(Buttons.LeftTrigger, Buttons.DPadUp) )
            {
                for (int i = _inputBoxes.Count - 1; i >= 0; i--)
                {
                    // if inputBox was found
                    if (_inputBoxes[i].IsSelected)
                    {
                        // find the previous active inputBox
                        for (int j = i; j >= 0; j--)
                        {
                            // if inputBox was found, make next inputBox active and previous inputBox selected
                            if (_inputBoxes[j].IsActive)
                            {
                                _inputBoxes[i].ToActive();
                                _inputBoxes[j].ToSelected();
                                return;
                            }
                        }
                    }

                    // if inputBox wasn't found
                    else if (i == 0)
                    {
                        // find last active inputBox and make it selected
                        for (int j = _inputBoxes.Count - 1; j >= 0; j--)
                        {
                            if (_inputBoxes[j].IsActive)
                            {
                                _inputBoxes[j].ToSelected();
                                return;
                            }
                        }
                    }
                }
            }

            // Don't blame me for this. Mah brain is broken.
            if (input.IsOncePressed(Buttons.LeftTrigger, Buttons.DPadDown))
            {
                // check if any inputBox is selected
                for (int i = 0; i < _inputBoxes.Count; i++)
                {
                    // if inputBox was found
                    if (_inputBoxes[i].IsSelected)
                    {
                        // find the next active inputBox
                        for (int j = i; j < _inputBoxes.Count; j++)
                        {
                            // if inputBox was found, make previous inputBox active and next inputBox selected
                            if (_inputBoxes[j].IsActive)
                            {
                                _inputBoxes[i].ToActive();
                                _inputBoxes[j].ToSelected();
                                return;
                            }
                        }
                    }

                    // if inputBox wasn't found
                    else if (i == _inputBoxes.Count - 1)
                    {
                        // find first active inputBox and make it selected
                        for (int j = 0; j < _inputBoxes.Count; j++)
                        {
                            if (_inputBoxes[j].IsActive)
                            {
                                _inputBoxes[j].ToSelected();
                                return;
                            }
                        }
                    }
                }
            }

            #endregion

            if (input.IsOncePressed(Buttons.LeftTrigger, Buttons.Start))
            {
                // Add to GUIItemManager as the last element because of priority drawing
                if (_daisywheel == null)
                {
                    _daisywheel = new GamePadDaisywheel(GameClient);
                    _daisywheel.OnCharReceived += OnKeyboardCharReceived;
                    _daisywheel.OnCharDeleted += OnKeyboardCharDelete;
                    _daisywheel.ToHidden();
                    GUIItemManager.AddGUIItem(_daisywheel);
                }

                if (_daisywheel.IsHidden)
                    _daisywheel.ToActive();
                else
                    _daisywheel.ToHidden();
            }

            if (input.IsOncePressed(Keys.Tab))
            {
                for (int i = _inputBoxes.Count - 1; i >= 0; i--)
                {
                    // if inputBox was found
                    if (_inputBoxes[i].IsSelected)
                    {
                        // find the previous active inputBox
                        for (int j = i; j >= 0; j--)
                        {
                            // if inputBox was found, make next inputBox active and previous inputBox selected
                            if (_inputBoxes[j].IsActive)
                            {
                                _inputBoxes[i].ToActive();
                                _inputBoxes[j].ToSelected();
                                return;
                            }
                        }
                    }

                    // if inputBox wasn't found
                    else if (i == 0)
                    {
                        // find last active inputBox and make it selected
                        for (int j = _inputBoxes.Count - 1; j >= 0; j--)
                        {
                            if (_inputBoxes[j].IsActive)
                            {
                                _inputBoxes[j].ToSelected();
                                return;
                            }
                        }
                    }
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            foreach (GUIInputBox inputBox in _inputBoxes)
            {
                inputBox.Update(gameTime);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (GUIInputBox inputBox in _inputBoxes)
            {
                inputBox.Draw(gameTime);
            }
        }
    }
}
