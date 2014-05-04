using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MineLib.GraphicClient.MonoGame.Buttons;

namespace MineLib.GraphicClient.MonoGame.Screens
{
    class AddServerScreen : Screen
    {
        bool _contentLoaded = false;

        GameClient _client;
        Rectangle _screenRectangle;

        Texture2D _mainMenuTexture;

        Button _buttonAdd;
        Button _buttonReturn;

        SoundEffect _effect;

        public AddServerScreen(GameClient gameClient)
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

            _buttonAdd = new Button(button, buttonPressed, buttonFont, "Add", _screenRectangle, ButtonPosition.Bottom2);
            _buttonAdd.OnButtonPressed += OnAddButtonPressed;
            _buttonReturn = new Button(button, buttonPressed, buttonFont, "Return", _screenRectangle, ButtonPosition.Bottom);
            _buttonReturn.OnButtonPressed += OnReturnButtonPressed;

            _contentLoaded = true;
        }

        void OnAddButtonPressed()
        {
            _effect.Play();
            //_client.CurrentScreen = new ServerListScreen(_client);
            // TODO : Check if data is correct
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

            _buttonAdd.Update(Mouse.GetState());
            _buttonReturn.Update(Mouse.GetState());
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!_contentLoaded)
                LoadContent();

            // Background
            spriteBatch.Draw(_mainMenuTexture, new Rectangle(0, 0, _screenRectangle.Width, _screenRectangle.Height), Color.White);

            // Buttons
            _buttonAdd.Draw(spriteBatch);
            _buttonReturn.Draw(spriteBatch);
        }
    }
}
