﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MineLib.GraphicClient.GUIItems.Button;
using MineLib.GraphicClient.Misc;

namespace MineLib.GraphicClient.Screens
{
    sealed class GameOptionScreen : InGameScreen
    {

        #region Resources

        Texture2D _backgroundTexture;
        SoundEffect _buttonEffect;

        #endregion

        public GameOptionScreen(GameClient client)
        {
            GameClient = client;
            Name = "GameOptionScreen";
        }

        public override void LoadContent()
        {
            ScreenManager.GetScreen("GameScreen").ToBackground();
            ScreenManager.GetScreen("GUIScreen").ToBackground();

            _backgroundTexture = new Texture2D(GraphicsDevice, 1, 1);
            _backgroundTexture.SetData(new[] { new Color(0, 0, 0, 170) });

            _buttonEffect = Content.Load<SoundEffect>("ButtonEffect");

            AddButtonMenu("Back To Game", ButtonMenuPosition.Top3, OnBackToGameButtonPressed);
            AddButtonMenu("Options", ButtonMenuPosition.Bottom4, OnOptionsButtonPressed);
            AddButtonMenu("Disconnect", ButtonMenuPosition.Bottom2, OnDisconnectButtonPressed);
        }

        void OnBackToGameButtonPressed()
        {
            _buttonEffect.Play();
            ExitScreenAndClearButtons();
        }

        void OnOptionsButtonPressed()
        {
            _buttonEffect.Play();
        }

        void OnDisconnectButtonPressed()
        {
            _buttonEffect.Play();
            AddScreenAndCloseOthers(new ServerListScreen(GameClient));
        }

        public override void HandleInput(InputManager input)
        {
            if (input.IsOncePressed(Keys.Escape) || input.IsOncePressed(Buttons.B))
                ExitScreenAndClearButtons();
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointWrap, null, null);

            // Backgroung
            SpriteBatch.Draw(_backgroundTexture, ScreenRectangle, Color.White);

            SpriteBatch.End();
        }

        protected override void ExitScreen()
        {
            ScreenManager.GetScreen("GameScreen").ToActive();
            ScreenManager.GetScreen("GUIScreen").ToActive();

            base.ExitScreen();
        }
    }
}
