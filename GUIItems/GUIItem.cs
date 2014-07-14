using Microsoft.Xna.Framework;

namespace MineLib.GraphicClient.GUIItems
{
    public enum GUIItemState
    {
        Active,
        NonPressable,
        Hidden,
        JustNowActive
    }

    public abstract class GUIItem
    {
        public string Name { get; set; }
        public GUIItemState GUIItemState { get; set; }

        public virtual void LoadContent() {}
        public virtual void UnloadContent() {}
        public virtual void Update(GameTime gameTime) {}
        public virtual void HandleInput(InputState input) {}
        public virtual void Draw(GameTime gameTime) {}
    }
}
