using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MineLib.GraphicClient.GUIButtons;

namespace MineLib.GraphicClient.Screens
{
    sealed class AddServerScreen : Screen
    {
        public override GameClient GameClient { get; set; }

        Rectangle _screenRectangle;

        Texture2D _mainMenuTexture;

        ButtonNormal _buttonAdd;
        ButtonNormal _buttonReturn;

        SoundEffect _effect;

        public AddServerScreen(GameClient gameClient)
        {
            GameClient = gameClient;
        }

        public override void LoadContent()
        {
            base.LoadContent();

            _screenRectangle = GameClient.Window.ClientBounds;

            GUITextures guiTextures = MinecraftTexturesStorage.GUITextures;

            _mainMenuTexture = GameClient.Content.Load<Texture2D>("MainMenu");
            _effect = GameClient.Content.Load<SoundEffect>("Button.Effect");
            SpriteFont buttonFont = GameClient.Content.Load<SpriteFont>("VolterGoldfish");

            _buttonAdd = new ButtonNormal(guiTextures, buttonFont, "Add", _screenRectangle, ButtonNormalPosition.Bottom2);
            _buttonAdd.OnButtonPressed += OnAddButtonPressed;
            _buttonReturn = new ButtonNormal(guiTextures, buttonFont, "Return", _screenRectangle, ButtonNormalPosition.Bottom);
            _buttonReturn.OnButtonPressed += OnReturnButtonPressed;
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
            SetScreen(new ServerListScreen(GameClient));
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            _buttonAdd.Update(Mouse.GetState());
            _buttonReturn.Update(Mouse.GetState());
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            // Background
            spriteBatch.Draw(_mainMenuTexture, _screenRectangle, Color.White);

            // Buttons
            _buttonAdd.Draw(spriteBatch);
            _buttonReturn.Draw(spriteBatch);
        }
    }
}
