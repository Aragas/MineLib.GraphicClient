using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MineLib.GraphicClient.GUIItems.Button;
using MineLib.GraphicClient.GUIItems.InputBox;
using MineLib.GraphicClient.Misc;

namespace MineLib.GraphicClient.Screens
{
    sealed class DirectConnectionScreen : InServerListScreen
    {
        InputBoxMenu ServerAddressInputBox;

        public DirectConnectionScreen(GameClient gameClient)
        {
            GameClient = gameClient;
            Name = "DirectConnectionScreen";
        }

        public override void LoadContent()
        {
            GUIButton connectButton = AddButtonMenu("Connect", ButtonMenuPosition.Bottom2, OnConnectButtonPressed);
            AddButtonMenu("Cancel", ButtonMenuPosition.Bottom, OnReturnButtonPressed);

            ServerAddressInputBox = AddInputBoxMenu(InputBoxMenuPosition.Center, new List<GUIButton>{connectButton});
        }

        public override void UnloadContent()
        {
            // Unload content only if we are in game
            if (ScreenManager.GetScreen("GameScreen") != null)
                ScreenManager.Content.Unload();
        }


        void OnConnectButtonPressed()
        {
            Server server = new Server
            {
                Name = "Minecraft Server",
                Address = StringToAddress(ServerAddressInputBox.InputBoxText)
            };

            ButtonEffect.Play();

            GameScreen gameScreen = new GameScreen(GameClient, GameClient.Player, server);
            bool status = gameScreen.Connect();
            AddScreenAndExit(status ? (Screen)gameScreen : new ServerListScreen(GameClient));
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

            if (input.IsOncePressed(Keys.Enter))
                OnConnectButtonPressed();
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
