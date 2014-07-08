using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MineLib.GraphicClient.GUIButtons;

namespace MineLib.GraphicClient.Screens
{
    public abstract class Screen
    {
        public GameClient GameClient { get; set; }

        public List<GUIButton> ButtonList = new List<GUIButton>();
        public string Name { get; set; }

        public MinecraftTexturesStorage MinecraftTexturesStorage { get { return GameClient.MinecraftTexturesStorage; }}
        public Rectangle ScreenRectangle { get { return GameClient.Window.ClientBounds; }}

        public ContentManager Content { get { return ScreenManager.Content; }}

        public bool ContentLoaded { get; set; }
        public ScreenManagerComponent ScreenManager { get { return GameClient.ScreenManager; } }
        public ScreenState ScreenState { get; set; }
        public SpriteBatch SpriteBatch { get { return ScreenManager.SpriteBatch; }}

        protected void AddScreen(Screen screen) { ScreenManager.AddScreen(screen); }
        protected void AddScreenAndCloseOthers(Screen screen) { ScreenManager.CloseOtherScreens(screen); } 
        protected void AddScreenAndExit(Screen screen) { ScreenManager.AddScreen(screen); ExitScreen(); }

        public virtual void LoadContent() { }
        public virtual void UnloadContent() { }
        public virtual void HandleInput(InputState input) { }
        public virtual void Update(GameTime gameTime) { }
        public virtual void Draw(GameTime gameTime) { }
        public virtual void Dispose() 
        {
            //foreach (GUIButton button in Buttons)
            //{
            //    button.Dispose();
            //}
            //ButtonList = null;
        }

        public void ToActive() { ScreenState = ScreenState.JustNowActive;}
        public void ToBackground() { ScreenState = ScreenState.Background;}
        public void ToHidden() { ScreenState = ScreenState.Hidden;}

        public void Exit() { GameClient.Exit(); }

        public virtual void ExitScreen()
        {
            // If the screen has a zero transition time, remove it immediately.
            ScreenManager.RemoveScreen(this);
        }
    }


}
