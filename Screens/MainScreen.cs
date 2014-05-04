using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MineLib.GraphicClient.MonoGame.Screens
{
    public class MainScreen : Screen
    {
        GameClient _client;
        Rectangle _screenRectangle;

        Texture2D _mainMenuTexture;

        Button _buttonServerList;
        Button _buttonOption;
        Button _buttonExit;

        SoundEffect _effect;

        public MainScreen(GameClient gameClient)
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

            _buttonServerList = new Button(button, buttonPressed, buttonFont, "Search Server", _screenRectangle, ButtonPosition.Center);
            _buttonServerList.OnButtonPressed += OnServerListButtonPressed;
            _buttonOption = new Button(button, buttonPressed, buttonFont, "Options", _screenRectangle, ButtonPosition.Bottom3);
            _buttonOption.OnButtonPressed += OnOptionButtonPressed;
            _buttonExit = new Button(button, buttonPressed, buttonFont, "Exit", _screenRectangle, ButtonPosition.Bottom2);
            _buttonExit.OnButtonPressed += OnExitButtonPressed;
        }

        void OnServerListButtonPressed()
        {
            _effect.Play();
            _client.CurrentScreen = new GameScreen();
        }

        void OnOptionButtonPressed()
        {
            _effect.Play();
        }

        void OnExitButtonPressed()
        {
            _effect.Play();
            _client.Exit();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            _buttonServerList.Update(Mouse.GetState());
            _buttonOption.Update(Mouse.GetState());
            _buttonExit.Update(Mouse.GetState());
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            // Background
            spriteBatch.Draw(_mainMenuTexture, new Rectangle(0, 0, _screenRectangle.Width, _screenRectangle.Height), Color.White);

            // Buttons
            _buttonServerList.Draw(spriteBatch);
            _buttonOption.Draw(spriteBatch);
            _buttonExit.Draw(spriteBatch);
        }
    }
}
