using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MineLib.GraphicClient.GUIButtons;

namespace MineLib.GraphicClient.Screens
{
    sealed class GameOptionScreen : Screen
    {
        public override GameClient GameClient { get; set; }

        Rectangle _screenRectangle;

        ButtonNormal _buttonBackToGame;
        ButtonNormal _buttonOptions;
        ButtonNormal _buttonDisconnect;

        SoundEffect _effect;

        public GameOptionScreen(GameClient client)
        {
            GameClient = client;
        }

        public override void LoadContent()
        {
            _screenRectangle = GameClient.Window.ClientBounds;

            GUITextures guiTextures = MinecraftTexturesStorage.GUITextures;

            _effect = GameClient.Content.Load<SoundEffect>("Button.Effect");
            SpriteFont buttonFont = GameClient.Content.Load<SpriteFont>("VolterGoldfish");

            _buttonBackToGame = new ButtonNormal(guiTextures, buttonFont, "Back To Game", _screenRectangle, ButtonNormalPosition.Top3);
            _buttonBackToGame.OnButtonPressed += OnBackToGameButtonPressed;

            _buttonOptions = new ButtonNormal(guiTextures, buttonFont, "Options", _screenRectangle, ButtonNormalPosition.Bottom4);
            _buttonOptions.OnButtonPressed += OnOptionsButtonPressed;

            _buttonDisconnect = new ButtonNormal(guiTextures, buttonFont, "Disconnect", _screenRectangle, ButtonNormalPosition.Bottom2);
            _buttonDisconnect.OnButtonPressed += OnDisconnectButtonPressed;
        }

        void OnBackToGameButtonPressed()
        {
            _effect.Play();
            GameClient.CurrentScreen.IsActive = true;
        }

        void OnOptionsButtonPressed()
        {
            _effect.Play();
        }

        void OnDisconnectButtonPressed()
        {
            _effect.Play();
            SetScreenAndDisposePreviousScreen(new ServerListScreen(GameClient));
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Bug: No response
            _buttonBackToGame.Update(Mouse.GetState());
            _buttonOptions.Update(Mouse.GetState());
            _buttonDisconnect.Update(Mouse.GetState());
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            // Backgroung
            Texture2D text = new Texture2D(GameClient.GraphicsDevice, 1, 1);
            text.SetData(new[] { new Color(0, 0, 0, 170)});
            spriteBatch.Draw(text, _screenRectangle, Color.White);

            // Bug: No response
            // Buttons
            _buttonBackToGame.Draw(spriteBatch);
            _buttonOptions.Draw(spriteBatch);
            _buttonDisconnect.Draw(spriteBatch);
        }
    }
}
