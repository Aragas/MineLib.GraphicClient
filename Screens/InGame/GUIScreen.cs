using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MineLib.ClientWrapper;
using MineLib.GraphicClient.Misc;

namespace MineLib.GraphicClient.Screens
{
    class GUIScreen : InGameScreen
    {
        public bool WindowOpened { get; set; }

        public bool WindowInventory { get; set; }

        public bool ChatInputBoxEnabled { get; set; }

        public string ChatInputBoxText { get; set; }

        Rectangle ChatWindow { get; set; }
        Rectangle InputWindow { get; set; }

        #region Resources

        Texture2D _backgroundTexture;
        SpriteFont _font { get { return Content.Load<SpriteFont>("Minecraftia"); }}

        Texture2D _widgetTexture;
        Texture2D _iconsTexture;
        Texture2D _inventoryTexture;

        #endregion

        #region Sprite rectangles

        Rectangle _crosshairRectangle = new Rectangle(0, 0, 16, 16);

        Rectangle _itemListRectangle = new Rectangle(0, 0, 182, 22);
        Rectangle _selectedItemListRectangle = new Rectangle(0, 22, 24, 24);

        Rectangle _expEmptyRectangle = new Rectangle(0, 64, 182, 5);
        Rectangle _expRectangle = new Rectangle(0, 69, 182, 5);

        Rectangle _heartEmptyRectangle = new Rectangle(16, 0, 9, 9);
        Rectangle _heartRectangle = new Rectangle(52, 0, 9, 9);
        Rectangle _heartHalfRectangle = new Rectangle(61, 0, 9, 9);

        Rectangle _foodEmptyRectangle = new Rectangle(16, 27, 9, 9);
        Rectangle _foodRectangle = new Rectangle(52, 27, 9, 9);
        Rectangle _foodHalfRectangle = new Rectangle(61, 27, 9, 9);

        Rectangle _inventoryRectangle = new Rectangle(0, 0, 176, 166);

        #endregion

        #region Values

        int _mouseState = 1;

        float _scale = 3f;

        #endregion


        public GUIScreen(GameClient gameClient, Minecraft minecraft, PlayerInteractionValues playerInteractionValues)
        {
            GameClient = gameClient;
            Minecraft = minecraft;
            PlayerInteractionValues = playerInteractionValues;
            Name = "GUIScreen";

            WindowOpened = false;
        }

        public override void LoadContent()
        {
            _backgroundTexture = new Texture2D(GraphicsDevice, 1, 1);
            _backgroundTexture.SetData(new[] { new Color(0, 0, 0, 150) });

            GUITextures guiTextures = MinecraftTexturesStorage.GUITextures;

            _widgetTexture = guiTextures.Widgets;
            _iconsTexture = guiTextures.Icons;
            _inventoryTexture = guiTextures.Inventory;

            ChatInputBoxText = "";

            ChatWindow = new Rectangle(10, (ScreenRectangle.Height - 400 - 45), 600, 400);
            InputWindow = new Rectangle(10, (ScreenRectangle.Height - 35), (ScreenRectangle.Width - 20), 25);
        }

        public override void HandleInput(InputManager input)
        {

            #region Message handling

            // Do not handle input if chat is used
            if (PlayerInteractionValues.ChatOn)
            {
                foreach (Keys key in input.CurrentKeyboardState.GetPressedKeys())
                {
                    if (input.LastKeyboardState.IsKeyUp(key))
                    {
                        switch (key)
                        {
                            case Keys.Back:
                                if (ChatInputBoxText.Length == 0) continue;
                                ChatInputBoxText = ChatInputBoxText.Remove(ChatInputBoxText.Length - 1, 1);
                                break;

                            case Keys.Enter:
                                Minecraft.SendChatMessage(ChatInputBoxText);
                                ChatInputBoxText = "";
                                PlayerInteractionValues.ChatOn = false;
                                break;

                            default:
                                ChatInputBoxText += ConvertKeyboardInput(input.CurrentKeyboardState, key);
                                break;
                        }
                    }
                }

                return;
            }

            #endregion

            if (input.IsOncePressed(Keys.Escape) && !WindowOpened)
                ScreenManager.AddScreen(new GameOptionScreen(GameClient));
            else if (input.IsOncePressed(Keys.Escape) && WindowOpened)
                CloseWindow();

            if (input.IsOncePressed(Keys.E))
            {
                WindowInventory = !WindowInventory;

                // For better understanding
                if (WindowInventory)
                    WindowOpened = true;
                else
                    WindowOpened = false;
            }

            if (input.IsOncePressed(Keys.Enter))
                PlayerInteractionValues.ChatOn = !PlayerInteractionValues.ChatOn;
            

            #region ItemList MouseScrollWheel
            if (input.MouseScrollDown)
            {
                if (_mouseState < 9)
                    _mouseState++;
                else
                    _mouseState = 1;
            }
            else if (input.MouseScrollUp)
            {
                if (_mouseState > 1)
                    _mouseState--;
                else
                    _mouseState = 9;
            }
            #endregion

            #region ItemList Keyboard
            if (input.IsOncePressed(Keys.D1))
                _mouseState = 1;

            if (input.IsOncePressed(Keys.D2))
                _mouseState = 2;

            if (input.IsOncePressed(Keys.D3))
                _mouseState = 3;

            if (input.IsOncePressed(Keys.D4))
                _mouseState = 4;

            if (input.IsOncePressed(Keys.D5))
                _mouseState = 5;

            if (input.IsOncePressed(Keys.D6))
                _mouseState = 6;

            if (input.IsOncePressed(Keys.D7))
                _mouseState = 7;

            if (input.IsOncePressed(Keys.D8))
                _mouseState = 8;

            if (input.IsOncePressed(Keys.D9))
                _mouseState = 9;
            #endregion

            #region ItemList GamepadShoulders

            if (input.GUIMenuRight)
            {
                if (_mouseState < 9)
                    _mouseState++;
                else
                    _mouseState = 1;
            }

            if (input.GUIMenuLeft)
            {
                if (_mouseState > 1)
                    _mouseState--;
                else
                    _mouseState = 9;
            }

            #endregion

        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointWrap, null, null);

            float startXPos = ScreenRectangle.Center.X - _itemListRectangle.Center.X * _scale;
            float endXPos = ScreenRectangle.Center.X + _itemListRectangle.Center.X * _scale;

            #region Crosshair

            Vector2 crosshairVector = new Vector2(ScreenRectangle.Center.X, ScreenRectangle.Center.Y);
            Vector2 crosshairOrigin = new Vector2(_crosshairRectangle.Width, _crosshairRectangle.Height) / 2;

            SpriteBatch.Draw(_iconsTexture, crosshairVector, _crosshairRectangle, Color.White, 0f, crosshairOrigin, 1f, SpriteEffects.None, 0f);

            #endregion

            #region ItemList

            // ItemList
            Vector2 itemListVector = new Vector2(startXPos, ScreenRectangle.Height - _itemListRectangle.Height * _scale);

            SpriteBatch.Draw(_widgetTexture, itemListVector, _itemListRectangle, Color.White, 0f, new Vector2(0), _scale, SpriteEffects.None, 0f);

            // SelectedItem
            SpriteBatch.Draw(_widgetTexture, GetItemPosition(_mouseState, _scale), _selectedItemListRectangle, Color.White, 0f, new Vector2(0), _scale, SpriteEffects.None, 0f);

            #endregion

            #region Exp

            // EmptyExp
            Vector2 emptyExpVector = new Vector2(startXPos, ScreenRectangle.Height - _expEmptyRectangle.Height * _scale - _itemListRectangle.Height * _scale - 2 * _scale);

            SpriteBatch.Draw(_iconsTexture, emptyExpVector, _expEmptyRectangle, Color.White, 0f, new Vector2(0), _scale, SpriteEffects.None, 0f);

            // Exp
            _expRectangle.Width = (int)(_expRectangle.Width * Minecraft.Player.Experience.ExperienceBar);
            Vector2 expVector = new Vector2(startXPos, ScreenRectangle.Height - _expRectangle.Height * _scale - _itemListRectangle.Height * _scale - 2 * _scale);

            SpriteBatch.Draw(_iconsTexture, expVector, _expRectangle, Color.White,  0f, new Vector2(0), _scale, SpriteEffects.None, 0f);

            #endregion

            #region Heart

            // Draw base
            float heartScale = _scale * 0.80f;

            for (int i = 0; i < 10; i++)
            {
                Vector2 baseHeartVector = new Vector2(startXPos + _heartEmptyRectangle.Width * heartScale * i, ScreenRectangle.Height - _heartEmptyRectangle.Height * heartScale - _itemListRectangle.Height * _scale - 2 * _scale - _expEmptyRectangle.Height * heartScale - 2 * heartScale);

                SpriteBatch.Draw(_iconsTexture, baseHeartVector, _heartEmptyRectangle, Color.White, 0f, new Vector2(0), heartScale, SpriteEffects.None, 0f);
            }

            // Draw Heart
            int fullHearts = (int)Math.Floor(Minecraft.Player.Health.Health * 0.5f);
            int fullHearts1 = (int)Math.Ceiling(Minecraft.Player.Health.Health * 0.5f);

            for (int i = 0; i < fullHearts; i++)
            {
                Vector2 heartVector = new Vector2(startXPos + _heartEmptyRectangle.Width * heartScale * i, ScreenRectangle.Height - _heartEmptyRectangle.Height * heartScale - _itemListRectangle.Height * _scale - 2 * _scale - _expEmptyRectangle.Height * heartScale - 2 * heartScale);

                SpriteBatch.Draw(_iconsTexture, heartVector, _heartRectangle, Color.White, 0f, new Vector2(0), heartScale, SpriteEffects.None, 0f);
            }

            // Draw HalfHeart
            if (fullHearts1 > fullHearts)
            {
                Vector2 halfHeartVector = new Vector2(startXPos + _heartEmptyRectangle.Width * fullHearts * heartScale, ScreenRectangle.Height - _heartEmptyRectangle.Height * heartScale - _itemListRectangle.Height * _scale - 2 * _scale - _expEmptyRectangle.Height * heartScale - 2 * heartScale);

                SpriteBatch.Draw(_iconsTexture, halfHeartVector, _heartHalfRectangle, Color.White,  0f, new Vector2(0), heartScale, SpriteEffects.None, 0f);
            }

            #endregion

            #region Food

            // Draw base
            float foodScale = _scale * 0.80f;
            float foodXPos = endXPos - _foodEmptyRectangle.Width * 10 * foodScale;

            for (int i = 0; i < 10; i++)
            {
                Vector2 baseFoodVector = new Vector2(foodXPos + _foodEmptyRectangle.Width * foodScale * i, ScreenRectangle.Height - _foodEmptyRectangle.Height * foodScale - _itemListRectangle.Height * _scale - 2 * _scale - _expEmptyRectangle.Height * foodScale - 2 * foodScale);
                
                SpriteBatch.Draw(_iconsTexture, baseFoodVector, _foodEmptyRectangle, Color.White, 0f, new Vector2(0), foodScale, SpriteEffects.None, 0f);
            }

            // Draw Food
            // TODO: short food = Minecraft.Player.Health.Food;
            int fullFood = (int)Math.Floor(Minecraft.Player.Health.Food * 0.5f);
            int fullFood1 = (int)Math.Ceiling(Minecraft.Player.Health.Food * 0.5f);

            for (int i = 0; i < fullFood; i++)
            {
                Vector2 foodVector = new Vector2(foodXPos + _foodEmptyRectangle.Width * foodScale * i, ScreenRectangle.Height - _foodEmptyRectangle.Height * foodScale - _itemListRectangle.Height * _scale - 2 * _scale - _expEmptyRectangle.Height * foodScale - 2 * foodScale);

                SpriteBatch.Draw(_iconsTexture, foodVector, _foodRectangle, Color.White, 0f, new Vector2(0), foodScale, SpriteEffects.None, 0f);
            }

            // Draw HalfFood
            if (fullFood1 > fullFood)
            {
                Vector2 halfFoodVector = new Vector2(foodXPos + _foodEmptyRectangle.Width * fullHearts * foodScale, ScreenRectangle.Height - _foodEmptyRectangle.Height * foodScale - _itemListRectangle.Height * _scale - 2 * _scale - _expEmptyRectangle.Height * foodScale - 2 * foodScale);

                SpriteBatch.Draw(_iconsTexture, halfFoodVector, _foodHalfRectangle, Color.White, 0f, new Vector2(0), foodScale, SpriteEffects.None, 0f);
            }

            #endregion

            #region Inventory

            if (WindowInventory)
            {
                Vector2 inventoryPosition = new Vector2(ScreenRectangle.Center.X, ScreenRectangle.Center.Y);
                Vector2 inventoryOrigin = new Vector2(_inventoryRectangle.Width, _inventoryRectangle.Height)/2;

                SpriteBatch.Draw(_inventoryTexture, inventoryPosition, _inventoryRectangle, Color.White, 0f, inventoryOrigin, _scale - 1f, SpriteEffects.None, 0f);
            }

            #endregion

            #region Chat

            if (PlayerInteractionValues.ChatOn)
            {
                // Chat Box
                SpriteBatch.Draw(_backgroundTexture, ChatWindow, Color.White);
                for (int i = 0; i < Minecraft.ChatHistory.Count; i++)
                {
                    if (i >= 16)
                        break;

                    DrawString(SpriteBatch, _font, Color.Black, Minecraft.ChatHistory[i], new Rectangle(15 + 1, ScreenRectangle.Height - 45 - 25 - 25 * i, ChatWindow.Width + 1, InputWindow.Height));
                    DrawString(SpriteBatch, _font, Color.White, Minecraft.ChatHistory[i], new Rectangle(15, ScreenRectangle.Height - 45 - 25 - 25 * i, ChatWindow.Width, InputWindow.Height));
                }

                // Chat Input Box
                SpriteBatch.Draw(_backgroundTexture, InputWindow, Color.White);
                DrawString(SpriteBatch, _font, Color.Black, ChatInputBoxText, new Rectangle(15 + 1, ScreenRectangle.Height - 35 + 1, InputWindow.Width, InputWindow.Height));
                DrawString(SpriteBatch, _font, Color.White, ChatInputBoxText, new Rectangle(15, ScreenRectangle.Height - 35, InputWindow.Width, InputWindow.Height));
            }

            #endregion

            SpriteBatch.End();
        }
        

        void CloseWindow()
        {
            if (WindowInventory)
            {
                WindowInventory = false;
                WindowOpened = false;
            }
        }

        Vector2 GetItemPosition(int position, float scale)
        {
            switch (position)
            {
                case 1:
                    return new Vector2(ScreenRectangle.Center.X - (_selectedItemListRectangle.Width * 0.5f) * scale - 20 * 4f * scale,
                            ScreenRectangle.Height - _selectedItemListRectangle.Height * scale + 1 * scale);
                case 2:
                    return new Vector2(ScreenRectangle.Center.X - (_selectedItemListRectangle.Width * 0.5f) * scale - 20 * 3f * scale,
                            ScreenRectangle.Height - _selectedItemListRectangle.Height * scale + 1 * scale);
                case 3:
                    return new Vector2(ScreenRectangle.Center.X - (_selectedItemListRectangle.Width * 0.5f) * scale - 20 * 2f * scale,
                            ScreenRectangle.Height - _selectedItemListRectangle.Height * scale + 1 * scale);
                case 4:
                    return new Vector2(ScreenRectangle.Center.X - (_selectedItemListRectangle.Width * 0.5f) * scale - 20 * 1f * scale,
                            ScreenRectangle.Height - _selectedItemListRectangle.Height * scale + 1 * scale);
                case 5:
                    return new Vector2(ScreenRectangle.Center.X - (_selectedItemListRectangle.Width * 0.5f) * scale,
                            ScreenRectangle.Height - _selectedItemListRectangle.Height * scale + 1 * scale);
                case 6:
                    return new Vector2(ScreenRectangle.Center.X - (_selectedItemListRectangle.Width * 0.5f) * scale + 20 * 1f * scale,
                            ScreenRectangle.Height - _selectedItemListRectangle.Height * scale + 1 * scale);
                case 7:
                    return new Vector2(ScreenRectangle.Center.X - (_selectedItemListRectangle.Width * 0.5f) * scale + 20 * 2f * scale,
                            ScreenRectangle.Height - _selectedItemListRectangle.Height * scale + 1 * scale);
                case 8:
                    return new Vector2(ScreenRectangle.Center.X - (_selectedItemListRectangle.Width * 0.5f) * scale + 20 * 3f * scale,
                            ScreenRectangle.Height - _selectedItemListRectangle.Height * scale + 1 * scale);
                case 9:
                    return new Vector2(ScreenRectangle.Center.X - (_selectedItemListRectangle.Width * 0.5f) * scale + 20 * 4f * scale,
                            ScreenRectangle.Height - _selectedItemListRectangle.Height * scale + 1 * scale);
            }
            return Vector2.Zero;
        }

        // Some function from internet, heavy modified.
        static char? ConvertKeyboardInput(KeyboardState keyboard, Keys key)
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

        static void DrawString(SpriteBatch spriteBatch, SpriteFont font, Color color, string strToDraw, Rectangle boundaries)
        {
            Vector2 size = font.MeasureString(strToDraw);

            float xScale = (boundaries.Width / size.X);
            float yScale = (boundaries.Height / size.Y);

            // Taking the smaller scaling value will result in the text always fitting in the boundaries.
            float scale = Math.Min(xScale, yScale);

            Vector2 position = new Vector2 {X = boundaries.X, Y = boundaries.Y};

            // A bunch of settings where we just want to use reasonable defaults.
            float rotation = 0.0f;
            Vector2 spriteOrigin = new Vector2(0, 0);
            float spriteLayer = 0.0f; // all the way in the front
            SpriteEffects spriteEffects = new SpriteEffects();

            // Draw the string to the sprite batch!
            spriteBatch.DrawString(font, strToDraw, position, color, rotation, spriteOrigin, scale, spriteEffects, spriteLayer);
        }
    }
}
