using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MineLib.GraphicClient.GUIButtons;

namespace MineLib.GraphicClient.Screens
{
    public abstract class Screen
    {
        public GameClient GameClient { get; set; }

        public string Name { get; set; }

        public MinecraftTexturesStorage MinecraftTexturesStorage { get { return GameClient.MinecraftTexturesStorage; }}
        public Rectangle ScreenRectangle { get { return GameClient.Window.ClientBounds; }}

        public ContentManager Content { get { return ScreenManager.Content; }}

        public GUIButtonManagerComponent GUIButtonManager { get { return GameClient.GuiButtonManager; } }
        public ScreenManagerComponent ScreenManager { get { return GameClient.ScreenManager; } }
        public ScreenState ScreenState { get; set; }
        public SpriteBatch SpriteBatch { get { return ScreenManager.SpriteBatch; }}

        protected void AddScreen(Screen screen) { ScreenManager.AddScreen(screen); }
        protected void AddScreenAndCloseOthers(Screen screen) { GUIButtonManager.Clear(); ScreenManager.CloseOtherScreens(screen); }
        protected void AddScreenAndExit(Screen screen) { GUIButtonManager.Clear(); ScreenManager.AddScreen(screen); ExitScreen(); }
        protected void AddGUIButton(string name, GUIButtonNormalPos pos, Action action)
        {
            Button button = new Button(GameClient, name, pos);
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

        public void Exit() { GameClient.Exit(); }

        public void ExitScreenAndClearButtons()
        {
            GUIButtonManager.Clear();
            ExitScreen();
        }
        public virtual void ExitScreen()
        {
            // If the screen has a zero transition time, remove it immediately.
            ScreenManager.RemoveScreen(this);
        }
    }


}
