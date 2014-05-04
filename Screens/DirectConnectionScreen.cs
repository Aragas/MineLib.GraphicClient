using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MineLib.GraphicClient.MonoGame.Buttons;

namespace MineLib.GraphicClient.MonoGame.Screens
{
    class DirectConnectionScreen : Screen
    {
        bool _contentLoaded = false;

        GameClient _client;
        Rectangle _screenRectangle;

        Texture2D _mainMenuTexture;

        Button _buttonConnect;
        Button _buttonReturn;

        SoundEffect _effect;

        public DirectConnectionScreen(GameClient gameClient)
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

            _buttonConnect = new Button(button, buttonPressed, buttonFont, "Connect", _screenRectangle, ButtonPosition.Bottom2);
            _buttonConnect.OnButtonPressed += OnConnectButtonPressed;
            _buttonReturn = new Button(button, buttonPressed, buttonFont, "Return", _screenRectangle, ButtonPosition.Bottom);
            _buttonReturn.OnButtonPressed += OnReturnButtonPressed;

            _contentLoaded = true;
        }

        void OnConnectButtonPressed()
        {
            _effect.Play();
            _client.CurrentScreen = new GameScreen();
        }

        void OnReturnButtonPressed()
        {
            _effect.Play();
            _client.CurrentScreen = new ServerListScreen(_client);
        }

        public override void Update(GameTime gameTime)
        {
            if (!_contentLoaded)
                LoadContent();

            _buttonConnect.Update(Mouse.GetState());
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
            _buttonReturn.Draw(spriteBatch);
        }

    }
}
