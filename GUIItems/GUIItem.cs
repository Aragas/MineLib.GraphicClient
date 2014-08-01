using System;
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
        SelectedMouseHover,
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
        internal GraphicsDevice GraphicsDevice { get { return GameClient.GraphicsDevice; } }

        protected SpriteFont MainFont { get { return Content.Load<SpriteFont>("Minecraftia"); } }


        public string Name { get; set; }
        public GUIItemState GUIItemState { get; set; }

        public bool IsActive                { get { return GUIItemState == GUIItemState.Active; }}
        public bool IsJustNowActive         { get { return GUIItemState == GUIItemState.JustNowActive; } }
        public bool IsSelected              { get { return GUIItemState == GUIItemState.Selected; } }
        public bool IsSelectedMouseHover    { get { return GUIItemState == GUIItemState.SelectedMouseHover; } }
        public bool IsNonPressable          { get { return GUIItemState == GUIItemState.NonPressable; } }
        public bool IsUnfocused             { get { return GUIItemState == GUIItemState.Unfocused; } }
        public bool IsHidden                { get { return GUIItemState == GUIItemState.Hidden; } }

        public virtual void LoadContent() {}
        public virtual void UnloadContent() {}
        public virtual void Update(GameTime gameTime) {}
        public virtual void HandleInput(InputManager input) {}
        public virtual void Draw(GameTime gameTime) {}

        public void ToActive() { GUIItemState = GUIItemState.Active; }
        public void ToJustNowActive() { GUIItemState = GUIItemState.JustNowActive; }
        public void ToUnfocused() { GUIItemState = GUIItemState.Unfocused; }
        public void ToSelected() { GUIItemState = GUIItemState.Selected; }
        public void ToSelectedMouseHover() { GUIItemState = GUIItemState.SelectedMouseHover; }
        public void ToNonPressable() { GUIItemState = GUIItemState.NonPressable; }
        public void ToHidden() { GUIItemState = GUIItemState.Hidden; }

        // Some function from internet, forgot url.
        protected static void DrawString(SpriteBatch spriteBatch, SpriteFont font, Color color, string strToDraw, Rectangle boundaries)
        {
            Vector2 size = font.MeasureString(strToDraw);

            float xScale = (boundaries.Width / size.X);
            float yScale = (boundaries.Height / size.Y);

            // Taking the smaller scaling value will result in the text always fitting in the boundaries.
            float scale = Math.Min(xScale, yScale);

            // Figure out the location to absolutely-center it in the boundaries rectangle.
            int strWidth = (int)Math.Round(size.X * scale);
            int strHeight = (int)Math.Round(size.Y * scale);
            Vector2 position = new Vector2();
            position.X = (((boundaries.Width - strWidth) / 2) + boundaries.X);
            position.Y = (((boundaries.Height - strHeight) / 2) + boundaries.Y);

            // A bunch of settings where we just want to use reasonable defaults.
            float rotation = 0.0f;
            Vector2 spriteOrigin = new Vector2(0, 0);
            float spriteLayer = 0.0f; // all the way in the front
            SpriteEffects spriteEffects = new SpriteEffects();

            // Draw the string to the sprite batch!
            spriteBatch.DrawString(font, strToDraw, position, color, rotation, spriteOrigin, scale, spriteEffects, spriteLayer);
        }

        protected static void DrawStringWithoutCentering(SpriteBatch spriteBatch, SpriteFont font, Color color, string strToDraw, Rectangle boundaries)
        {
            Vector2 size = font.MeasureString(strToDraw);

            float xScale = (boundaries.Width / size.X);
            float yScale = (boundaries.Height / size.Y);

            // Taking the smaller scaling value will result in the text always fitting in the boundaries.
            float scale = Math.Min(xScale, yScale);

            // Figure out the location to absolutely-center it in the boundaries rectangle.
            int strWidth = (int)Math.Round(size.X * scale);
            int strHeight = (int)Math.Round(size.Y * scale);
            Vector2 position = new Vector2();
            position.X = (boundaries.X);
            position.Y = (((boundaries.Height * 0.5f - strHeight) * 0.5f) + boundaries.Y);

            //position.X = (((boundaries.Width - strWidth) / 2) + boundaries.X);
            //position.Y = (((boundaries.Height - strHeight) / 2) + boundaries.Y);

            // A bunch of settings where we just want to use reasonable defaults.
            float rotation = 0.0f;
            Vector2 spriteOrigin = new Vector2(0, 0);
            float spriteLayer = 0.0f; // all the way in the front
            SpriteEffects spriteEffects = new SpriteEffects();

            // Draw the string to the sprite batch!
            spriteBatch.DrawString(font, strToDraw, position, color, rotation, spriteOrigin, scale, spriteEffects, spriteLayer);
        }
    }
}
