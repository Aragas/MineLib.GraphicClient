﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using MineLib.GraphicClient.GUIButtons;

namespace MineLib.GraphicClient.Screens
{
    sealed class ServerListScreen : Screen
    {
        #region Resources
        Texture2D _mainMenuTexture;

        GUIButton _buttonConnect;
        GUIButton _buttonRefresh;
        GUIButton _buttonDirectConnection;

        GUIButton _buttonAddServer;
        GUIButton _buttonEditServer;
        GUIButton _buttonReturn;

        SoundEffect _effect;
        #endregion

        string ServerIP = "127.0.0.1";
        short ServerPort = 25565;

        public ServerListScreen(GameClient gameClient)
        {
            GameClient = gameClient;
        }

        public override void LoadContent()
        {
            //base.LoadContent();

            Texture2D widgetsTexture = MinecraftTexturesStorage.GUITextures.Widgets;

            _mainMenuTexture = GameClient.Content.Load<Texture2D>("MainMenu");
            _effect = GameClient.Content.Load<SoundEffect>("Button.Effect");
            SpriteFont buttonFont = GameClient.Content.Load<SpriteFont>("VolterGoldfish");

            _buttonConnect = new Button(widgetsTexture, buttonFont, "Connect", ScreenRectangle, ButtonEnum.LeftBottom2);
            _buttonConnect.OnButtonPressed += OnConnectButtonPressed;
            _buttonRefresh = new Button(widgetsTexture, buttonFont, "Refresh", ScreenRectangle, ButtonEnum.Bottom2);
            _buttonRefresh.OnButtonPressed += OnRefreshButtonPressed;
            _buttonDirectConnection = new Button(widgetsTexture, buttonFont, "Direct Connection", ScreenRectangle, ButtonEnum.RightBottom2);
            _buttonDirectConnection.OnButtonPressed += OnDirectConnectionButtonPressed;

            _buttonAddServer = new Button(widgetsTexture, buttonFont, "Add Server", ScreenRectangle, ButtonEnum.LeftBottom);
            _buttonAddServer.OnButtonPressed += OnAddServerButtonPressed;
            _buttonEditServer = new Button(widgetsTexture, buttonFont, "Edit Server", ScreenRectangle, ButtonEnum.Bottom);
            _buttonEditServer.OnButtonPressed += OnEditServerButtonPressed;
            _buttonReturn = new Button(widgetsTexture, buttonFont, "Return", ScreenRectangle, ButtonEnum.RightBottom);
            _buttonReturn.OnButtonPressed += OnReturnButtonPressed;
        }


        void OnConnectButtonPressed()
        {
            _effect.Play();

            GameScreen gameScreen = new GameScreen(GameClient, GameClient.Login, GameClient.Password, GameClient.OnlineMode);
            bool status = gameScreen.Connect(ServerIP, ServerPort);
            AddScreen(!status? (Screen) gameScreen : new ServerListScreen(GameClient));

            ExitScreen();
        }

        void OnRefreshButtonPressed()
        {
            _effect.Play();
        }

        void OnDirectConnectionButtonPressed()
        {
            _effect.Play();
            AddScreen(new DirectConnectionScreen(GameClient));
            ExitScreen();
        }


        void OnAddServerButtonPressed()
        {
            _effect.Play();
            ExitScreen();
            AddScreen(new AddServerScreen(GameClient));
        }

        void OnEditServerButtonPressed()
        {
            _effect.Play();
            ExitScreen();
            AddScreen(new EditServerScreen(GameClient));
        }

        void OnReturnButtonPressed()
        {
            _effect.Play();
            ExitScreen();
            AddScreen(new MainMenuScreen(GameClient));
        }

        public override void HandleInput(InputState input)
        {
            _buttonConnect.HandleInput(input);
            _buttonRefresh.HandleInput(input);
            _buttonDirectConnection.HandleInput(input);

            _buttonAddServer.HandleInput(input);
            _buttonEditServer.HandleInput(input);
            _buttonReturn.HandleInput(input);
        }

        public override void Update(GameTime gameTime)
        {
            //base.Update(gameTime);

            _buttonConnect.Update(gameTime);
            _buttonRefresh.Update(gameTime);
            _buttonDirectConnection.Update(gameTime);

            _buttonAddServer.Update(gameTime);
            _buttonEditServer.Update(gameTime);
            _buttonReturn.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            //base.Draw(gameTime);

            SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointClamp,
                DepthStencilState.None, RasterizerState.CullNone);

            // Background
            SpriteBatch.Draw(_mainMenuTexture, ScreenRectangle, Color.White);

            // Buttons
            _buttonConnect.Draw(SpriteBatch);
            _buttonRefresh.Draw(SpriteBatch);
            _buttonDirectConnection.Draw(SpriteBatch);

            _buttonAddServer.Draw(SpriteBatch);
            _buttonEditServer.Draw(SpriteBatch);
            _buttonReturn.Draw(SpriteBatch);

            SpriteBatch.End();

        }
    }
}
