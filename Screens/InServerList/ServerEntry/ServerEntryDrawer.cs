using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MineLib.GraphicClient.GUIItems.Button;
using MineLib.GraphicClient.Misc;

namespace MineLib.GraphicClient.Screens.InServerList.ServerEntry
{
    class ServerEntryDrawer
    {
        public delegate void ServerEntryEventHandler(int index);
        public event ServerEntryEventHandler OnClickedPressed;

        Action OnEntryClicked;

        readonly InServerListScreen _screen;

        #region Constants

        const int TOP_BACKGROUND = 64;
        const int TOP_BACKGROUND_GRADIENT = 8;

        const int BOTTOM_BACKGROUND = 128;
        const int BOTTOM_BACKGROUND_GRADIENT = 8;

        const int SLIDER_WIDTH = 16;

        const int SPACE_BETWEEN_ELEMENTS = 6;

        const int SECOND_LINE = 32;

        const int SPACE_FOR_DATA = 200;

        #endregion

        Vector2 ServerEntrySize = new Vector2(700, 76);
        //Vector2 ServerEntrySize = new Vector2(608, 72); // Vanilla settings
        Vector2 FrameSize = new Vector2(2);

        #region Resources

        int CurrentProtocol { get { return _screen.NetworkProtocol; } }

        MinecraftTexturesStorage MinecraftTexturesStorage { get { return _screen.MinecraftTexturesStorage; } }
        ContentManager Content { get { return _screen.Content; } }
        SpriteBatch SpriteBatch { get { return _screen.SpriteBatch; } }
        GraphicsDevice GraphicsDevice { get { return _screen.GraphicsDevice; } }
        Rectangle ScreenRectangle { get { return _screen.ScreenRectangle; } }

        Color MainBackgroundColor { get { return Screen.MainBackgroundColor; } }

        List<Server> Servers { get; set; }

        bool[] ServerSelected { get; set; }

        Rectangle[] ServerEntryRectangles { get; set; }

        Texture2D[] ServerEntryImageTextures { get; set; }
        Rectangle[] ServerEntryImageRectangles { get; set; }

        Rectangle[] WhiteFrameTopRectangles { get; set; }
        Rectangle[] WhiteFrameBottomRectangles { get; set; }
        Rectangle[] WhiteFrameLeftRectangles { get; set; }
        Rectangle[] WhiteFrameRightRectangles { get; set; }

        Rectangle SliderRectangle { get; set; }
        Rectangle SliderSelectedRectangle { get; set; }

        Rectangle AvailableScreenRectangle { get; set; }

        Vector2[] ServerNameVectors { get; set; }
        Vector2[] ServerPlayersVectors { get; set; }
        Vector2[] ServerAddressVectors { get; set; }
        Vector2[] ServerPingVectors { get; set; }

        Vector2 BackgroundVector { get; set; }

        Texture2D WhiteFrameTexture { get; set; }
        Texture2D ServerEntryImageTexture { get; set; }
        Texture2D BlackTexture { get; set; }
        Texture2D BackgroundTexture { get; set; }

        float TextScale { get; set; }

        SpriteFont TextFont { get { return Content.Load<SpriteFont>("Minecraftia"); } }

        #endregion

        public ServerEntryDrawer(InServerListScreen screen, List<Server> server)
        {
            _screen = screen;
            Servers = server;

            ServerSelected = new bool[Servers.Count];

            ServerEntryRectangles = new Rectangle[Servers.Count];

            ServerEntryImageTextures = new Texture2D[Servers.Count];
            ServerEntryImageRectangles = new Rectangle[Servers.Count];

            WhiteFrameTopRectangles = new Rectangle[Servers.Count];
            WhiteFrameBottomRectangles = new Rectangle[Servers.Count];
            WhiteFrameLeftRectangles = new Rectangle[Servers.Count];
            WhiteFrameRightRectangles = new Rectangle[Servers.Count];

            ServerNameVectors = new Vector2[Servers.Count];
            ServerPlayersVectors = new Vector2[Servers.Count];
            ServerAddressVectors = new Vector2[Servers.Count];
            ServerPingVectors = new Vector2[Servers.Count];
        }

        /// <summary>
        /// Link GUIButtons to ServerEntryDrawer.
        /// </summary>
        /// <param name="screen">Screen</param>
        /// <param name="server">Server list</param>
        /// <param name="buttons">GUIButtons</param>
        public ServerEntryDrawer(InServerListScreen screen, List<Server> server, IEnumerable<GUIButton> buttons)
        {
            _screen = screen;
            Servers = server;

            ServerSelected = new bool[Servers.Count];

            ServerEntryRectangles = new Rectangle[Servers.Count];

            ServerEntryImageTextures = new Texture2D[Servers.Count];
            ServerEntryImageRectangles = new Rectangle[Servers.Count];

            WhiteFrameTopRectangles = new Rectangle[Servers.Count];
            WhiteFrameBottomRectangles = new Rectangle[Servers.Count];
            WhiteFrameLeftRectangles = new Rectangle[Servers.Count];
            WhiteFrameRightRectangles = new Rectangle[Servers.Count];

            ServerNameVectors = new Vector2[Servers.Count];
            ServerPlayersVectors = new Vector2[Servers.Count];
            ServerAddressVectors = new Vector2[Servers.Count];
            ServerPingVectors = new Vector2[Servers.Count];

            foreach (GUIButton guiButton in buttons)
            {
                guiButton.ToNonPressable();
                OnEntryClicked += guiButton.ToActive;
            }
        }

        public void LoadContent()
        {
            BackgroundTexture = MinecraftTexturesStorage.GUITextures.OptionsBackground;

            ServerEntryImageTexture = new Texture2D(GraphicsDevice, 64, 64);
            ServerEntryImageTexture.SetData(new[] { new Color(255, 255, 255, 255) });

            BlackTexture = new Texture2D(GraphicsDevice, 1, 1);
            BlackTexture.SetData(new[] { new Color(0, 0, 0, 255) });

            WhiteFrameTexture = new Texture2D(GraphicsDevice, 1, 1);
            WhiteFrameTexture.SetData(new[] { Color.LightGray });

            #region First Load

            BackgroundVector = Vector2.Zero;

            for (int i = 0; i < Servers.Count; i++)
            {
                ServerEntryRectangles[i] = GetServerEntryRectangle(i);

                ServerEntryImageRectangles[i] = new Rectangle(ServerEntryRectangles[i].X + SPACE_BETWEEN_ELEMENTS, ServerEntryRectangles[i].Y + SPACE_BETWEEN_ELEMENTS, ServerEntryImageTexture.Width, ServerEntryImageTexture.Height);
                ServerEntryImageRectangles[i] = new Rectangle(ServerEntryRectangles[i].X + SPACE_BETWEEN_ELEMENTS, ServerEntryRectangles[i].Y + SPACE_BETWEEN_ELEMENTS, ServerEntryImageTexture.Width, ServerEntryImageTexture.Height);

                WhiteFrameTopRectangles[i] = new Rectangle(ServerEntryRectangles[i].X, ServerEntryRectangles[i].Y, (int)ServerEntrySize.X, (int)FrameSize.Y);
                WhiteFrameBottomRectangles[i] = new Rectangle(ServerEntryRectangles[i].X, (int)(ServerEntryRectangles[i].Y + ServerEntrySize.Y - FrameSize.Y), (int)ServerEntrySize.X, (int)FrameSize.Y);
                WhiteFrameLeftRectangles[i] = new Rectangle(ServerEntryRectangles[i].X, ServerEntryRectangles[i].Y, (int)FrameSize.X, (int)ServerEntrySize.Y);
                WhiteFrameRightRectangles[i] = new Rectangle((int)(ServerEntryRectangles[i].X + ServerEntrySize.X - FrameSize.X), ServerEntryRectangles[i].Y, (int)FrameSize.X, (int)ServerEntrySize.Y);

                SliderRectangle = new Rectangle(ServerEntryRectangles[0].X + ServerEntryRectangles[0].Width, ServerEntryRectangles[0].Y - TOP_BACKGROUND_GRADIENT, SLIDER_WIDTH, ScreenRectangle.Width - TOP_BACKGROUND - BOTTOM_BACKGROUND);

                ServerNameVectors[i] = new Vector2(ServerEntryImageRectangles[i].X + ServerEntryImageRectangles[i].Width + SPACE_BETWEEN_ELEMENTS, ServerEntryImageRectangles[i].Y);
                ServerPlayersVectors[i] = new Vector2(ServerEntryRectangles[i].X + ServerEntryRectangles[i].Width - SPACE_FOR_DATA, ServerNameVectors[i].Y);
                ServerAddressVectors[i] = new Vector2(ServerNameVectors[i].X, ServerNameVectors[i].Y + SECOND_LINE);
                ServerPingVectors[i] = new Vector2(ServerEntryRectangles[i].X + ServerEntryRectangles[i].Width - SPACE_FOR_DATA, ServerAddressVectors[i].Y);
            }

            #endregion

            AvailableScreenRectangle = new Rectangle(0, TOP_BACKGROUND, ScreenRectangle.Width, ScreenRectangle.Height - TOP_BACKGROUND -  BOTTOM_BACKGROUND);

            TextScale = 0.125f;
        }


        void MenuUp()
        {
            for (int i = ServerSelected.Length - 1; i >= 0; i--)
            {
                if (i == 0)
                    break;

                if (ServerSelected[i])
                {
                    ServerSelected[i] = false;
                    ServerSelected[i - 1] = true;

                    OnClickedPressed(i - 1);
                    break;
                }
            }

            if (BackgroundVector.Y >= 0)
                return;

            BackgroundVector = new Vector2(BackgroundVector.X, BackgroundVector.Y + ServerEntrySize.Y);

            for (int i = 0; i < Servers.Count; i++)
            {
                ServerEntryRectangles[i] = new Rectangle(ServerEntryRectangles[i].X, ServerEntryRectangles[i].Y + (int)ServerEntrySize.Y, (int)ServerEntrySize.X, (int)ServerEntrySize.Y);
            }

            UpdateOthersPositions();
        }
        void MenuDown()
        {
            int visibleY = ScreenRectangle.Height - (64 + 8 + 128 + 8);
            double maxEntryToShow = Math.Floor(visibleY / ServerEntrySize.Y);


            for (int i = 0; i < ServerSelected.Length; i++)
            {
                if (ServerSelected[i])
                {
                    if (i != ServerSelected.Length - 1)
                    {
                        ServerSelected[i] = false;
                        ServerSelected[i + 1] = true;
                        OnClickedPressed(i + 1);
                    }

                    if (ServerEntrySize.Y * maxEntryToShow > ServerEntrySize.Y * (i + 1))
                        return;

                    if (ServerEntrySize.Y * (i + 1) > ServerEntrySize.Y * (ServerSelected.Length - 1))
                        return;

                    break;
                }
            }

            BackgroundVector = new Vector2(BackgroundVector.X, BackgroundVector.Y - ServerEntrySize.Y);

            for (int i = 0; i < Servers.Count; i++)
            {
                ServerEntryRectangles[i] = new Rectangle(ServerEntryRectangles[i].X, ServerEntryRectangles[i].Y - (int)ServerEntrySize.Y, (int)ServerEntrySize.X, (int)ServerEntrySize.Y);
            }

            UpdateOthersPositions();
        }
        
        void MouseScrollUp()
        {
            int mouse = 10;

            if (BackgroundVector.Y >= 0)
                return;

            BackgroundVector = new Vector2(BackgroundVector.X, BackgroundVector.Y + mouse);

            for (int i = 0; i < Servers.Count; i++)
            {
                ServerEntryRectangles[i] = new Rectangle(ServerEntryRectangles[i].X, ServerEntryRectangles[i].Y + mouse, (int)ServerEntrySize.X, (int)ServerEntrySize.Y);
            }

            UpdateOthersPositions();
        }
        void MouseScrollDown()
        {
            if (ServerEntryRectangles[ServerEntryRectangles.Length - 1].Y + ServerEntrySize.Y < ScreenRectangle.Height - (128))
                return;

            int mouse = 10;

            BackgroundVector = new Vector2(BackgroundVector.X, BackgroundVector.Y - mouse);

            for (int i = 0; i < Servers.Count; i++)
            {
                ServerEntryRectangles[i] = new Rectangle(ServerEntryRectangles[i].X, ServerEntryRectangles[i].Y - mouse,
                    (int) ServerEntrySize.X, (int) ServerEntrySize.Y);
            }

            UpdateOthersPositions();
        }

        void UpdateOthersPositions()
        {
            for (int i = 0; i < Servers.Count; i++)
            {
                ServerEntryImageRectangles[i] = new Rectangle(ServerEntryRectangles[i].X + SPACE_BETWEEN_ELEMENTS, ServerEntryRectangles[i].Y + SPACE_BETWEEN_ELEMENTS, ServerEntryImageTexture.Width, ServerEntryImageTexture.Height);
                ServerEntryImageRectangles[i] = new Rectangle(ServerEntryRectangles[i].X + SPACE_BETWEEN_ELEMENTS, ServerEntryRectangles[i].Y + SPACE_BETWEEN_ELEMENTS, ServerEntryImageTexture.Width, ServerEntryImageTexture.Height);

                WhiteFrameTopRectangles[i] = new Rectangle(ServerEntryRectangles[i].X, ServerEntryRectangles[i].Y, (int)ServerEntrySize.X, (int)FrameSize.Y);
                WhiteFrameBottomRectangles[i] = new Rectangle(ServerEntryRectangles[i].X, (int)(ServerEntryRectangles[i].Y + ServerEntrySize.Y - FrameSize.Y), (int)ServerEntrySize.X, (int)FrameSize.Y);
                WhiteFrameLeftRectangles[i] = new Rectangle(ServerEntryRectangles[i].X, ServerEntryRectangles[i].Y, (int)FrameSize.X, (int)ServerEntrySize.Y);
                WhiteFrameRightRectangles[i] = new Rectangle((int)(ServerEntryRectangles[i].X + ServerEntrySize.X - FrameSize.X), ServerEntryRectangles[i].Y, (int)FrameSize.X, (int)ServerEntrySize.Y);

                ServerNameVectors[i] = new Vector2(ServerEntryImageRectangles[i].X + ServerEntryImageRectangles[i].Width + SPACE_BETWEEN_ELEMENTS, ServerEntryImageRectangles[i].Y);
                ServerPlayersVectors[i] = new Vector2(ServerEntryRectangles[i].X + ServerEntryRectangles[i].Width - SPACE_FOR_DATA, ServerNameVectors[i].Y);
                ServerAddressVectors[i] = new Vector2(ServerNameVectors[i].X, ServerNameVectors[i].Y + SECOND_LINE);
                ServerPingVectors[i] = new Vector2(ServerEntryRectangles[i].X + ServerEntryRectangles[i].Width - SPACE_FOR_DATA, ServerAddressVectors[i].Y);
            }
        }

        
        public void HandleInput(InputManager input)
        {

            #region Mouse selection handling

            MouseState mouse = input.CurrentMouseState;

            for (int i = 0; i < ServerEntryRectangles.Length; i++)
            {
                Rectangle mouseRectangle = new Rectangle(mouse.X, mouse.Y, 1, 1);
                if (AvailableScreenRectangle.Intersects(mouseRectangle) && 
                    ServerEntryRectangles[i].Intersects(mouseRectangle) &&
                input.CurrentMouseState.LeftButton == ButtonState.Pressed && input.LastMouseState.LeftButton == ButtonState.Released)
                {
                    // Reinitiate and make others bools false
                    ServerSelected = new bool[Servers.Count];

                    ServerSelected[i] = true;
                    OnClickedPressed(i);

                    if (OnEntryClicked != null)
                        OnEntryClicked();
                }
            }

            #endregion

            #region Keyboard and Gamepad

            if (Array.IndexOf(ServerSelected, true) == -1 && (input.IsOncePressed(Keys.Down) || input.IsOncePressed(Buttons.LeftTrigger, Buttons.DPadDown)))
            {
                ServerSelected[0] = true;
                OnClickedPressed(0);

                if (OnEntryClicked != null)
                    OnEntryClicked();

                return;
            }

            // Server entry was selected
            if (Array.IndexOf(ServerSelected, true) != -1)
            {
                if (input.IsOncePressed(Keys.Up) || input.IsOncePressed(Buttons.LeftTrigger, Buttons.DPadUp))
                    MenuUp();

                if (input.IsOncePressed(Keys.Down) || input.IsOncePressed(Buttons.LeftTrigger, Buttons.DPadDown))
                    MenuDown();
            }

            #endregion

            #region Mouse Scroll

            if (input.MouseScrollUp)
                MouseScrollUp();

            if (input.MouseScrollDown)
                MouseScrollDown();

            #endregion

        }

        public void Draw(GameTime gameTime)
        {
            SpriteBatch.Draw(BackgroundTexture, BackgroundVector, ScreenRectangle, MainBackgroundColor, 0.0f, Vector2.Zero, 4.0f, SpriteEffects.None, 0f);

            SpriteBatch.Draw(BlackTexture, SliderRectangle, Rectangle.Empty, Color.White);

            for (int i = 0; i < Servers.Count; i++)
            {

                #region WhiteFrames

                if (ServerSelected[i])
                {
                    SpriteBatch.Draw(BlackTexture, ServerEntryRectangles[i], Rectangle.Empty, Color.White);

                    SpriteBatch.Draw(WhiteFrameTexture, WhiteFrameTopRectangles[i], Rectangle.Empty, Color.White);
                    SpriteBatch.Draw(WhiteFrameTexture, WhiteFrameBottomRectangles[i], Rectangle.Empty, Color.White);
                    SpriteBatch.Draw(WhiteFrameTexture, WhiteFrameLeftRectangles[i], Rectangle.Empty, Color.White);
                    SpriteBatch.Draw(WhiteFrameTexture, WhiteFrameRightRectangles[i], Rectangle.Empty, Color.White);
                }

                #endregion

                #region Favicon

                // If server have a Favicon, use it.
                if (ServerEntryImageTextures[i] == null && Servers[i].ServerResponse.Info.Favicon != null)
                    ServerEntryImageTextures[i] = Texture2D.FromStream(GraphicsDevice, new MemoryStream(Servers[i].ServerResponse.Info.Favicon));
                
                if (ServerEntryImageTextures[i] != null)
                    SpriteBatch.Draw(ServerEntryImageTextures[i], ServerEntryImageRectangles[i], new Rectangle(0, 0, ServerEntryImageTextures[i].Width, ServerEntryImageTextures[i].Height), Color.White);

                #endregion

                // BUG: string.Format use a lot of memory (0.5 MB in a sec, lol). Qualify as a leak?
                SpriteBatch.DrawString(TextFont, Servers[i].Name, ServerNameVectors[i], Color.White, 0.0f, Vector2.Zero, TextScale, SpriteEffects.None, 0.0f);

                SpriteBatch.DrawString(TextFont, string.Format("{0}/{1}", Servers[i].ServerResponse.Info.Players.Online, Servers[i].ServerResponse.Info.Players.Max), ServerPlayersVectors[i], Color.White, 0.0f, Vector2.Zero, TextScale, SpriteEffects.None, 0.0f);

                if (Servers[i].ServerResponse.Ping == int.MaxValue)
                    SpriteBatch.DrawString(TextFont, string.Format("Ping: {0}", "None"), ServerPingVectors[i], Color.White, 0.0f, Vector2.Zero, TextScale, SpriteEffects.None, 0.0f);
                else
                    SpriteBatch.DrawString(TextFont, string.Format("Ping: {0}", Servers[i].ServerResponse.Ping), ServerPingVectors[i], Color.White, 0.0f, Vector2.Zero, TextScale, SpriteEffects.None, 0.0f);



                if (Servers[i].ServerResponse.Ping == 0 && Servers[i].ServerResponse.Info.Players.Max == 0)
                    SpriteBatch.DrawString(TextFont, "Connecting...", ServerAddressVectors[i], Color.White, 0.0f, Vector2.Zero, TextScale - 0.0125f, SpriteEffects.None, 0.0f);
                else if (Servers[i].ServerResponse.Ping == int.MaxValue)
                    SpriteBatch.DrawString(TextFont, "Can't connect to server.", ServerAddressVectors[i], Color.DarkRed, 0.0f, Vector2.Zero, TextScale - 0.0125f, SpriteEffects.None, 0.0f);
                else if (Servers[i].ServerResponse.Info.Version.Protocol != CurrentProtocol)
                    SpriteBatch.DrawString(TextFont, "Unsupported protocol.", ServerAddressVectors[i], Color.DarkRed, 0.0f, Vector2.Zero, TextScale - 0.0125f, SpriteEffects.None, 0.0f);
                //else
                // BUG: Fuckin' unicode font support. Oh god why. Should i take fonts from minecraft.jar?
                //    SpriteBatch.DrawString(ButtonFont, Servers[i].ServerResponse.Info.Description, ServerAddressVectors[i], Color.DarkRed, 0.0f, Vector2.Zero, TextScale - 0.0125f, SpriteEffects.None, 0.0f);

            }
        }

        Rectangle GetServerEntryRectangle(int index)
        {
            return new Rectangle((int)(ScreenRectangle.Center.X - ServerEntrySize.X * 0.5f), (int)(64 + 8 + ServerEntrySize.Y * index), (int)ServerEntrySize.X, (int)ServerEntrySize.Y);
        }
    }
}
