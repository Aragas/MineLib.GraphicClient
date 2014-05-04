using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MineLib.GraphicClient.MonoGame.Buttons;

namespace MineLib.GraphicClient.MonoGame.Screens
{
    class ServerListScreen : Screen
    {
        bool _contentLoaded = false;

        GameClient _client;
        Rectangle _screenRectangle;

        Texture2D _mainMenuTexture;

        Button _buttonConnect;
        Button _buttonRefresh;
        Button _buttonDirectConnection;

        Button _buttonAddServer;
        Button _buttonEditServer;
        Button _buttonReturn;

        SoundEffect _effect;

        public ServerListScreen(GameClient gameClient)
        {
            _client = gameClient;
        }

        public override void LoadContent()
        {
            base.LoadContent();

            _screenRectangle = _client.Window.ClientBounds;

            _mainMenuTexture = _client.Content.Load<Texture2D>("MainMenu");

            _effect = _client.Content.Load<SoundEffect>("ButtonEffect");

            Texture2D button = _client.Content.Load<Texture2D>("Button");
            Texture2D buttonPressed = _client.Content.Load<Texture2D>("ButtonPressed");
            SpriteFont buttonFont = _client.Content.Load<SpriteFont>("VolterGoldfish");

            _buttonConnect = new Button(button, buttonPressed, buttonFont, "Connect", _screenRectangle, ButtonPosition.LeftBottom2);
            _buttonConnect.OnButtonPressed += OnConnectButtonPressed;
            _buttonRefresh = new Button(button, buttonPressed, buttonFont, "Refresh", _screenRectangle, ButtonPosition.Bottom2);
            _buttonRefresh.OnButtonPressed += OnRefreshButtonPressed;
            _buttonDirectConnection = new Button(button, buttonPressed, buttonFont, "Direct Connection", _screenRectangle, ButtonPosition.RightBottom2);
            _buttonDirectConnection.OnButtonPressed += OnDirectConnectionButtonPressed;

            _buttonAddServer = new Button(button, buttonPressed, buttonFont, "Add Server", _screenRectangle, ButtonPosition.LeftBottom);
            _buttonAddServer.OnButtonPressed += OnAddServerButtonPressed;
            _buttonEditServer = new Button(button, buttonPressed, buttonFont, "Edit Server", _screenRectangle, ButtonPosition.Bottom);
            _buttonEditServer.OnButtonPressed += OnEditServerButtonPressed;
            _buttonReturn = new Button(button, buttonPressed, buttonFont, "Return", _screenRectangle, ButtonPosition.RightBottom);
            _buttonReturn.OnButtonPressed += OnReturnButtonPressed;

            _contentLoaded = true;
        }


        void OnConnectButtonPressed()
        {
            _effect.Play();
            _client.CurrentScreen = new GameScreen();
        }

        void OnRefreshButtonPressed()
        {
            _effect.Play();
        }

        void OnDirectConnectionButtonPressed()
        {
            _effect.Play();
            _client.CurrentScreen = new DirectConnectionScreen(_client);
        }


        void OnAddServerButtonPressed()
        {
            _effect.Play();
            _client.CurrentScreen = new AddServerScreen(_client);
        }

        void OnEditServerButtonPressed()
        {
            _effect.Play();
            _client.CurrentScreen = new EditServerScreen(_client);
        }

        void OnReturnButtonPressed()
        {
            _effect.Play();
            _client.CurrentScreen = new MainScreen(_client);
        }


        public override void Update(GameTime gameTime)
        {
            if (!_contentLoaded)
                LoadContent();

            _buttonConnect.Update(Mouse.GetState());
            _buttonRefresh.Update(Mouse.GetState());
            _buttonDirectConnection.Update(Mouse.GetState());

            _buttonAddServer.Update(Mouse.GetState());
            _buttonEditServer.Update(Mouse.GetState());
            _buttonReturn.Update(Mouse.GetState());
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!_contentLoaded)
                LoadContent();

            // Background
            spriteBatch.Draw(_mainMenuTexture, new Rectangle(0, 0, _screenRectangle.Width, _screenRectangle.Height), Color.White);

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
