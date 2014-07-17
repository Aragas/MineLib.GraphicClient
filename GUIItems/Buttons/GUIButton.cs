using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MineLib.GraphicClient.GUIItems.Buttons
{
    public abstract class GUIButton : GUIItem
    {
        public event Action OnButtonPressed;

        protected GameClient GameClient { get; set; }

        protected string ButtonText;
        protected Rectangle ButtonRectangle { get; set; }
        protected Rectangle ButtonRectangleShadow { get { return new Rectangle(ButtonRectangle.X + 2, ButtonRectangle.Y + 2, ButtonRectangle.Width, ButtonRectangle.Height); } }
        protected static Rectangle ButtonPosition = new Rectangle(0, 66, 200, 20);
        protected static Rectangle ButtonPressedPosition = new Rectangle(0, 86, 200, 20);
        protected static Rectangle ButtonUnavailablePosition = new Rectangle(0, 46, 200, 20);
        protected SpriteFont ButtonFont { get { return Content.Load<SpriteFont>("Minecraftia"); } }

        #region HalfButton

        protected Rectangle ButtonFirstHalfPosition = new Rectangle(0, 66, 49, 20);
        protected Rectangle ButtonSecondHalfPosition = new Rectangle(151, 66, 49, 20);

        protected Rectangle ButtonPressedFirstHalfPosition = new Rectangle(0, 86, 49, 20);
        protected Rectangle ButtonPressedSecondHalfPosition = new Rectangle(151, 86, 49, 20);

        protected Rectangle ButtonUnavailableFirstHalfPosition = new Rectangle(0, 46, 49, 20);
        protected Rectangle ButtonUnavailableSecondHalfPosition = new Rectangle(151, 46, 49, 20);

        protected Rectangle ButtonRectangleFirstHalf;
        protected Rectangle ButtonRectangleSecondHalf;

        #endregion

        //protected Color ButtonColor = new Color(224, 224, 224, 255); // Vanilla
        protected Color ButtonColor = Color.White;
        //protected Color ButtonShadowColor = new Color(54, 54, 54, 255); // Vanilla
        protected Color ButtonShadowColor = Color.Black;
        //protected Color ButtonPressedColor = new Color(255, 255, 160, 255); // Vanilla
        protected Color ButtonPressedColor = Color.Yellow;
        //protected Color ButtonPressedShadowColor = new Color(63, 63, 40, 255); // Vanilla
        protected Color ButtonPressedShadowColor = Color.Black;
        //protected Color ButtonUnavailableColor = new Color(160, 160, 160, 255); // Vanilla
        protected Color ButtonUnavailableColor = Color.Gray; // Vanilla

        protected Texture2D WidgetsTexture { get { return GameClient.MinecraftTexturesStorage.GUITextures.Widgets; } }

        protected Rectangle ScreenRectangle { get { return GameClient.Window.ClientBounds; } }

        protected ContentManager Content { get { return GUIItemManager.Content; } }
        protected GUIItemManagerComponent GUIItemManager { get { return GameClient.GUIItemManager; } }
        protected SpriteBatch SpriteBatch { get { return GUIItemManager.SpriteBatch; } }

        // Handle is same for all buttons, so it can be moved here instead of copying in each implementation
        public override void HandleInput(InputState input)
        {

            #region Mouse handling

            MouseState mouse = input.CurrentMouseState;

            if (ButtonRectangle.Intersects(new Rectangle(mouse.X, mouse.Y, 1, 1)) && !IsNonPressable)
            {
                ToSelected();

                if (input.CurrentMouseState.LeftButton == ButtonState.Pressed &&
                    input.LastMouseState.LeftButton == ButtonState.Released)
                {
                    if (OnButtonPressed != null)
                        OnButtonPressed();
                }
            }
            else
                ToActive();

            #endregion

        }

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
