using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MineLib.GraphicClient.GUIButtons;

namespace MineLib.GraphicClient.Screens
{
    // TODO: Delete all base calls, now it is a game component
    // TODO: Use AddScreenAndExit
    // TODO: Implement UnloadContent
    public abstract class Screen
    {
        public GameClient GameClient { get; set; }

        public List<GUIButton> ButtonList = new List<GUIButton>();

        public MinecraftTexturesStorage MinecraftTexturesStorage { get { return GameClient.MinecraftTexturesStorage; }}
        public Rectangle ScreenRectangle { get { return GameClient.Window.ClientBounds; }}

        public bool ContentLoaded { get; set; }
        public ScreenManagerComponent ScreenManager { get { return GameClient.ScreenManager; } }
        public ScreenState ScreenState { get; set; }
        public SpriteBatch SpriteBatch { get { return ScreenManager.SpriteBatch; }}

        protected void AddScreen(Screen screen) { ScreenManager.AddScreen(screen); }
        protected void AddScreenAndExit(Screen screen) { ScreenManager.AddScreen(screen); ExitScreen(); }

        public virtual void LoadContent() { ContentLoaded = true; }
        public virtual void HandleInput(InputState input) { if (!ContentLoaded) { LoadContent(); } }
        public virtual void Update(GameTime gameTime) { if(!ContentLoaded) { LoadContent();} }
        public virtual void Draw(GameTime gameTime) { if (!ContentLoaded) { LoadContent(); } }
        public virtual void Dispose() 
        {
            //foreach (GUIButton button in Buttons)
            //{
            //    button.Dispose();
            //}
            //ButtonList = null;
        }

        public void Exit() { GameClient.Exit(); }

        public virtual void ExitScreen()
        {
            // If the screen has a zero transition time, remove it immediately.
            ScreenManager.RemoveScreen(this);
        }
    }


}
