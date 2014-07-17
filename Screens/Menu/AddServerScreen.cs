using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MineLib.GraphicClient.GUIItems.Buttons;
using MineLib.GraphicClient.GUIItems.InputBox;


namespace MineLib.GraphicClient.Screens
{
    sealed class AddServerScreen : Screen
    {
        #region Resources

        Texture2D _mainMenuTexture;
        SoundEffect _effect;

        #endregion

        ButtonMenu AddButton;
        InputBoxMenu ServerNameInputBox;
        InputBoxMenu ServerAdressInputBox;

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

            AddButton = AddButtonMenu("Add", ButtonMenuPosition.Bottom2, OnAddButtonPressed);
            AddButton.ToNonPressable();
            AddButtonMenu("Return", ButtonMenuPosition.Bottom, OnReturnButtonPressed);
            ServerNameInputBox   = AddInputBoxMenu(InputBoxMenuPosition.Top4);
            ServerAdressInputBox = AddInputBoxMenu(InputBoxMenuPosition.Bottom4);
        }

        void OnAddButtonPressed()
        {
            _effect.Play();

            string ServerName = ServerNameInputBox.InputBoxText;
            string ServerAddress = ServerAdressInputBox.InputBoxText;

            //_client.CurrentScreen = new ServerListScreen(_client);
        }

        void OnReturnButtonPressed()
        {
            _effect.Play();
            AddScreenAndExit(new ServerListScreen(GameClient));
        }

        public override void Update(GameTime gameTime)
        {
            if (ServerAdressInputBox.InputBoxText.Length <= 0)
                AddButton.ToNonPressable();
            else
                AddButton.ToActive();
        }

        public override void HandleInput(InputState input)
        {
            if (input.IsOncePressed(Keys.Escape))
                AddScreenAndExit(new ServerListScreen(GameClient));

            if (input.IsOncePressed(Keys.Tab))
            {
                if (ServerNameInputBox.IsSelected)
                {
                    ServerNameInputBox.ToUnfocused();
                    ServerAdressInputBox.ToSelected();
                }

                else if (ServerAdressInputBox.IsSelected)
                {
                    ServerAdressInputBox.ToUnfocused();
                    ServerNameInputBox.ToSelected();
                }

            }
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointWrap, null, null);

            // Background
            SpriteBatch.Draw(_mainMenuTexture, Vector2.Zero, ScreenRectangle, SecondaryBackgroundColor, 0.0f,
                Vector2.Zero, 4.0f, SpriteEffects.None, 0.5f);

            SpriteBatch.End();
        }
    }
}
