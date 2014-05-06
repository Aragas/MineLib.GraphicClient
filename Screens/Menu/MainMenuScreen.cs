using System.IO;
using Ionic.Zip;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MineLib.GraphicClient.GUIButtons;

namespace MineLib.GraphicClient.Screens
{
    public sealed class MainMenuScreen : Screen
    {
        public override GameClient GameClient { get; set; }

        Rectangle _screenRectangle;

        Texture2D _mainMenuTexture;

        ButtonNormal _buttonServerList;
        ButtonNormal _buttonOption;
        ButtonNormal _buttonExit;

        SoundEffect _effect;

        public MainMenuScreen(GameClient gameClient)
        {
            GameClient = gameClient;
        }

        public override void LoadContent()
        {
            base.LoadContent();

            _screenRectangle = GameClient.Window.ClientBounds;

            GUITextures guiTextures = MinecraftTexturesStorage.GUITextures;

            // Custom stuff TODO: Take from minecraft
            _mainMenuTexture = GameClient.Content.Load<Texture2D>("MainMenu");
            _effect = GameClient.Content.Load<SoundEffect>("Button.Effect");
            SpriteFont buttonFont = GameClient.Content.Load<SpriteFont>("VolterGoldfish");

            _buttonServerList = new ButtonNormal(guiTextures, buttonFont, "Search Server", _screenRectangle, ButtonNormalPosition.Top4);
            _buttonServerList.OnButtonPressed += OnServerListButtonPressed;
            _buttonOption = new ButtonNormal(guiTextures, buttonFont, "Options", _screenRectangle, ButtonNormalPosition.Bottom4);
            _buttonOption.OnButtonPressed += OnOptionButtonPressed;
            _buttonExit = new ButtonNormal(guiTextures, buttonFont, "Exit", _screenRectangle, ButtonNormalPosition.Bottom3);
            _buttonExit.OnButtonPressed += OnExitButtonPressed;
        }

        void OnServerListButtonPressed()
        {
            _effect.Play();
            SetScreen(new ServerListScreen(GameClient));
        }

        void OnOptionButtonPressed()
        {
            _effect.Play();
            SetScreen(new OptionScreen());
        }

        void OnExitButtonPressed()
        {
            _effect.Play();
            Exit();
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
            spriteBatch.Draw(_mainMenuTexture, _screenRectangle, Color.White);

            // Buttons
            _buttonServerList.Draw(spriteBatch);
            _buttonOption.Draw(spriteBatch);
            _buttonExit.Draw(spriteBatch);
        }
    }
}
