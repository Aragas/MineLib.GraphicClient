using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MineLib.ClientWrapper;

namespace MineLib.GraphicClient.Screens
{
    sealed class GUIScreen : InGameScreen
    {
        #region Resources
        Texture2D _crosshairTexture;

        Texture2D _widgetTexture;
        Texture2D _iconsTexture;
        #endregion

        #region Sprite rectangles
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
        #endregion

        int previousScrollValue;
        int mousevalue = 1;

        float scale = 3f;

        float exp = 0.4f;
        float health = 10f;
        short food = 10;

        public GUIScreen(GameClient gameClient, Minecraft minecraft)
        {
            GameClient = gameClient;
            Minecraft = minecraft;
        }

        public override void LoadContent()
        {
            base.LoadContent();

            _crosshairTexture = GameClient.Content.Load<Texture2D>("Crosshair");

            GUITextures guiTextures = MinecraftTexturesStorage.GUITextures;

            _widgetTexture = guiTextures.Widgets;
            _iconsTexture = guiTextures.Icons;
        }

        bool _gamepadLeftShoulderIsDown;
        bool _gamepadRightShoulderIsDown;

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            MouseState currentMouseState = Mouse.GetState();

            #region ItemList MouseScrollWheel
            if (currentMouseState.ScrollWheelValue < previousScrollValue)
            {
                if (mousevalue < 9)
                    mousevalue++;
                else
                    mousevalue = 1;
            }
            else if (currentMouseState.ScrollWheelValue > previousScrollValue)
            {
                if (mousevalue > 1)
                    mousevalue--;
                else
                    mousevalue = 9;
            }
            #endregion

            #region ItemList Keyboard
            if (Keyboard.GetState().IsKeyDown(Keys.D1))
                mousevalue = 1;

            if (Keyboard.GetState().IsKeyDown(Keys.D2))
                mousevalue = 2;

            if (Keyboard.GetState().IsKeyDown(Keys.D3))
                mousevalue = 3;

            if (Keyboard.GetState().IsKeyDown(Keys.D4))
                mousevalue = 4;

            if (Keyboard.GetState().IsKeyDown(Keys.D5))
                mousevalue = 5;

            if (Keyboard.GetState().IsKeyDown(Keys.D6))
                mousevalue = 6;

            if (Keyboard.GetState().IsKeyDown(Keys.D7))
                mousevalue = 7;

            if (Keyboard.GetState().IsKeyDown(Keys.D8))
                mousevalue = 8;

            if (Keyboard.GetState().IsKeyDown(Keys.D9))
                mousevalue = 9;
            #endregion

            #region ItemList GamepadShoulders
            if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.LeftShoulder))
            {
                if (!_gamepadLeftShoulderIsDown)
                {
                    if (mousevalue < 9)
                        mousevalue++;
                    else
                        mousevalue = 1;
                }
                _gamepadLeftShoulderIsDown = true;
            }
            else
                _gamepadLeftShoulderIsDown = false;

            if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.LeftShoulder))
            {
                if (!_gamepadRightShoulderIsDown)
                {
                    if (mousevalue > 1)
                        mousevalue--;
                    else
                        mousevalue = 9;
                }
                _gamepadRightShoulderIsDown = true;
            }
            else
                _gamepadRightShoulderIsDown = false;
            #endregion

            previousScrollValue = currentMouseState.ScrollWheelValue;
        }

        Vector2 GetItemPosition(int position, float scale )
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

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointClamp,
                DepthStencilState.None, RasterizerState.CullNone);

            #region Crosshair
            SpriteBatch.Draw(_crosshairTexture,
                    new Vector2(ScreenRectangle.Center.X, ScreenRectangle.Center.Y), // Center of screen
                    null, // Source rectangle
                    Color.White, // Color
                    0f, // Rotation
                    new Vector2(_crosshairTexture.Width, _crosshairTexture.Height) / 2, // Image center
                    1f, // Scale
                    SpriteEffects.None,
                    0f // Depth
                    );
            #endregion

            float startXPos = ScreenRectangle.Center.X - _itemListRectangle.Center.X * scale;
            float endXPos = ScreenRectangle.Center.X + _itemListRectangle.Center.X * scale;

            #region ItemList
            // ItemList
            SpriteBatch.Draw(_widgetTexture,
                    new Vector2(startXPos, ScreenRectangle.Height - _itemListRectangle.Height * scale), // Center of screen
                    _itemListRectangle, // Source rectangle
                    Color.White, // Color
                    0f, // Rotation
                    new Vector2(0), // Image center
                    scale, // Scale
                    SpriteEffects.None,
                    0f // Depth
                    );

            // SelectedItem
            SpriteBatch.Draw(_widgetTexture,
                    GetItemPosition(mousevalue, scale),
                    _selectedItemListRectangle, // Source rectangle
                    Color.White, // Color
                    0f, // Rotation
                    new Vector2(0), // Image center
                    scale, // Scale
                    SpriteEffects.None,
                    0f // Depth
                    );
            #endregion

            #region Exp
            // EmptyExp
            SpriteBatch.Draw(_iconsTexture,
                new Vector2(startXPos, ScreenRectangle.Height - _expEmptyRectangle.Height * scale - _itemListRectangle.Height * scale - 2 * scale), // Center of screen
                    _expEmptyRectangle, // Source rectangle
                    Color.White, // Color
                    0f, // Rotation
                    new Vector2(0), // Image center
                    scale, // Scale
                    SpriteEffects.None,
                    0f // Depth
                    );

            // Exp
            // TODO: float exp = Minecraft.Player.Experience.ExperienceBar;
            _expRectangle.Width = (int)(_expRectangle.Width * exp);
            SpriteBatch.Draw(_iconsTexture,
                new Vector2(startXPos, ScreenRectangle.Height - _expRectangle.Height * scale - _itemListRectangle.Height * scale - 2 * scale), // Center of screen
                    _expRectangle, // Source rectangle
                    Color.White, // Color
                    0f, // Rotation
                    new Vector2(0), // Image center
                    scale, // Scale
                    SpriteEffects.None,
                    0f // Depth
                    );
            #endregion

            #region Heart
            // Draw base
            float heartScale = scale * 0.80f;
            for (int i = 0; i < 10; i++)
            {
                SpriteBatch.Draw(_iconsTexture,
                new Vector2(startXPos + _heartEmptyRectangle.Width * heartScale * i, ScreenRectangle.Height - _heartEmptyRectangle.Height * heartScale - _itemListRectangle.Height * scale - 2 * scale - _expEmptyRectangle.Height * heartScale - 2 * heartScale), // Center of screen
                    _heartEmptyRectangle, // Source rectangle
                    Color.White, // Color
                    0f, // Rotation
                    new Vector2(0), // Image center
                    heartScale, // Scale
                    SpriteEffects.None,
                    0f // Depth
                    );
            }

            // Draw Heart
            // TODO: float health = Minecraft.Player.Health.Health;
            int fullHearts = (int)Math.Floor(health*0.5f);
            int fullHearts1 = (int)Math.Ceiling(health * 0.5f);

            for (int i = 0; i < fullHearts; i++)
            {
                SpriteBatch.Draw(_iconsTexture,
                new Vector2(startXPos + _heartEmptyRectangle.Width * heartScale * i, ScreenRectangle.Height - _heartEmptyRectangle.Height * heartScale - _itemListRectangle.Height * scale - 2 * scale - _expEmptyRectangle.Height * heartScale - 2 * heartScale), // Center of screen
                    _heartRectangle, // Source rectangle
                    Color.White, // Color
                    0f, // Rotation
                    new Vector2(0), // Image center
                    heartScale, // Scale
                    SpriteEffects.None,
                    0f // Depth
                    );
            }

            // Draw HalfHeart
            if (fullHearts1 > fullHearts)
            {
                SpriteBatch.Draw(_iconsTexture,
                    new Vector2(startXPos + _heartEmptyRectangle.Width * fullHearts * heartScale, ScreenRectangle.Height - _heartEmptyRectangle.Height * heartScale - _itemListRectangle.Height * scale - 2 * scale - _expEmptyRectangle.Height * heartScale - 2 * heartScale), // Center of screen
                    _heartHalfRectangle, // Source rectangle
                    Color.White, // Color
                    0f, // Rotation
                    new Vector2(0), // Image center
                    heartScale, // Scale
                    SpriteEffects.None,
                    0f // Depth
                    );
            }
            #endregion

            #region Food
            // Draw base
            float foodScale = scale * 0.80f;
            float foodXPos = endXPos - _foodEmptyRectangle.Width * 10 * foodScale;
            for (int i = 0; i < 10; i++)
            {
                SpriteBatch.Draw(_iconsTexture,
                new Vector2(foodXPos + _foodEmptyRectangle.Width * foodScale * i, ScreenRectangle.Height - _foodEmptyRectangle.Height * foodScale - _itemListRectangle.Height * scale - 2 * scale - _expEmptyRectangle.Height * foodScale - 2 * foodScale), // Center of screen
                    _foodEmptyRectangle, // Source rectangle
                    Color.White, // Color
                    0f, // Rotation
                    new Vector2(0), // Image center
                    foodScale, // Scale
                    SpriteEffects.None,
                    0f // Depth
                    );
            }

            // Draw Food
            // TODO: short food = Minecraft.Player.Health.Food;
            int fullFood = (int)Math.Floor(food * 0.5f);
            int fullFood1 = (int)Math.Ceiling(food * 0.5f);

            for (int i = 0; i < fullFood; i++)
            {
                SpriteBatch.Draw(_iconsTexture,
                new Vector2(foodXPos + _foodEmptyRectangle.Width * foodScale * i, ScreenRectangle.Height - _foodEmptyRectangle.Height * foodScale - _itemListRectangle.Height * scale - 2 * scale - _expEmptyRectangle.Height * foodScale - 2 * foodScale), // Center of screen
                    _foodRectangle, // Source rectangle
                    Color.White, // Color
                    0f, // Rotation
                    new Vector2(0), // Image center
                    foodScale, // Scale
                    SpriteEffects.None,
                    0f // Depth
                    );
            }

            // Draw HalfFood
            if (fullFood1 > fullFood)
            {
                SpriteBatch.Draw(_iconsTexture,
                    new Vector2(foodXPos + _foodEmptyRectangle.Width * fullHearts * foodScale, ScreenRectangle.Height - _foodEmptyRectangle.Height * foodScale - _itemListRectangle.Height * scale - 2 * scale - _expEmptyRectangle.Height * foodScale - 2 * foodScale), // Center of screen
                    _foodHalfRectangle, // Source rectangle
                    Color.White, // Color
                    0f, // Rotation
                    new Vector2(0), // Image center
                    foodScale, // Scale
                    SpriteEffects.None,
                    0f // Depth
                    );
            }
            #endregion

            SpriteBatch.End();
        }
    }
}
