using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MineLib.GraphicClient.GUIButtons;

namespace MineLib.GraphicClient.Screens
{
    sealed class GameOptionScreen : InGameScreen
    {
        #region Resources
        Texture2D _backgroundTexture;

        ButtonNormal _buttonBackToGame;
        ButtonNormal _buttonOptions;
        ButtonNormal _buttonDisconnect;

        SoundEffect _effect;
        #endregion

        public GameOptionScreen(GameClient client)
        {
            GameClient = client;
        }

        public override void LoadContent()
        {
            _backgroundTexture = new Texture2D(GameClient.GraphicsDevice, 1, 1);
            _backgroundTexture.SetData(new[] { new Color(0, 0, 0, 170) });

            Texture2D widgetsTexture = MinecraftTexturesStorage.GUITextures.Widgets;

            _effect = GameClient.Content.Load<SoundEffect>("Button.Effect");
            SpriteFont buttonFont = GameClient.Content.Load<SpriteFont>("VolterGoldfish");

            _buttonBackToGame = new ButtonNormal(widgetsTexture, buttonFont, "Back To Game", ScreenRectangle, ButtonNormalPosition.Top3);
            _buttonBackToGame.OnButtonPressed += OnBackToGameButtonPressed;

            _buttonOptions = new ButtonNormal(widgetsTexture, buttonFont, "Options", ScreenRectangle, ButtonNormalPosition.Bottom4);
            _buttonOptions.OnButtonPressed += OnOptionsButtonPressed;

            _buttonDisconnect = new ButtonNormal(widgetsTexture, buttonFont, "Disconnect", ScreenRectangle, ButtonNormalPosition.Bottom2);
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
            spriteBatch.Draw(_backgroundTexture, ScreenRectangle, Color.White);

            // Bug: No response
            // Buttons
            _buttonBackToGame.Draw(spriteBatch);
            _buttonOptions.Draw(spriteBatch);
            _buttonDisconnect.Draw(spriteBatch);
        }
    }
}
