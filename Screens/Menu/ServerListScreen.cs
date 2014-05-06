using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MineLib.GraphicClient.GUIButtons;

namespace MineLib.GraphicClient.Screens
{
    sealed class ServerListScreen : Screen
    {
        #region Resources
        Texture2D _mainMenuTexture;

        ButtonNormal _buttonConnect;
        ButtonNormal _buttonRefresh;
        ButtonNormal _buttonDirectConnection;

        ButtonNormal _buttonAddServer;
        ButtonNormal _buttonEditServer;
        ButtonNormal _buttonReturn;

        SoundEffect _effect;
        #endregion

        string ServerIP = "127.0.0.1";
        short ServerPort = 25565;

        public ServerListScreen(GameClient gameClient)
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

            _buttonConnect = new ButtonNormal(widgetsTexture, buttonFont, "Connect", ScreenRectangle, ButtonNormalPosition.LeftBottom2);
            _buttonConnect.OnButtonPressed += OnConnectButtonPressed;
            _buttonRefresh = new ButtonNormal(widgetsTexture, buttonFont, "Refresh", ScreenRectangle, ButtonNormalPosition.Bottom2);
            _buttonRefresh.OnButtonPressed += OnRefreshButtonPressed;
            _buttonDirectConnection = new ButtonNormal(widgetsTexture, buttonFont, "Direct Connection", ScreenRectangle, ButtonNormalPosition.RightBottom2);
            _buttonDirectConnection.OnButtonPressed += OnDirectConnectionButtonPressed;

            _buttonAddServer = new ButtonNormal(widgetsTexture, buttonFont, "Add Server", ScreenRectangle, ButtonNormalPosition.LeftBottom);
            _buttonAddServer.OnButtonPressed += OnAddServerButtonPressed;
            _buttonEditServer = new ButtonNormal(widgetsTexture, buttonFont, "Edit Server", ScreenRectangle, ButtonNormalPosition.Bottom);
            _buttonEditServer.OnButtonPressed += OnEditServerButtonPressed;
            _buttonReturn = new ButtonNormal(widgetsTexture, buttonFont, "Return", ScreenRectangle, ButtonNormalPosition.RightBottom);
            _buttonReturn.OnButtonPressed += OnReturnButtonPressed;
        }


        void OnConnectButtonPressed()
        {
            _effect.Play();
            SetScreen(new GameScreen(GameClient, GameClient.Login, GameClient.Password, GameClient.OnlineMode).Connect(ServerIP, ServerPort));
        }

        void OnRefreshButtonPressed()
        {
            _effect.Play();
        }

        void OnDirectConnectionButtonPressed()
        {
            _effect.Play();
            SetScreen(new DirectConnectionScreen(GameClient));
        }


        void OnAddServerButtonPressed()
        {
            _effect.Play();
            SetScreen(new AddServerScreen(GameClient));
        }

        void OnEditServerButtonPressed()
        {
            _effect.Play();
            SetScreen(new EditServerScreen(GameClient));
        }

        void OnReturnButtonPressed()
        {
            _effect.Play();
            SetScreen(new MainMenuScreen(GameClient));
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            _buttonConnect.Update(Mouse.GetState());
            _buttonRefresh.Update(Mouse.GetState());
            _buttonDirectConnection.Update(Mouse.GetState());

            _buttonAddServer.Update(Mouse.GetState());
            _buttonEditServer.Update(Mouse.GetState());
            _buttonReturn.Update(Mouse.GetState());
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            // Background
            spriteBatch.Draw(_mainMenuTexture, ScreenRectangle, Color.White);

            // Buttons
            _buttonConnect.Draw(spriteBatch);
            _buttonRefresh.Draw(spriteBatch);
            _buttonDirectConnection.Draw(spriteBatch);

            _buttonAddServer.Draw(spriteBatch);
            _buttonEditServer.Draw(spriteBatch);
            _buttonReturn.Draw(spriteBatch);
        }
    }
}
