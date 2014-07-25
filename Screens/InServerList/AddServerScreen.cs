using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MineLib.GraphicClient.GUIItems.Button;
using MineLib.GraphicClient.GUIItems.InputBox;
using MineLib.GraphicClient.Misc;

namespace MineLib.GraphicClient.Screens
{
    sealed class AddServerScreen : InServerListScreen
    {
        ButtonMenu AddButton;
        InputBoxMenu ServerNameInputBox;
        InputBoxMenu ServerAddressInputBox;

        public AddServerScreen(GameClient gameClient)
        {
            GameClient = gameClient;
            Name = "AddServerScreen";
        }

        public override void LoadContent()
        {
            AddButton = AddButtonMenu("Add", ButtonMenuPosition.Bottom2, OnAddButtonPressed);
            AddButton.ToNonPressable();
            AddButtonMenu("Cancel", ButtonMenuPosition.Bottom, OnReturnButtonPressed);

            ServerNameInputBox   = AddInputBoxMenu(InputBoxMenuPosition.Top4);
            ServerAddressInputBox = AddInputBoxMenu(InputBoxMenuPosition.Bottom4);
        }


        void OnAddButtonPressed()
        {
            Server server = new Server
            {
                Name = ServerNameInputBox.InputBoxText,
                Address = StringToAddress(ServerAddressInputBox.InputBoxText)
            };

            AddServerAndSaveServerList(server);

            ButtonEffect.Play();
            AddScreenAndExit(new ServerListScreen(GameClient));
        }

        void OnReturnButtonPressed()
        {
            ButtonEffect.Play();
            AddScreenAndExit(new ServerListScreen(GameClient));
        }


        public override void Update(GameTime gameTime)
        {
            if (ServerAddressInputBox.InputBoxText.Length <= 0)
                AddButton.ToNonPressable();
            else
                AddButton.ToActive();
        }

        public override void HandleInput(InputManager input)
        {
            if (input.IsOncePressed(Keys.Escape))
                AddScreenAndExit(new ServerListScreen(GameClient));

            if (input.IsOncePressed(Keys.Tab))
            {
                if (ServerNameInputBox.IsSelected)
                {
                    ServerNameInputBox.ToUnfocused();
                    ServerAddressInputBox.ToSelected();
                }

                else if (ServerAddressInputBox.IsSelected)
                {
                    ServerAddressInputBox.ToUnfocused();
                    ServerNameInputBox.ToSelected();
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointWrap, null, null);

            // Background
            SpriteBatch.Draw(MainBackgroundTexture, Vector2.Zero, ScreenRectangle, SecondaryBackgroundColor, 0.0f, Vector2.Zero, 4.0f, SpriteEffects.None, 0.5f);

            SpriteBatch.End();
        }
    }
}
