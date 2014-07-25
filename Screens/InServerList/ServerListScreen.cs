using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MineLib.GraphicClient.GUIItems.Button;
using MineLib.GraphicClient.Misc;
using MineLib.GraphicClient.Screens.InServerList.ServerEntry;

namespace MineLib.GraphicClient.Screens
{
    sealed class ServerListScreen : InServerListScreen
    {

        #region Resources

        Texture2D _gradientUpTexture;
        Texture2D _gradientDownTexture;

        #endregion

        ButtonNavigation ConnectButton;
        ButtonNavigation EditServerButton;

        ServerEntryDrawer ServerEntryDrawer;
        private int SelectedServerIndex;

        public ServerListScreen(GameClient gameClient)
        {
            GameClient = gameClient;
            Name = "ServerListScreen";
        }

        public override void LoadContent()
        {
            _gradientUpTexture = CreateGradientUp();
            _gradientDownTexture = CreateGradientDown();

            ConnectButton = AddButtonNavigation("Connect", ButtonNavigationPosition.LeftTop, OnConnectButtonPressed);
            ConnectButton.ToNonPressable();
            AddButtonNavigation("Refresh", ButtonNavigationPosition.Top, OnRefreshButtonPressed);
            AddButtonNavigation("Direct Connection", ButtonNavigationPosition.RightTop, OnDirectConnectionButtonPressed);

            AddButtonNavigation("Add Server", ButtonNavigationPosition.LeftBottom, OnAddServerButtonPressed);
            EditServerButton = AddButtonNavigation("Edit Server", ButtonNavigationPosition.Bottom, OnEditServerButtonPressed);
            EditServerButton.ToNonPressable();
            AddButtonNavigation("Return", ButtonNavigationPosition.RightBottom, OnReturnButtonPressed);

            // TODO: Better improve dat shiet
            LoadServerList();
            ParseServerEntries();

            ServerEntryDrawer = new ServerEntryDrawer(this, Servers);
            ServerEntryDrawer.LoadContent();
            ServerEntryDrawer.OnClickedPressed += OnClickedServerEntry;
        }

        public override void UnloadContent()
        {
            SaveServerList(Servers);

            // Unload content only if we are in game
            if (ScreenManager.GetScreen("GameScreen") != null)
                ScreenManager.Content.Unload();
        }


        void OnConnectButtonPressed()
        {
            ButtonEffect.Play();

            GameScreen gameScreen = new GameScreen(GameClient, GameClient.Player, Servers[SelectedServerIndex]);
            bool status = gameScreen.Connect();
            AddScreenAndExit(status ? (Screen)gameScreen : new ServerListScreen(GameClient));
        }
        void OnRefreshButtonPressed()
        {
            ButtonEffect.Play();
            ParseServerEntries();
        }
        void OnDirectConnectionButtonPressed()
        {
            ButtonEffect.Play();
            AddScreenAndExit(new DirectConnectionScreen(GameClient));
        }

        void OnAddServerButtonPressed()
        {
            ButtonEffect.Play();
            AddScreenAndExit(new AddServerScreen(GameClient));
        }
        void OnEditServerButtonPressed()
        {
            ButtonEffect.Play();
            AddScreenAndExit(new EditServerScreen(GameClient, Servers, SelectedServerIndex));
        }
        void OnReturnButtonPressed()
        {
            ButtonEffect.Play();
            AddScreenAndExit(new MainMenuScreen(GameClient));
        }

        void OnClickedServerEntry(int index)
        {
            SelectedServerIndex = index;

            ConnectButton.ToActive();
            EditServerButton.ToActive();
        }
        

        public override void HandleInput(InputManager input)
        {
            if (input.IsOncePressed(Keys.Escape))
                AddScreenAndExit(new MainMenuScreen(GameClient));

            ServerEntryDrawer.HandleInput(input);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointWrap, null, null);

            // We can't handle ServerEntryDrawer as a GUIItem, drawing order is important. We draw it in mid-cycle ot this draw call
            ServerEntryDrawer.Draw(gameTime);

            #region Background

            Rectangle gradientUp = new Rectangle(0, 0, ScreenRectangle.Width, 8);
            SpriteBatch.Draw(_gradientUpTexture, new Vector2(0, 63), gradientUp, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0f);

            Rectangle backgroundUp = new Rectangle(0, 0, ScreenRectangle.Width, 16);
            SpriteBatch.Draw(MainBackgroundTexture, Vector2.Zero, backgroundUp, SecondaryBackgroundColor, 0.0f, Vector2.Zero, 4.0f, SpriteEffects.None, 0f);

            Rectangle gradientDown = new Rectangle(0, 0, ScreenRectangle.Width, 8);
            SpriteBatch.Draw(_gradientDownTexture, new Vector2(0, ScreenRectangle.Height - 128 - 8), gradientDown, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0f);

            Rectangle backgroundDown = new Rectangle(0, 0, ScreenRectangle.Width, 32);
            SpriteBatch.Draw(MainBackgroundTexture, new Vector2(0, ScreenRectangle.Height - 128), backgroundDown, SecondaryBackgroundColor, 0.0f, Vector2.Zero, 4.0f, SpriteEffects.None, 0f);

            #endregion

            SpriteBatch.End();
        }

        private Texture2D CreateGradientUp()
        {
            Texture2D backgroundTex = new Texture2D(GraphicsDevice, ScreenRectangle.Width, 8);
            Color[] bgc = new Color[ScreenRectangle.Width * 8];

            for (int i = 1; i < bgc.Length + 1; i++)
            {
                if ((float)i / ScreenRectangle.Width > 1f)
                    bgc[i - 1] = new Color(0, 0, 0, 255);

                if ((float)i / ScreenRectangle.Width > 2f)
                    bgc[i - 1] = new Color(0, 0, 0, 225);

                if ((float)i / ScreenRectangle.Width > 3f)
                    bgc[i - 1] = new Color(0, 0, 0, 195);

                if ((float)i / ScreenRectangle.Width > 4f)
                    bgc[i - 1] = new Color(0, 0, 0, 165);

                if ((float)i / ScreenRectangle.Width > 5f)
                    bgc[i - 1] = new Color(0, 0, 0, 135);

                if ((float)i / ScreenRectangle.Width > 6f)
                    bgc[i - 1] = new Color(0, 0, 0, 105);

                if ((float)i / ScreenRectangle.Width > 7f)
                    bgc[i - 1] = new Color(0, 0, 0, 75);

                if ((float)i / ScreenRectangle.Width > 8f)
                    bgc[i - 1] = new Color(0, 0, 0, 45);
            }
            backgroundTex.SetData(bgc);
            return backgroundTex;
        }

        private Texture2D CreateGradientDown()
        {
            Texture2D backgroundTex = new Texture2D(GraphicsDevice, ScreenRectangle.Width, 8);
            Color[] bgc = new Color[ScreenRectangle.Width * 8];

            for (int i = 1; i < bgc.Length + 1; i++)
            {
                if ((float)i / ScreenRectangle.Width > 1f)
                    bgc[i - 1] = new Color(0, 0, 0, 45);

                if ((float)i / ScreenRectangle.Width > 2f)
                    bgc[i - 1] = new Color(0, 0, 0, 75);

                if ((float)i / ScreenRectangle.Width > 3f)
                    bgc[i - 1] = new Color(0, 0, 0, 105);

                if ((float)i / ScreenRectangle.Width > 4f)
                    bgc[i - 1] = new Color(0, 0, 0, 135);

                if ((float)i / ScreenRectangle.Width > 5f)
                    bgc[i - 1] = new Color(0, 0, 0, 165);

                if ((float)i / ScreenRectangle.Width > 6f)
                    bgc[i - 1] = new Color(0, 0, 0, 195);

                if ((float)i / ScreenRectangle.Width > 7f)
                    bgc[i - 1] = new Color(0, 0, 0, 225);

                if ((float)i / ScreenRectangle.Width > 8f)
                    bgc[i - 1] = new Color(0, 0, 0, 255);
            }
            backgroundTex.SetData(bgc);
            return backgroundTex;
        }
    }
}
