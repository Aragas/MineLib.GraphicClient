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

        ButtonNormal _buttonConnect;
        ButtonNormal _buttonReturn;

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

            _buttonConnect = new ButtonNormal(widgetsTexture, buttonFont, "Connect", ScreenRectangle, ButtonNormalPosition.Bottom2);
            _buttonConnect.OnButtonPressed += OnConnectButtonPressed;
            _buttonReturn = new ButtonNormal(widgetsTexture, buttonFont, "Return", ScreenRectangle, ButtonNormalPosition.Bottom);
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

            _buttonConnect.Update(Mouse.GetState());
            _buttonReturn.Update(Mouse.GetState());
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            // Background
            spriteBatch.Draw(_mainMenuTexture, ScreenRectangle, Color.White);

            // Buttons
            _buttonConnect.Draw(spriteBatch);
            _buttonReturn.Draw(spriteBatch);
        }
    }
}
