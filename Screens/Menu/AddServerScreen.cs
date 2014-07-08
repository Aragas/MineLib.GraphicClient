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

        GUIButton _buttonAdd;
        GUIButton _buttonReturn;

        SoundEffect _effect;
        #endregion

        public AddServerScreen(GameClient gameClient)
        {
            GameClient = gameClient;
            Name = "AddServerScreen";
        }

        public override void LoadContent()
        {
            Texture2D widgetsTexture = MinecraftTexturesStorage.GUITextures.Widgets;

            _mainMenuTexture = Content.Load<Texture2D>("MainMenu");
            _effect = Content.Load<SoundEffect>("Button.Effect");
            SpriteFont buttonFont = Content.Load<SpriteFont>("VolterGoldfish");

            _buttonAdd = new Button(widgetsTexture, buttonFont, "Add", ScreenRectangle, ButtonEnum.Bottom2);
            _buttonAdd.OnButtonPressed += OnAddButtonPressed;
            _buttonReturn = new Button(widgetsTexture, buttonFont, "Return", ScreenRectangle, ButtonEnum.Bottom);
            _buttonReturn.OnButtonPressed += OnReturnButtonPressed;
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
            if (input.IsNewKeyPress(Keys.Escape))
                AddScreenAndExit(new ServerListScreen(GameClient));
        }

        public override void Update(GameTime gameTime)
        {
            _buttonAdd.Update(gameTime);
            _buttonReturn.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointClamp,
                DepthStencilState.None, RasterizerState.CullNone);

            // Background
            SpriteBatch.Draw(_mainMenuTexture, ScreenRectangle, Color.White);

            // Buttons
            _buttonAdd.Draw(ScreenManager.SpriteBatch);
            _buttonReturn.Draw(ScreenManager.SpriteBatch);

            SpriteBatch.End();
        }
    }
}
