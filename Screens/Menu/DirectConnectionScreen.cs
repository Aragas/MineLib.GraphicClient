using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MineLib.GraphicClient.GUIButtons;

namespace MineLib.GraphicClient.Screens
{
    sealed class DirectConnectionScreen : Screen
    {
        #region Resources
        Texture2D _mainMenuTexture;

        GUIButton _buttonConnect;
        GUIButton _buttonReturn;

        SoundEffect _effect;
        #endregion

        string ServerIP;
        short ServerPort;

        public DirectConnectionScreen(GameClient gameClient)
        {
            GameClient = gameClient;
            Name = "DirectConnectionScreen";
        }

        public override void LoadContent()
        {
            Texture2D widgetsTexture = MinecraftTexturesStorage.GUITextures.Widgets;

            _mainMenuTexture = Content.Load<Texture2D>("MainMenu");
            _effect = Content.Load<SoundEffect>("Button.Effect");
            SpriteFont buttonFont = Content.Load<SpriteFont>("VolterGoldfish");

            _buttonConnect = new Button(widgetsTexture, buttonFont, "Connect", ScreenRectangle, ButtonEnum.Bottom2);
            _buttonConnect.OnButtonPressed += OnConnectButtonPressed;
            _buttonReturn = new Button(widgetsTexture, buttonFont, "Return", ScreenRectangle, ButtonEnum.Bottom);
            _buttonReturn.OnButtonPressed += OnReturnButtonPressed;
        }

        public override void UnloadContent()
        {
            // Unload content only if we are in game
            if (ScreenManager.GetScreen("GameScreen") != null)
                ScreenManager.Content.Unload();
        }

        void OnConnectButtonPressed()
        {
            _effect.Play();

            GameScreen gameScreen = new GameScreen(GameClient, GameClient.Login, GameClient.Password, GameClient.OnlineMode);
            bool status = gameScreen.Connect(ServerIP, ServerPort);
            AddScreenAndExit(status ? (Screen)gameScreen : new ServerListScreen(GameClient));
        }

        void OnReturnButtonPressed()
        {
            _effect.Play();
            AddScreenAndExit(new ServerListScreen(GameClient));
        }

        public override void HandleInput(InputState input)
        {
            if (input.IsNewKeyPress(Keys.Escape))
                AddScreenAndExit(new ServerListScreen(GameClient));
        }

        public override void Update(GameTime gameTime)
        {
            _buttonConnect.Update(gameTime);
            _buttonReturn.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointClamp,
                DepthStencilState.None, RasterizerState.CullNone);

            // Background
            SpriteBatch.Draw(_mainMenuTexture, ScreenRectangle, Color.White);

            // Buttons
            _buttonConnect.Draw(SpriteBatch);
            _buttonReturn.Draw(SpriteBatch);

            SpriteBatch.End();
        }
    }
}
