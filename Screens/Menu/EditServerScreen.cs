using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MineLib.GraphicClient.GUIButtons;

namespace MineLib.GraphicClient.Screens
{
    sealed class EditServerScreen : Screen
    {
        #region Resources
        Texture2D _mainMenuTexture;

        ButtonNormal _buttonConnect;
        ButtonNormal _buttonAdd;
        ButtonNormal _buttonDirectConnection;

        ButtonNormal _buttonAddServer;
        ButtonNormal _buttonEditServer;
        ButtonNormal _buttonReturn;

        SoundEffect _effect;
        #endregion

        public EditServerScreen(GameClient gameClient)
        {
            GameClient = gameClient;
        }

        public override void LoadContent()
        {
            base.LoadContent();

            Texture2D widgetsTexture = MinecraftTexturesStorage.GUITextures.Widgets;

            _mainMenuTexture = GameClient.Content.Load<Texture2D>("MainMenu");
            _effect = GameClient.Content.Load<SoundEffect>("Button.Effect");
            SpriteFont buttonFont = GameClient.Content.Load<SpriteFont>("VolterGoldfish");

            _buttonAdd = new ButtonNormal(widgetsTexture, buttonFont, "Add", ScreenRectangle, ButtonNormalPosition.Bottom2);
            _buttonAdd.OnButtonPressed += OnAddButtonPressed;
            _buttonReturn = new ButtonNormal(widgetsTexture, buttonFont, "Return", ScreenRectangle, ButtonNormalPosition.Bottom);
            _buttonReturn.OnButtonPressed += OnReturnButtonPressed;
        }

        void OnAddButtonPressed()
        {
            _effect.Play();
            SetScreen(new ServerListScreen(GameClient));
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
            spriteBatch.Draw(_mainMenuTexture, ScreenRectangle, Color.White);

            // Buttons
            _buttonAdd.Draw(spriteBatch);
            _buttonReturn.Draw(spriteBatch);
        }
    }
}
