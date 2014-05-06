using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MineLib.GraphicClient.Screens
{
    public abstract class Screen
    {
        public GameClient GameClient { get; set; }

        public MinecraftTexturesStorage MinecraftTexturesStorage { get { return GameClient.MinecraftTexturesStorage; }}
        public Rectangle ScreenRectangle { get { return GameClient.Window.ClientBounds; }}

        public bool IsActive { get; set; }
        public bool ContentLoaded { get; set; }

        protected void SetScreen(Screen screen) { GameClient.SetScreen(screen);}
        protected void SetScreenAndDisposePreviousScreen(Screen screen) { GameClient.SetScreen(screen); GameClient.DisposePreviousScreen(); }
        protected void DisposePreviousScreen() { GameClient.DisposePreviousScreen(); }

        public virtual void LoadContent() { ContentLoaded = true; }
        public virtual void Update(GameTime gameTime) { if(!ContentLoaded) { LoadContent();} }
        public virtual void Draw(SpriteBatch spriteBatch) { if (!ContentLoaded) { LoadContent(); } }
        public virtual void Dispose() { }

        public void Exit() { GameClient.Exit(); }
    }


}
