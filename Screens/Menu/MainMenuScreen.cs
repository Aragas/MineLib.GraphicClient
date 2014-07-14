using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MineLib.GraphicClient.GUIButtons;

namespace MineLib.GraphicClient.Screens
{
    public sealed class MainMenuScreen : Screen
    {
        #region Resources

        private Texture2D _mainMenuTexture;

        private Texture2D _xboxController;

        private SoundEffect _effect;

        #endregion


        public MainMenuScreen(GameClient gameClient)
        {
            GameClient = gameClient;
            Name = "MainMenuScreen";
        }

        public override void LoadContent()
        {
            // Custom stuff TODO: Take from minecraft
            _mainMenuTexture = Content.Load<Texture2D>("MainMenu");
            //_mainMenuTexture = new Texture2D(GameClient.GraphicsDevice, 1, 1);
            //_mainMenuTexture.SetData(new[] { new Color(255, 255, 255, 170) });
            _effect = Content.Load<SoundEffect>("Button.Effect");

            _xboxController = Content.Load<Texture2D>("XboxControllerSpriteFont");

            AddButtonMenu("Search Server", ButtonMenuPosition.Top4, OnServerListButtonPressed);
            AddButtonMenu("Options", ButtonMenuPosition.Bottom4, OnOptionButtonPressed);
            //AddGUIButton("Exit", GUIButtonPosition.Bottom3, OnExitButtonPressed);

            ButtonMenuHalf button1 = new ButtonMenuHalf(GameClient, "Language", ButtonMenuHalfPosition.LeftBottom3);
            button1.OnButtonPressed += OnLanguageButtonPressed;
            GUIButtonManager.AddButton(button1);

            ButtonMenuHalf button2 = new ButtonMenuHalf(GameClient, "Exit", ButtonMenuHalfPosition.RightBottom3);
            button2.OnButtonPressed += OnExitButtonPressed;
            GUIButtonManager.AddButton(button2);
        }

        private void OnServerListButtonPressed()
        {
            _effect.Play();
            AddScreenAndExit(new ServerListScreen(GameClient));
        }

        private void OnOptionButtonPressed()
        {
            _effect.Play();
            AddScreenAndExit(new OptionScreen(GameClient));
        }

        private void OnLanguageButtonPressed()
        {
            _effect.Play();
            AddScreenAndExit(new LanguageScreen(GameClient));
        }

        private void OnExitButtonPressed()
        {
            _effect.Play();
            Exit();
        }

        public override void HandleInput(InputState input)
        {
            if (input.IsOncePressed(Keys.Escape))
                Exit();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null);

            // Background
            SpriteBatch.Draw(_mainMenuTexture, ScreenRectangle, Color.White);
            //SpriteBatch.Draw(panorama, panoramaLocation, Color.White);

            SpriteBatch.End();

            //XboxButtonTest();
        }

        private void XboxButtonTest()
        {
            // X button (798, 55, 78, 78)
            // A button (878, 55, 78, 78)
            // Y button (958, 55, 78, 78)
            // B button (1038, 55, 78, 78)

            int x = ScreenManager.TitleSafeArea.Height;
            int y = ScreenManager.TitleSafeArea.Width;

            SpriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.AnisotropicClamp, null, null);

            SpriteBatch.Draw(_xboxController,
                new Vector2(y - 39*2, x - 39*1), // Center of screen
                new Rectangle(798, 55, 78, 78), // Source rectangle
                Color.White, // Color
                0f, // Rotation
                new Vector2(0), // Image center
                0.5f, // Scale
                SpriteEffects.None,
                0f // Depth
                );

            SpriteBatch.Draw(_xboxController,
                new Vector2(y - 39*1, x), // Center of screen
                new Rectangle(878, 55, 78, 78), // Source rectangle
                Color.White, // Color
                0f, // Rotation
                new Vector2(0), // Image center
                0.5f, // Scale
                SpriteEffects.None,
                0f // Depth
                );

            SpriteBatch.Draw(_xboxController,
                new Vector2(y - 39*1, x - 39*2), // Center of screen
                new Rectangle(958, 55, 78, 78), // Source rectangle
                Color.White, // Color
                0f, // Rotation
                new Vector2(0), // Image center
                0.5f, // Scale
                SpriteEffects.None,
                0f // Depth
                );

            SpriteBatch.Draw(_xboxController,
                new Vector2(y, x - 39*1), // Center of screen
                new Rectangle(1038, 55, 78, 78), // Source rectangle
                Color.White, // Color
                0f, // Rotation
                new Vector2(0), // Image center
                0.5f, // Scale
                SpriteEffects.None,
                0f // Depth
                );

            SpriteBatch.End();

        }
    }
}
