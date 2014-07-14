using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MineLib.GraphicClient.Screens
{
    sealed class OptionScreen : Screen
    {
        #region Resources

        Texture2D _mainMenuTexture;
        SoundEffect _effect;

        #endregion

        public OptionScreen(GameClient gameClient)
        {
            GameClient = gameClient;
            Name = "OptionScreen";
        }

        public override void LoadContent()
        {
            _mainMenuTexture = MinecraftTexturesStorage.GUITextures.OptionsBackground;
            _effect = Content.Load<SoundEffect>("Button.Effect");

        }

        public override void HandleInput(InputState input)
        {
            if (input.IsOncePressed(Keys.Escape))
                AddScreenAndExit(new MainMenuScreen(GameClient));
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointWrap, null, null);

            SpriteBatch.Draw(_mainMenuTexture, Vector2.Zero, ScreenRectangle, SecondaryBackgroundColor, 0.0f,
                Vector2.Zero, 4.0f, SpriteEffects.None, 0.5f);

            SpriteBatch.End();
        }
    }
}
