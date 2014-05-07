using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MineLib.GraphicClient.GUIButtons;

namespace MineLib.GraphicClient.Screens
{
    public abstract class Screen
    {
        public GameClient GameClient { get; set; }

        public List<GUIButton> ButtonList = new List<GUIButton>();

        public MinecraftTexturesStorage MinecraftTexturesStorage { get { return GameClient.MinecraftTexturesStorage; }}
        public Rectangle ScreenRectangle { get { return GameClient.Window.ClientBounds; }}

        public bool IsActive { get; set; }
        public bool ContentLoaded { get; set; }

        protected void SetScreen(Screen screen) { IsActive = false; GameClient.SetScreen(screen); GameClient.CurrentScreen.IsActive = true;}
        protected void SetScreenAndDisposePreviousScreen(Screen screen) { IsActive = false; GameClient.SetScreen(screen); GameClient.CurrentScreen.IsActive = true; GameClient.DisposePreviousScreen(); }
        protected void DisposePreviousScreen() { GameClient.DisposePreviousScreen(); }

        public virtual void LoadContent() { ContentLoaded = true; }
        public virtual void Update(GameTime gameTime) { if(!ContentLoaded) { LoadContent();} }
        public virtual void Draw(SpriteBatch spriteBatch) { if (!ContentLoaded) { LoadContent(); } }
        public virtual void Dispose() 
        {
            //foreach (GUIButton button in Buttons)
            //{
            //    button.Dispose();
            //}
            //GameClient = null;
            //ButtonList = null;
        }

        public void Exit() { GameClient.Exit(); }
    }


}
