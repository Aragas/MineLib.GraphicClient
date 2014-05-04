using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MineLib.GraphicClient.MonoGame.Screens
{
    abstract public class Screen
    {
        public bool ContentLoaded;
        public virtual void LoadContent() { }
        public virtual void Update(GameTime gameTime) { if (!ContentLoaded) { LoadContent(); ContentLoaded = true;} }
        public virtual void Draw(SpriteBatch spriteBatch) { }
    }


}
