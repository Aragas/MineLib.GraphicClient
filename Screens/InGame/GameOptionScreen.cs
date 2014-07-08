using System.ComponentModel.Design.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MineLib.GraphicClient.GUIButtons;

namespace MineLib.GraphicClient.Screens
{
    sealed class GameOptionScreen : InGameScreen
    {
        #region Resources
        Texture2D _backgroundTexture;

        GUIButton _buttonBackToGame;
        GUIButton _buttonOptions;
        GUIButton _buttonDisconnect;

        SoundEffect _effect;
        #endregion

        public GameOptionScreen(GameClient client)
        {
            GameClient = client;
            Name = "GameOptionScreen";
        }

        public override void LoadContent()
        {
            ScreenManager.GetScreen("GameScreen").ToBackground();
            ScreenManager.GetScreen("GUIScreen").ToBackground();

            _backgroundTexture = new Texture2D(GameClient.GraphicsDevice, 1, 1);
            _backgroundTexture.SetData(new[] { new Color(0, 0, 0, 170) });

            Texture2D widgetsTexture = MinecraftTexturesStorage.GUITextures.Widgets;

            _effect = Content.Load<SoundEffect>("Button.Effect");
            SpriteFont buttonFont = Content.Load<SpriteFont>("VolterGoldfish");

            _buttonBackToGame = new Button(widgetsTexture, buttonFont, "Back To Game", ScreenRectangle, ButtonEnum.Top3);
            _buttonBackToGame.OnButtonPressed += OnBackToGameButtonPressed;

            _buttonOptions = new Button(widgetsTexture, buttonFont, "Options", ScreenRectangle, ButtonEnum.Bottom4);
            _buttonOptions.OnButtonPressed += OnOptionsButtonPressed;

            _buttonDisconnect = new Button(widgetsTexture, buttonFont, "Disconnect", ScreenRectangle, ButtonEnum.Bottom2);
            _buttonDisconnect.OnButtonPressed += OnDisconnectButtonPressed;

        }

        void OnBackToGameButtonPressed()
        {
            _effect.Play();
            ExitScreen();
        }

        void OnOptionsButtonPressed()
        {
            _effect.Play();
        }

        void OnDisconnectButtonPressed()
        {
            _effect.Play();
            AddScreenAndCloseOthers(new ServerListScreen(GameClient));
        }

        public override void HandleInput(InputState input)
        {
            if (input.IsNewKeyPress(Keys.Escape))
                ExitScreen();

            _buttonBackToGame.HandleInput(input);
            _buttonOptions.HandleInput(input);
            _buttonDisconnect.HandleInput(input);
        }

        public override void Update(GameTime gameTime)
        {
            _buttonBackToGame.Update(gameTime);
            _buttonOptions.Update(gameTime);
            _buttonDisconnect.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointClamp,
                DepthStencilState.None, RasterizerState.CullNone);

            // Backgroung
            SpriteBatch.Draw(_backgroundTexture, ScreenRectangle, Color.White);

            // Buttons
            _buttonBackToGame.Draw(SpriteBatch);
            _buttonOptions.Draw(SpriteBatch);
            _buttonDisconnect.Draw(SpriteBatch);

            SpriteBatch.End();
        }

        public override void ExitScreen()
        {
            ScreenManager.GetScreen("GameScreen").ToActive();
            ScreenManager.GetScreen("GUIScreen").ToActive();

            base.ExitScreen();
        }
    }
}
