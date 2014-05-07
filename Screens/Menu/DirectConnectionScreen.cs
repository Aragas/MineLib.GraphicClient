using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
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
        }

        public override void LoadContent()
        {
            base.LoadContent();

            Texture2D widgetsTexture = MinecraftTexturesStorage.GUITextures.Widgets;

            _mainMenuTexture = GameClient.Content.Load<Texture2D>("MainMenu");
            _effect = GameClient.Content.Load<SoundEffect>("Button.Effect");
            SpriteFont buttonFont = GameClient.Content.Load<SpriteFont>("VolterGoldfish");

            _buttonConnect = new Button(widgetsTexture, buttonFont, "Connect", ScreenRectangle, ButtonEnum.Bottom2);
            _buttonConnect.OnButtonPressed += OnConnectButtonPressed;
            _buttonReturn = new Button(widgetsTexture, buttonFont, "Return", ScreenRectangle, ButtonEnum.Bottom);
            _buttonReturn.OnButtonPressed += OnReturnButtonPressed;
        }

        void OnConnectButtonPressed()
        {
            _effect.Play();
            SetScreen(new GameScreen(GameClient, GameClient.Login, GameClient.Password, GameClient.OnlineMode).Connect(ServerIP, ServerPort));
        }

        void OnReturnButtonPressed()
        {
            _effect.Play();
            SetScreen(new ServerListScreen(GameClient));
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (IsActive)
            {
                _buttonConnect.Update(gameTime);
                _buttonReturn.Update(gameTime);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if (IsActive)
            {
                // Background
                spriteBatch.Draw(_mainMenuTexture, ScreenRectangle, Color.White);

                // Buttons
                _buttonConnect.Draw(spriteBatch);
                _buttonReturn.Draw(spriteBatch);
            }
        }
    }
}
