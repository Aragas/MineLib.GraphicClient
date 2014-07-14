using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MineLib.GraphicClient.InputBox
{
    class GUIItemManagerComponent : DrawableGameComponent
    {
        #region Fields

        List<GUIItem> guiItems = new List<GUIItem>();
        List<GUIItem> guiItemsToUpdate = new List<GUIItem>();
        List<GUIItem> guiItemsToDraw = new List<GUIItem>();

        // TODO: Maybe make one (global?) input manager
        InputState input = new InputState();

        IGraphicsDeviceService graphicsDeviceService;

        ContentManager content;
        SpriteBatch spriteBatch;
        SpriteFont font;
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

        public GUIItemManagerComponent(Game game)
            : base(game)
        {
            content = new ContentManager(game.Services, "Content");

            graphicsDeviceService = (IGraphicsDeviceService)game.Services.GetService(
                                                        typeof(IGraphicsDeviceService));

            if (graphicsDeviceService == null)
                throw new InvalidOperationException("No graphics device service.");
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            foreach (GUIItem guiItem in guiItems)
            {
                guiItem.LoadContent();
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
            // Unload content belonging to the GUIButton manager.
            content.Unload();

            foreach (GUIItem guiItem in guiItems)
            {
                guiItem.UnloadContent();
            }
        }

        #endregion

        #region Update and Draw

        public override void Update(GameTime gameTime)
        {
            // Read the keyboard, gamepad and mouse.
            input.Update();

            // Make a copy of the master screen list, to avoid confusion if
            // the process of updating one screen adds or removes others
            // (or it happens on another thread)
            guiItemsToUpdate.Clear();

            foreach (GUIItem guiItem in guiItems)
                guiItemsToUpdate.Add(guiItem);

            // Loop as long as there are screens waiting to be updated.
            while (guiItemsToUpdate.Count > 0)
            {
                // Pop the topmost screen off the waiting list.
                GUIItem guiItem = guiItemsToUpdate[guiItemsToUpdate.Count - 1];

                guiItemsToUpdate.RemoveAt(guiItemsToUpdate.Count - 1);

                // Update the screen.
                guiItem.Update(gameTime);

                if (guiItem.GUIItemState == GUIItemState.JustNowActive)
                 {
                     // Skip one HandleInput, now we won't get a ESC button loop
                     guiItem.GUIItemState = GUIItemState.Active;
                     continue;
                 }

                if (guiItem.GUIItemState == GUIItemState.Active)
                    guiItem.HandleInput(input);
                
            }

            // Print debug trace?
            if (traceEnabled)
                TraceScreens();
        }

        void TraceScreens()
        {
            List<string> guiItemNames = new List<string>();

            foreach (GUIItem guiItem in guiItems)
                guiItemNames.Add(guiItem.GetType().Name);

            Debug.WriteLine(string.Join(", ", guiItemNames.ToArray()));
        }

        public override void Draw(GameTime gameTime)
        {
            // Make a copy of the master screen list, to avoid confusion if
            // the process of drawing one screen adds or removes others
            // (or it happens on another thread
            guiItemsToDraw.Clear();

            foreach (GUIItem guiItem in guiItems)
                guiItemsToDraw.Add(guiItem);

            foreach (GUIItem guiItem in guiItemsToDraw)
            {
                if (guiItem.GUIItemState == GUIItemState.Hidden)
                    continue;

                guiItem.Draw(gameTime);
            }
        }

        #endregion

        #region Public Methods

        public void AddGUIItem(GUIItem guiItem)
        {
            // If we have a graphics device, tell the screen to load content.
            if ((graphicsDeviceService != null) &&
                (graphicsDeviceService.GraphicsDevice != null))
            {
                guiItem.LoadContent();
            }

            guiItems.Add(guiItem);
        }

        public void RemoveGUIItem(GUIItem guiItem)
        {
            // If we have a graphics device, tell the screen to unload content.
            if ((graphicsDeviceService != null) &&
                (graphicsDeviceService.GraphicsDevice != null))
            {
                guiItem.UnloadContent();
            }

            guiItems.Remove(guiItem);
            guiItemsToUpdate.Remove(guiItem);
        }

        public GUIItem GetGUIItem(string name)
        {
            foreach (GUIItem guiItem in guiItems)
            {
                if (guiItem.Name == name)
                    return guiItem;
            }
            return null;
        }

        public GUIItem[] GetGUIItems()
        {
            return guiItems.ToArray();
        }

        public void Clear()
        {
            foreach (GUIItem guiItem in guiItems)
            {
                guiItem.UnloadContent();
            }

            guiItems.Clear();
            guiItemsToUpdate.Clear();
        }

        #endregion
    }
}
