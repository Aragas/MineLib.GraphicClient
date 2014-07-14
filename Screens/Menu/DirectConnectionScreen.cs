using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MineLib.GraphicClient.GUIItems.Buttons;


namespace MineLib.GraphicClient.Screens
{
    sealed class DirectConnectionScreen : Screen
    {
        #region Resources

        Texture2D _mainMenuTexture;
        SoundEffect _effect;

        #endregion

        string ServerIP = "127.0.0.1";
        short ServerPort = 25565;

        public DirectConnectionScreen(GameClient gameClient)
        {
            GameClient = gameClient;
            Name = "DirectConnectionScreen";
        }

        public override void LoadContent()
        {
            //_mainMenuTexture = Content.Load<Texture2D>("MainMenu");
            _mainMenuTexture = MinecraftTexturesStorage.GUITextures.OptionsBackground;
            _effect = Content.Load<SoundEffect>("Button.Effect");

            AddButtonMenu("Connect", ButtonMenuPosition.Bottom2, OnConnectButtonPressed);
            AddButtonMenu("Return", ButtonMenuPosition.Bottom, OnReturnButtonPressed);
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
            if (input.IsOncePressed(Keys.Escape))
                AddScreenAndExit(new ServerListScreen(GameClient));
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointWrap, null, null);

            // Background
            //SpriteBatch.Draw(_mainMenuTexture, ScreenRectangle, Color.White);
            SpriteBatch.Draw(_mainMenuTexture, Vector2.Zero, ScreenRectangle, SecondaryBackgroundColor, 0.0f,
                Vector2.Zero, 4.0f, SpriteEffects.None, 0.5f);
            
            SpriteBatch.End();
        }
    }
}
