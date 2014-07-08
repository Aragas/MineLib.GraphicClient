using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MineLib.GraphicClient.GUIButtons
{
    // TODO: Create GUIButtonManager similar to ScreenManager
    public abstract class GUIButton
    {
        public GameClient GameClient { get; set; }

        public string Name { get; set; }

        protected const int ButtonWidth = 200;
        protected const int ButtonHeight = 20;

        protected string ButtonText;
        protected Rectangle ButtonRectangle { get; set; }
        protected Rectangle ButtonPosition = new Rectangle(0, 66, 200, 20);
        protected Rectangle ButtonPressedPosition = new Rectangle(0, 86, 200, 20);
        protected SpriteFont ButtonFont { get { return Content.Load<SpriteFont>("VolterGoldfish"); } }

        public GUIButtonState GUIButtonState { get; set; }
        public Texture2D WidgetsTexture { get { return GameClient.MinecraftTexturesStorage.GUITextures.Widgets; } }

        public Rectangle ScreenRectangle { get { return GameClient.Window.ClientBounds; } }

        public ContentManager Content { get { return GUIButtonManager.Content; } }

        public GUIButtonManagerComponent GUIButtonManager { get { return GameClient.GuiButtonManager; } }
        public SpriteBatch SpriteBatch { get { return GUIButtonManager.SpriteBatch; } }

        protected void AddButton(GUIButton button) { GUIButtonManager.AddButton(button); }
        protected virtual Vector2 GetPosition(GUIButtonNormalPos level) { return new Vector2(0); }

        public virtual void LoadContent() { }
        public virtual void UnloadContent() { }
        public virtual void HandleInput(InputState input) { }
        public virtual void Update(GameTime gameTime) { }
        public virtual void Draw(GameTime gameTime) { }

        public void ToActive() { GUIButtonState = GUIButtonState.JustNowActive; }
        public void ToNonPressable() { GUIButtonState = GUIButtonState.NonPressable; }
        public void ToHidden() { GUIButtonState = GUIButtonState.Hidden; }

        public bool IsSelected;

        protected bool EventCalled;

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

        public virtual event Action OnButtonPressed;
    }
}
