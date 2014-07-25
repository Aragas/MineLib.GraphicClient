using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MineLib.GraphicClient.GUIItems.Button;
using MineLib.GraphicClient.GUIItems.InputBox;
using MineLib.GraphicClient.Misc;

namespace MineLib.GraphicClient.Screens
{
    sealed class EditServerScreen : InServerListScreen
    {
        InputBoxMenu ServerNameInputBox;
        InputBoxMenu ServerAddressInputBox;

        readonly int SelectedServerIndex;

        public EditServerScreen(GameClient gameClient, List<Server> servers, int serverIndex)
        {
            GameClient = gameClient;
            Name = "EditServerScreen";
            Servers = servers;
            SelectedServerIndex = serverIndex;
        }

        public override void LoadContent()
        {
            AddButtonMenu("Delete Server", ButtonMenuPosition.Bottom3, OnDeleteServerButtonPressed);
            AddButtonMenu("Save Server", ButtonMenuPosition.Bottom2, OnDoneButtonPressed);
            AddButtonMenu("Cancel", ButtonMenuPosition.Bottom, OnReturnButtonPressed);

            ServerNameInputBox = AddInputBoxMenu(InputBoxMenuPosition.Top4);
            ServerNameInputBox.InputBoxText = Servers[SelectedServerIndex].Name;
            ServerAddressInputBox = AddInputBoxMenu(InputBoxMenuPosition.Bottom4);
            ServerAddressInputBox.InputBoxText = string.Format("{0}:{1}", Servers[SelectedServerIndex].Address.IP, Servers[SelectedServerIndex].Address.Port);
        }


        void OnDeleteServerButtonPressed()
        {
            Servers.Remove(Servers[SelectedServerIndex]);
            SaveServerList(Servers);

            ButtonEffect.Play();
            AddScreenAndExit(new ServerListScreen(GameClient));
        }
        
        void OnDoneButtonPressed()
        {
            Servers[SelectedServerIndex] = new Server
            {
                Name = ServerNameInputBox.InputBoxText,
                Address = StringToAddress(ServerAddressInputBox.InputBoxText)
            };

            SaveServerList(Servers);

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
            if (input.IsOncePressed(Keys.Escape))
                AddScreenAndExit(new ServerListScreen(GameClient));
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
