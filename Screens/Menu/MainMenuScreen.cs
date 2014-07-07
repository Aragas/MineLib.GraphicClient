using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using MineLib.GraphicClient.GUIButtons;

namespace MineLib.GraphicClient.Screens
{
    public sealed class MainMenuScreen : Screen
    {
        #region Resources
        Texture2D _mainMenuTexture;

        GUIButton _buttonServerList;
        GUIButton _buttonOption;
        GUIButton _buttonExit;

        SoundEffect _effect;
        #endregion

        public MainMenuScreen(GameClient gameClient)
        {
            GameClient = gameClient;
        }

        public override void LoadContent()
        {
            //base.LoadContent();

            Texture2D widgetsTexture = MinecraftTexturesStorage.GUITextures.Widgets;

            // Custom stuff TODO: Take from minecraft
            _mainMenuTexture = GameClient.Content.Load<Texture2D>("MainMenu");
            _effect = GameClient.Content.Load<SoundEffect>("Button.Effect");
            SpriteFont buttonFont = GameClient.Content.Load<SpriteFont>("VolterGoldfish");

            _buttonServerList = new Button(widgetsTexture, buttonFont, "Search Server", ScreenRectangle, ButtonEnum.Top4);
            _buttonServerList.OnButtonPressed += OnServerListButtonPressed;
            _buttonOption = new Button(widgetsTexture, buttonFont, "Options", ScreenRectangle, ButtonEnum.Bottom4);
            _buttonOption.OnButtonPressed += OnOptionButtonPressed;
            _buttonExit = new Button(widgetsTexture, buttonFont, "Exit", ScreenRectangle, ButtonEnum.Bottom3);
            _buttonExit.OnButtonPressed += OnExitButtonPressed;
        }

        void OnServerListButtonPressed()
        {
            _effect.Play();
            AddScreen(new ServerListScreen(GameClient));
            ExitScreen();
        }

        void OnOptionButtonPressed()
        {
            _effect.Play();
            AddScreen(new OptionScreen());
            ExitScreen();
        }

        void OnExitButtonPressed()
        {
            _effect.Play();
            Exit();
        }

        public override void HandleInput(InputState input)
        {
            _buttonServerList.HandleInput(input);
            _buttonOption.HandleInput(input);
            _buttonExit.HandleInput(input);
        }

        public override void Update(GameTime gameTime)
        {
            //base.Update(gameTime);

            _buttonServerList.Update(gameTime);
            _buttonOption.Update(gameTime);
            _buttonExit.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            //base.Draw(gameTime);

            SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointClamp,
                DepthStencilState.None, RasterizerState.CullNone);

            // Background
            SpriteBatch.Draw(_mainMenuTexture, ScreenRectangle, Color.White);

            // Buttons
            _buttonServerList.Draw(SpriteBatch);
            _buttonOption.Draw(SpriteBatch);
            _buttonExit.Draw(SpriteBatch);

            SpriteBatch.End();

        }
    }
}
