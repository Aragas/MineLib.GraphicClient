using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MineLib.GraphicClient.Misc;

namespace MineLib.GraphicClient.GUIItems
{
    public enum GUIItemState
    {
        Active,
        JustNowActive,
        Selected,
        NonPressable,
        Unfocused,
        Hidden
    }

    public abstract class GUIItem
    {
        protected GameClient GameClient { get; set; }

        protected Rectangle ScreenRectangle { get { return GameClient.Window.ClientBounds; } }

        protected ContentManager Content { get { return GUIItemManager.Content; } }
        protected GUIItemManagerComponent GUIItemManager { get { return GameClient.GUIItemManager; } }
        protected SpriteBatch SpriteBatch { get { return GUIItemManager.SpriteBatch; } }


        public string Name { get; set; }
        public GUIItemState GUIItemState { get; set; }

        public bool IsActive        { get { return GUIItemState == GUIItemState.Active; }}
        public bool IsJustNowActive { get { return GUIItemState == GUIItemState.JustNowActive; } }
        public bool IsSelected      { get { return GUIItemState == GUIItemState.Selected; } }
        public bool IsNonPressable  { get { return GUIItemState == GUIItemState.NonPressable; } }
        public bool IsUnfocused     { get { return GUIItemState == GUIItemState.Unfocused; } }
        public bool IsHidden        { get { return GUIItemState == GUIItemState.Hidden; } }

        public virtual void LoadContent() {}
        public virtual void UnloadContent() {}
        public virtual void Update(GameTime gameTime) {}
        public virtual void HandleInput(InputManager input) {}
        public virtual void Draw(GameTime gameTime) {}

        public void ToActive() { GUIItemState = GUIItemState.Active; }
        public void ToJustNowActive() { GUIItemState = GUIItemState.JustNowActive; }
        public void ToUnfocused() { GUIItemState = GUIItemState.Unfocused; }
        public void ToSelected() { GUIItemState = GUIItemState.Selected; }
        public void ToNonPressable() { GUIItemState = GUIItemState.NonPressable; }
        public void ToHidden() { GUIItemState = GUIItemState.Hidden; }
    }
}
