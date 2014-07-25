using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MineLib.GraphicClient.Misc;

namespace MineLib.GraphicClient.Screens
{
    sealed class OptionScreen : Screen
    {

        #region Resources

        Texture2D _mainBackgroundTexture;

        SoundEffect _buttonEffect;

        #endregion

        public OptionScreen(GameClient gameClient)
        {
            GameClient = gameClient;
            Name = "OptionScreen";
        }

        public override void LoadContent()
        {
            _mainBackgroundTexture = MinecraftTexturesStorage.GUITextures.OptionsBackground;
            _buttonEffect = Content.Load<SoundEffect>("ButtonEffect");
        }

        public override void HandleInput(InputManager input)
        {
            if (input.IsOncePressed(Keys.Escape))
                AddScreenAndExit(new MainMenuScreen(GameClient));
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointWrap, null, null);

            SpriteBatch.Draw(_mainBackgroundTexture, Vector2.Zero, ScreenRectangle, SecondaryBackgroundColor, 0.0f,
                Vector2.Zero, 4.0f, SpriteEffects.None, 0.5f);

            SpriteBatch.End();
        }
    }
}
