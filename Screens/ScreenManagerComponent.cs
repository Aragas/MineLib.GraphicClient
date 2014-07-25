using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MineLib.GraphicClient.Misc;

namespace MineLib.GraphicClient.Screens
{
    public enum ScreenState
    {
        Active,
        Background,
        Hidden,
        JustNowActive
    }

    public class ScreenManagerComponent : DrawableGameComponent
    {
        #region Fields

        List<Screen> screens = new List<Screen>();
        List<Screen> screensToUpdate = new List<Screen>();
        List<Screen> screensToDraw = new List<Screen>();

        InputManager input = new InputManager();

        IGraphicsDeviceService graphicsDeviceService;

        ContentManager content;
        ContentManager gameContent;
        SpriteBatch spriteBatch;
        Rectangle titleSafeArea;

        bool traceEnabled;

        #endregion


        #region Properties

        new public GraphicsDevice GraphicsDevice
        {
            get { return base.GraphicsDevice; }
        }

        public ContentManager Content
        {
            get { return content; }
        }

        public ContentManager GameContent
        {
            get { return gameContent; }
        }

        public SpriteBatch SpriteBatch
        {
            get { return spriteBatch; }
        }

        public bool TraceEnabled
        {
            get { return traceEnabled; }
            set { traceEnabled = value; }
        }

        public Rectangle TitleSafeArea
        {
            get { return titleSafeArea; }
        }

        #endregion


        #region Initialization

        public ScreenManagerComponent(Game game)
            : base(game)
        {
            content = new ContentManager(game.Services, "Content");
            gameContent = new ContentManager(game.Services, "Content");

            graphicsDeviceService = (IGraphicsDeviceService)game.Services.GetService(
                                                        typeof(IGraphicsDeviceService));

            if (graphicsDeviceService == null)
                throw new InvalidOperationException("No graphics device service.");
        }

        protected override void LoadContent()
        {
            // Load content belonging to the screen manager.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Tell each of the screens to load their content.
            foreach (Screen screen in screens)
            {
                screen.LoadContent();
            }

            // update the title-safe area
            titleSafeArea = new Rectangle(
                (int)Math.Floor(GraphicsDevice.Viewport.X +
                   GraphicsDevice.Viewport.Width * 0.05f),
                (int)Math.Floor(GraphicsDevice.Viewport.Y +
                   GraphicsDevice.Viewport.Height * 0.05f),
                (int)Math.Floor(GraphicsDevice.Viewport.Width * 0.9f),
                (int)Math.Floor(GraphicsDevice.Viewport.Height * 0.9f));
        }

        protected override void UnloadContent()
        {
            // Unload content belonging to the screen manager.
            content.Unload();

            // Tell each of the screens to unload their content.
            foreach (Screen screen in screens)
            {
                screen.UnloadContent();
            }
        }

        #endregion


        #region Update and Draw

        public override void Update(GameTime gameTime)
        {
            // Read the keyboard and gamepad.
            input.Update();

            // Make a copy of the master screen list, to avoid confusion if
            // the process of updating one screen adds or removes others
            // (or it happens on another thread)
            screensToUpdate.Clear();

            foreach (Screen screen in screens)
                screensToUpdate.Add(screen);

            // Loop as long as there are screens waiting to be updated.
            while (screensToUpdate.Count > 0)
            {
                // Pop the topmost screen off the waiting list.
                Screen screen = screensToUpdate[screensToUpdate.Count - 1];

                screensToUpdate.RemoveAt(screensToUpdate.Count - 1);

                // Update the screen.
                 screen.Update(gameTime);

                 if (screen.ScreenState == ScreenState.JustNowActive)
                 {
                     // Skip one HandleInput, now we won't get a ESC button loop
                     screen.ScreenState = ScreenState.Active;
                     return;
                 }

                 if (screen.ScreenState == ScreenState.Hidden || 
                     screen.ScreenState == ScreenState.Background)
                   continue;

                screen.HandleInput(input);
                
            }

            // Print debug trace?
            if (traceEnabled)
                TraceScreens();
        }

        void TraceScreens()
        {
            List<string> screenNames = new List<string>();

            foreach (Screen screen in screens)
                screenNames.Add(screen.GetType().Name);

            Debug.WriteLine(string.Join(", ", screenNames.ToArray()));
        }

        public override void Draw(GameTime gameTime)
        {
            // Make a copy of the master screen list, to avoid confusion if
            // the process of drawing one screen adds or removes others
            // (or it happens on another thread
            screensToDraw.Clear();

            foreach (Screen screen in screens)
                screensToDraw.Add(screen);

            foreach (Screen screen in screensToDraw)
            {
                if (screen.ScreenState == ScreenState.Hidden)
                    continue;

                screen.Draw(gameTime);
            }
        }

        #endregion


        #region Public Methods

        public void AddScreen(Screen screen)
        {
            // If we have a graphics device, tell the screen to load content.
            if ((graphicsDeviceService != null) &&
                (graphicsDeviceService.GraphicsDevice != null))
            {
                screen.LoadContent();
            }

            screens.Add(screen);
        }

        public void RemoveScreen(Screen screen)
        {
            // If we have a graphics device, tell the screen to unload content.
            if ((graphicsDeviceService != null) &&
                (graphicsDeviceService.GraphicsDevice != null))
            {
                screen.UnloadContent();
            }

            screens.Remove(screen);
            screensToUpdate.Remove(screen);
        }

        public void CloseOtherScreens(Screen currentScreen)
        {
            foreach (Screen screen in screens)
            {
                // If we have a graphics device, tell the screen to unload content.
                if ((graphicsDeviceService != null) &&
                    (graphicsDeviceService.GraphicsDevice != null))
                {
                    screen.UnloadContent();
                }
            }
            screens.Clear();
            screensToUpdate.Clear();

            AddScreen(currentScreen);
        }

        public Screen GetScreen(string name)
        {
            foreach (Screen screen in screens)
            {
                if (screen.Name == name)
                    return screen;
            }
            return null;
        }

        public Screen[] GetScreens()
        {
            return screens.ToArray();
        }

        #endregion
    }
}
