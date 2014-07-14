using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MineLib.GraphicClient.GUIButtons;

namespace MineLib.GraphicClient.Screens
{
    sealed class ServerListScreen : Screen
    {
        #region Resources

        Texture2D _mainMenuTexture;
        Texture2D _gradientUpTexture;
        Texture2D _gradientDownTexture;
        SoundEffect _effect;

        #endregion

        string ServerIP = "127.0.0.1";
        short ServerPort = 25565;

        public ServerListScreen(GameClient gameClient)
        {
            GameClient = gameClient;
            Name = "ServerListScreen";
        }

        public override void LoadContent()
        {
            //_mainMenuTexture = Content.Load<Texture2D>("MainMenu");
            _mainMenuTexture = MinecraftTexturesStorage.GUITextures.OptionsBackground;
            _gradientUpTexture = CreateGradientUp();
            _gradientDownTexture = CreateGradientDown();
            _effect = Content.Load<SoundEffect>("Button.Effect");

            //AddGUIButton("Connect", GUIButtonPosition.LeftBottom2, OnConnectButtonPressed);
            AddButtonNavigation("Connect", ButtonNavigationPosition.LeftTop, OnConnectButtonPressed);
            //AddGUIButton("Refresh", GUIButtonPosition.Bottom2, OnRefreshButtonPressed);
            AddButtonNavigation("Refresh", ButtonNavigationPosition.Top, OnRefreshButtonPressed);
            //AddGUIButton("Direct Connection", GUIButtonPosition.RightBottom2, OnDirectConnectionButtonPressed);
            AddButtonNavigation("Direct Connection", ButtonNavigationPosition.RightTop, OnDirectConnectionButtonPressed);

            //AddGUIButton("Add Server", GUIButtonPosition.LeftBottom, OnAddServerButtonPressed);
            AddButtonNavigation("Add Server", ButtonNavigationPosition.LeftBottom, OnAddServerButtonPressed);
            //AddGUIButton("Edit Server", GUIButtonPosition.Bottom, OnEditServerButtonPressed);
            AddButtonNavigation("Edit Server", ButtonNavigationPosition.Bottom, OnEditServerButtonPressed);
            //AddGUIButton("Return", GUIButtonPosition.RightBottom, OnReturnButtonPressed);
            AddButtonNavigation("Return", ButtonNavigationPosition.RightBottom, OnReturnButtonPressed);
        }

        public override void UnloadContent()
        {
            // Unload content only if we are in game
            if (ScreenManager.GetScreen("GameScreen") != null)
                ScreenManager.Content.Unload();
        }


        void OnConnectButtonPressed()
        {
            _effect.Play();

            GameScreen gameScreen = new GameScreen(GameClient, GameClient.Login, GameClient.Password, GameClient.OnlineMode);
            bool status = gameScreen.Connect(ServerIP, ServerPort);
            AddScreenAndExit(status ? (Screen)gameScreen : new ServerListScreen(GameClient));
        }

        void OnRefreshButtonPressed()
        {
            _effect.Play();
        }

        void OnDirectConnectionButtonPressed()
        {
            _effect.Play();
            AddScreenAndExit(new DirectConnectionScreen(GameClient));
        }


        void OnAddServerButtonPressed()
        {
            _effect.Play();
            AddScreenAndExit(new AddServerScreen(GameClient));
        }

        void OnEditServerButtonPressed()
        {
            _effect.Play();
            AddScreenAndExit(new EditServerScreen(GameClient));
        }

        void OnReturnButtonPressed()
        {
            _effect.Play();
            AddScreenAndExit(new MainMenuScreen(GameClient));
        }

        public override void HandleInput(InputState input)
        {
            if (input.IsOncePressed(Keys.Escape))
                AddScreenAndExit(new MainMenuScreen(GameClient));
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointWrap, null, null);

            // Background
            SpriteBatch.Draw(_mainMenuTexture, ScreenRectangle, Color.White);

            SpriteBatch.Draw(_mainMenuTexture, Vector2.Zero, ScreenRectangle, MainBackgroundColor, 0.0f, Vector2.Zero, 4.0f,
                SpriteEffects.None, 0f);

            Rectangle gradientUp = new Rectangle(0, 0, ScreenRectangle.Width, 8);
            SpriteBatch.Draw(_gradientUpTexture, new Vector2(0, 63), gradientUp,
                Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0f);

            Rectangle backgroundUp = new Rectangle(0, 0, ScreenRectangle.Width, 16);
            SpriteBatch.Draw(_mainMenuTexture, Vector2.Zero, backgroundUp, 
                SecondaryBackgroundColor, 0.0f,  Vector2.Zero, 4.0f, SpriteEffects.None, 0f);

            Rectangle gradientDown = new Rectangle(0, 0, ScreenRectangle.Width, 8);
            SpriteBatch.Draw(_gradientDownTexture, new Vector2(0, ScreenRectangle.Height - 128 - 8), gradientDown,
                Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0f);

            Rectangle backgroundDown = new Rectangle(0, 0, ScreenRectangle.Width, 32);
            SpriteBatch.Draw(_mainMenuTexture, new Vector2(0, ScreenRectangle.Height - 128), backgroundDown, 
                SecondaryBackgroundColor, 0.0f, Vector2.Zero, 4.0f, SpriteEffects.None, 0f);

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

        private Texture2D CreateBG1()
        {
            Texture2D backgroundTex = new Texture2D(GraphicsDevice, ScreenRectangle.Width, ScreenRectangle.Height);
            Color[] bgc = new Color[ScreenRectangle.Width * ScreenRectangle.Height];
            int texColour = 0;          // Defines the colour of the gradient.
            int gradientThickness = 16;  // Defines how "diluted" the gradient gets. I've found 2 works great, and 16 is a very fine gradient.

            for (int i = 0; i < bgc.Length; i++)
            {
                texColour = (i / (ScreenRectangle.Height * gradientThickness));
                bgc[i] = new Color(texColour, texColour, texColour, 0);
            }
            backgroundTex.SetData(bgc);
            return backgroundTex;
        }
    }
}
