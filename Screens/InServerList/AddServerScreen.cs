using System.Collections.Generic;
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
        InputBoxMenu ServerNameInputBox;
        InputBoxMenu ServerAddressInputBox;

        public AddServerScreen(GameClient gameClient)
        {
            GameClient = gameClient;
            Name = "AddServerScreen";
        }

        public override void LoadContent()
        {
            GUIButton addButton = AddButtonMenu("Add", ButtonMenuPosition.Bottom2, OnAddButtonPressed);
            AddButtonMenu("Cancel", ButtonMenuPosition.Bottom, OnReturnButtonPressed);

            ServerNameInputBox = AddInputBoxMenu(InputBoxMenuPosition.Top4, new List<GUIButton>{addButton});
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


        public override void HandleInput(InputManager input)
        {
            if (input.IsOncePressed(Keys.Escape) ||
                (input.IsOncePressed(Buttons.B) && input.CurrentGamePadState.IsButtonUp(Buttons.LeftTrigger) && input.CurrentGamePadState.ThumbSticks.Left == Vector2.Zero))
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
