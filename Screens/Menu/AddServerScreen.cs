using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MineLib.GraphicClient.GUIButtons;

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
            _mainMenuTexture = Content.Load<Texture2D>("MainMenu");
            _effect = Content.Load<SoundEffect>("Button.Effect");

            AddGUIButton("Add", GUIButtonNormalPos.Bottom2, OnAddButtonPressed);
            AddGUIButton("Return", GUIButtonNormalPos.Bottom, OnReturnButtonPressed);
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
            SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointClamp,
                DepthStencilState.None, RasterizerState.CullNone);

            // Background
            SpriteBatch.Draw(_mainMenuTexture, ScreenRectangle, Color.White);

            SpriteBatch.End();
        }
    }
}
