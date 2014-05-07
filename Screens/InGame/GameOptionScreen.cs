using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using MineLib.GraphicClient.GUIButtons;

namespace MineLib.GraphicClient.Screens
{
    sealed class GameOptionScreen : InGameScreen
    {
        #region Resources
        Texture2D _backgroundTexture;

        GUIButton _buttonBackToGame;
        GUIButton _buttonOptions;
        GUIButton _buttonDisconnect;

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

            _buttonBackToGame = new Button(widgetsTexture, buttonFont, "Back To Game", ScreenRectangle, ButtonEnum.Top3);
            _buttonBackToGame.OnButtonPressed += OnBackToGameButtonPressed;

            _buttonOptions = new Button(widgetsTexture, buttonFont, "Options", ScreenRectangle, ButtonEnum.Bottom4);
            _buttonOptions.OnButtonPressed += OnOptionsButtonPressed;

            _buttonDisconnect = new Button(widgetsTexture, buttonFont, "Disconnect", ScreenRectangle, ButtonEnum.Bottom2);
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

            //if(gameTime.IsRunningSlowly)
            //    throw new Exception("");

            // Bug: No response
            _buttonBackToGame.Update(gameTime);
            _buttonOptions.Update(gameTime);
            _buttonDisconnect.Update(gameTime);
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
