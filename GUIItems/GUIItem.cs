using Microsoft.Xna.Framework;

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
        public virtual void HandleInput(InputState input) {}
        public virtual void Draw(GameTime gameTime) {}

        public void ToActive() { GUIItemState = GUIItemState.Active; }
        public void ToJustNowActive() { GUIItemState = GUIItemState.JustNowActive; }
        public void ToUnfocused() { GUIItemState = GUIItemState.Unfocused; }
        public void ToSelected() { GUIItemState = GUIItemState.Selected; }
        public void ToNonPressable() { GUIItemState = GUIItemState.NonPressable; }
        public void ToHidden() { GUIItemState = GUIItemState.Hidden; }
    }
}
