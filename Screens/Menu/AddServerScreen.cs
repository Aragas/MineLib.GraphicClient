using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MineLib.GraphicClient.GUIItems.Buttons;


namespace MineLib.GraphicClient.Screens
{
    sealed class AddServerScreen : Screen
    {
        #region Resources

        Texture2D _mainMenuTexture;
        SoundEffect _effect;

        #endregion

        public AddServerScreen(GameClient gameClient)
        {
            GameClient = gameClient;
            Name = "AddServerScreen";
        }

        public override void LoadContent()
        {
            //_mainMenuTexture = Content.Load<Texture2D>("MainMenu");
            _mainMenuTexture = MinecraftTexturesStorage.GUITextures.OptionsBackground;
            _effect = Content.Load<SoundEffect>("Button.Effect");

            AddButtonMenu("Add", ButtonMenuPosition.Bottom2, OnAddButtonPressed);
            AddButtonMenu("Return", ButtonMenuPosition.Bottom, OnReturnButtonPressed);
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
            AddScreenAndExit(new ServerListScreen(GameClient));
        }

        public override void HandleInput(InputState input)
        {
            if (input.IsOncePressed(Keys.Escape))
                AddScreenAndExit(new ServerListScreen(GameClient));
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointWrap, null, null);

            // Background
            //SpriteBatch.Draw(_mainMenuTexture, ScreenRectangle, Color.White);
            SpriteBatch.Draw(_mainMenuTexture, Vector2.Zero, ScreenRectangle, SecondaryBackgroundColor, 0.0f,
                Vector2.Zero, 4.0f, SpriteEffects.None, 0.5f);

            SpriteBatch.End();
        }
    }
}
