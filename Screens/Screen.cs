using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MineLib.GraphicClient.GUIButtons;

namespace MineLib.GraphicClient.Screens
{
    public abstract class Screen
    {
        protected GameClient GameClient { get; set; }

        public string Name { get; protected set; }
        public ScreenState ScreenState { get; set; }

        protected MinecraftTexturesStorage MinecraftTexturesStorage { get { return GameClient.MinecraftTexturesStorage; } }
        protected Rectangle ScreenRectangle { get { return GameClient.Window.ClientBounds; } }

        protected ContentManager Content { get { return ScreenManager.Content; } }

        protected GUIButtonManagerComponent GUIButtonManager { get { return GameClient.GuiButtonManager; } }
        protected ScreenManagerComponent ScreenManager { get { return GameClient.ScreenManager; } }
        protected SpriteBatch SpriteBatch { get { return ScreenManager.SpriteBatch; } }
        protected GraphicsDevice GraphicsDevice { get { return GameClient.GraphicsDevice; } }

        protected static Color MainBackgroundColor { get { return new Color(30, 30, 30, 255); } }
        protected static Color SecondaryBackgroundColor { get { return new Color(75, 75, 75, 255); } }

        protected void AddScreen(Screen screen) { ScreenManager.AddScreen(screen); }
        protected void AddScreenAndCloseOthers(Screen screen) { GUIButtonManager.Clear(); ScreenManager.CloseOtherScreens(screen); }
        protected void AddScreenAndExit(Screen screen) { GUIButtonManager.Clear(); ScreenManager.AddScreen(screen); ExitScreen(); }
        protected void AddButtonMenu(string name, ButtonMenuPosition pos, Action action)
        {
            ButtonMenu button = new ButtonMenu(GameClient, name, pos);
            button.OnButtonPressed += action;
            GUIButtonManager.AddButton(button);
        }
        protected void AddButtonNavigation(string name, ButtonNavigationPosition pos, Action action)
        {
            ButtonNavigation button = new ButtonNavigation(GameClient, name, pos);
            button.OnButtonPressed += action;
            GUIButtonManager.AddButton(button);
        }

        public virtual void LoadContent() { }
        public virtual void UnloadContent() { }
        public virtual void HandleInput(InputState input) { }
        public virtual void Update(GameTime gameTime) { }
        public virtual void Draw(GameTime gameTime) { }
        public virtual void Dispose() { }

        public void ToActive() { ScreenState = ScreenState.JustNowActive;}
        public void ToBackground() { ScreenState = ScreenState.Background;}
        public void ToHidden() { ScreenState = ScreenState.Hidden;}

        protected void Exit() { GameClient.Exit(); }

        protected void ExitScreenAndClearButtons()
        {
            GUIButtonManager.Clear();
            Dispose();
            ExitScreen();
        }

        protected virtual void ExitScreen()
        {
            // If the screen has a zero transition time, remove it immediately.
            ScreenManager.RemoveScreen(this);
        }
    }


}
