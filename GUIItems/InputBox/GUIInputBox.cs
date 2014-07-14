using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MineLib.GraphicClient.GUIItems.InputBox
{
    public abstract class GUIInputBox : GUIItem
    {
        protected GameClient GameClient { get; set; }

        protected Rectangle ScreenRectangle { get { return GameClient.Window.ClientBounds; } }
        protected ContentManager Content { get { return GUIItemManager.Content; } }
        protected GUIItemManagerComponent GUIItemManager { get { return GameClient.GUIItemManager; } }
        protected SpriteBatch SpriteBatch { get { return GUIItemManager.SpriteBatch; } }

        protected void AddInputBox(GUIInputBox inputBox) { GUIItemManager.AddGUIItem(inputBox); }

        public void ToActive() { GUIItemState = GUIItemState.JustNowActive; }
        public void ToNonPressable() { GUIItemState = GUIItemState.NonPressable; }
        public void ToHidden() { GUIItemState = GUIItemState.Hidden; }

        protected bool IsSelected;

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
    }
}
