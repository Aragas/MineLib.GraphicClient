using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MineLib.GraphicClient.GUIButtons
{
    public enum GUIButtonState
    {
        Active,
        NonPressable,
        Hidden,
        JustNowActive
    }

    public class GUIButtonManagerComponent : DrawableGameComponent
    {
        #region Fields

        List<GUIButton> buttons = new List<GUIButton>();
        List<GUIButton> buttonsToUpdate = new List<GUIButton>();
        List<GUIButton> buttonsToDraw = new List<GUIButton>();

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

        public GUIButtonManagerComponent(Game game)
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

            foreach (GUIButton button in buttons)
            {
                button.LoadContent();
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

            foreach (GUIButton button in buttons)
            {
                button.UnloadContent();
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
            buttonsToUpdate.Clear();

            foreach (GUIButton button in buttons)
                buttonsToUpdate.Add(button);

            // Loop as long as there are screens waiting to be updated.
            while (buttonsToUpdate.Count > 0)
            {
                // Pop the topmost screen off the waiting list.
                GUIButton button = buttonsToUpdate[buttonsToUpdate.Count - 1];

                buttonsToUpdate.RemoveAt(buttonsToUpdate.Count - 1);

                // Update the screen.
                 button.Update(gameTime);

                 if (button.GUIButtonState == GUIButtonState.NonPressable)
                 {
                     continue;
                 }

                 if (button.GUIButtonState == GUIButtonState.JustNowActive)
                 {
                     // Skip one HandleInput, now we won't get a ESC button loop
                     button.GUIButtonState = GUIButtonState.Active;
                     continue;
                 }

                 if (button.GUIButtonState == GUIButtonState.Active)
                    button.HandleInput(input);
                
            }

            // Print debug trace?
            if (traceEnabled)
                TraceScreens();
        }

        void TraceScreens()
        {
            List<string> buttonNames = new List<string>();

            foreach (GUIButton button in buttons)
                buttonNames.Add(button.GetType().Name);

            Debug.WriteLine(string.Join(", ", buttonNames.ToArray()));
        }

        public override void Draw(GameTime gameTime)
        {
            // Make a copy of the master screen list, to avoid confusion if
            // the process of drawing one screen adds or removes others
            // (or it happens on another thread
            buttonsToDraw.Clear();

            foreach (GUIButton button in buttons)
                buttonsToDraw.Add(button);

            foreach (GUIButton button in buttonsToDraw)
            {
                if (button.GUIButtonState == GUIButtonState.Hidden)
                    continue;

                button.Draw(gameTime);
            }
        }

        #endregion

        #region Public Methods

        public void AddButton(GUIButton button)
        {
            // If we have a graphics device, tell the screen to load content.
            if ((graphicsDeviceService != null) &&
                (graphicsDeviceService.GraphicsDevice != null))
            {
                button.LoadContent();
            }

            buttons.Add(button);
        }

        public void RemoveButton(GUIButton button)
        {
            // If we have a graphics device, tell the screen to unload content.
            if ((graphicsDeviceService != null) &&
                (graphicsDeviceService.GraphicsDevice != null))
            {
                button.UnloadContent();
            }

            buttons.Remove(button);
            buttonsToUpdate.Remove(button);
        }

        public GUIButton GetButton(string name)
        {
            foreach (GUIButton button in buttons)
            {
                if (button.Name == name)
                    return button;
            }
            return null;
        }

        public GUIButton[] GetButtons()
        {
            return buttons.ToArray();
        }

        public void Clear()
        {
            foreach (GUIButton button in buttons)
            {
                button.UnloadContent();
            }

            buttons.Clear();
            buttonsToUpdate.Clear();
        }

        #endregion

    }
}
