using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MineLib.GraphicClient.Misc;

namespace MineLib.GraphicClient.GUIItems.InputBox
{
    public abstract class GUIInputBox : GUIItem
    {
        public delegate void InputBoxEventHandler(string text);
        public event InputBoxEventHandler OnEnterPressed;

        protected InputBoxMenuPosition Position { get; set; }

        protected Rectangle InputBoxRectangle { get; set; }

        protected Rectangle WhiteFrameTopRectangle { get; set; }
        protected Rectangle WhiteFrameBottomRectangle { get; set; }
        protected Rectangle WhiteFrameLeftRectangle { get; set; }
        protected Rectangle WhiteFrameRightRectangle { get; set; }
        protected SpriteFont ButtonFont { get { return Content.Load<SpriteFont>("Minecraftia"); } }

        public string InputBoxText = "";

        protected Color TextColor = Color.White;
        protected Color TextShadowColor = Color.Gray;
        protected float TextScale { get; set; }


        protected Texture2D BlackTexture;
        protected Texture2D WhiteFrameTexture;

        protected Vector2 TextVector;
        protected Vector2 TextShadowVector;

        protected const int CycleNumb = 40;
        protected int CycleCount = CycleNumb;

        protected void UnFocusOtherInputTextBoxes() { CycleCount = CycleNumb; GUIItemManager.UnFocusOtherInputTextBoxes(this); }

        public override void HandleInput(InputManager input)
        {

            #region Mouse handling

            MouseState mouse = input.CurrentMouseState;

            if (InputBoxRectangle.Intersects(new Rectangle(mouse.X, mouse.Y, 1, 1)) && !IsNonPressable && 
                input.CurrentMouseState.LeftButton == ButtonState.Pressed &&
                input.LastMouseState.LeftButton == ButtonState.Released)
            {
                UnFocusOtherInputTextBoxes();
                ToSelected();
            }

            #endregion

            #region Keyboard handling

            if (IsSelected)
            {
                foreach (Keys key in input.CurrentKeyboardState.GetPressedKeys())
                {
                    if (input.LastKeyboardState.IsKeyUp(key))
                    {
                        switch (key)
                        {
                            case Keys.Back:
                                if (InputBoxText.Length == 0) continue;
                                InputBoxText = InputBoxText.Remove(InputBoxText.Length - 1, 1);
                                break;

                            case Keys.Enter:
                                OnEnterPressed(InputBoxText);
                                break;

                            default:
                                InputBoxText += ConvertKeyboardInput(input.CurrentKeyboardState, key);
                                break;
                        }
                    }
                }
            }

            #endregion

        }

        protected bool ShowInput;

        // Some function from internet, heavy modified.
        private static char? ConvertKeyboardInput(KeyboardState keyboard, Keys key)
        {
            bool shift = keyboard.IsKeyDown(Keys.LeftShift) || keyboard.IsKeyDown(Keys.RightShift);

            switch (key)
            {
                //Alphabet keys
                case Keys.A: return shift ? 'A' : 'a';
                case Keys.B: return shift ? 'B' : 'b';
                case Keys.C: return shift ? 'C' : 'c';
                case Keys.D: return shift ? 'D' : 'd';
                case Keys.E: return shift ? 'E' : 'e';
                case Keys.F: return shift ? 'F' : 'f';
                case Keys.G: return shift ? 'G' : 'g';
                case Keys.H: return shift ? 'H' : 'h';
                case Keys.I: return shift ? 'I' : 'i';
                case Keys.J: return shift ? 'J' : 'j';
                case Keys.K: return shift ? 'K' : 'k';
                case Keys.L: return shift ? 'L' : 'l';
                case Keys.M: return shift ? 'M' : 'm';
                case Keys.N: return shift ? 'N' : 'n';
                case Keys.O: return shift ? 'O' : 'o';
                case Keys.P: return shift ? 'P' : 'p';
                case Keys.Q: return shift ? 'Q' : 'q';
                case Keys.R: return shift ? 'R' : 'r';
                case Keys.S: return shift ? 'S' : 's';
                case Keys.T: return shift ? 'T' : 't';
                case Keys.U: return shift ? 'U' : 'u';
                case Keys.V: return shift ? 'V' : 'v';
                case Keys.W: return shift ? 'W' : 'w';
                case Keys.X: return shift ? 'X' : 'x';
                case Keys.Y: return shift ? 'Y' : 'y';
                case Keys.Z: return shift ? 'Z' : 'z';

                //Decimal keys
                case Keys.D0: return shift ? ')' : '0';
                case Keys.D1: return shift ? '!' : '1';
                case Keys.D2: return shift ? '@' : '2';
                case Keys.D3: return shift ? '#' : '3';
                case Keys.D4: return shift ? '$' : '4';
                case Keys.D5: return shift ? '%' : '5';
                case Keys.D6: return shift ? '^' : '6';
                case Keys.D7: return shift ? '&' : '7';
                case Keys.D8: return shift ? '*' : '8';
                case Keys.D9: return shift ? '(' : '9';

                //Decimal numpad keys
                case Keys.NumPad0: return '0';
                case Keys.NumPad1: return '1';
                case Keys.NumPad2: return '2';
                case Keys.NumPad3: return '3';
                case Keys.NumPad4: return '4';
                case Keys.NumPad5: return '5';
                case Keys.NumPad6: return '6';
                case Keys.NumPad7: return '7';
                case Keys.NumPad8: return '8';
                case Keys.NumPad9: return '9';

                //Special keys
                case Keys.OemTilde: return shift ? '~' : '`';
                case Keys.OemSemicolon: return shift ? ':' : ';';
                case Keys.OemQuotes: return shift ? '"' : '\'';
                case Keys.OemQuestion: return shift ? '?' : '/';
                case Keys.OemPlus: return shift ? '+' : '=';
                case Keys.OemPipe: return shift ? '|' : '\\';
                case Keys.OemPeriod: return shift ? '>' : '.';
                case Keys.OemOpenBrackets: return shift ? '{' : '[';
                case Keys.OemCloseBrackets: return shift ? '}' : ']';
                case Keys.OemMinus: return shift ? '_' : '-';
                case Keys.OemComma: return shift ? '<' : ',';
                case Keys.Space: return ' ';

            }

            return null;
        }
    }
}
