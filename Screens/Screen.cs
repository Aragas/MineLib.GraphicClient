using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MineLib.GraphicClient.Screens
{
    abstract public class Screen
    {
        public virtual GameClient GameClient { get; set; }
        public MinecraftTexturesStorage MinecraftTexturesStorage { get { return GameClient.MinecraftTexturesStorage; }}

        public virtual bool IsActive { get; set; }
        public virtual bool ContentLoaded { get; set; }

        public void SetScreen(Screen screen) { GameClient.SetScreen(screen);}
        public void SetScreenAndDisposePreviousScreen(Screen screen) { GameClient.SetScreen(screen); GameClient.DisposePreviousScreen(); }
        public void DisposePreviousScreen() { GameClient.DisposePreviousScreen(); }

        public virtual void LoadContent() { ContentLoaded = true; }
        public virtual void Update(GameTime gameTime) { if(!ContentLoaded) { LoadContent();} }
        public virtual void Draw(SpriteBatch spriteBatch) { if (!ContentLoaded) { LoadContent(); } }
        public virtual void Dispose() { }

        public void Exit() { GameClient.Exit(); }
    }


}
