using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MineLib.GraphicClient.Misc;

namespace MineLib.GraphicClient.GUIItems.Button
{
    public abstract class GUIButton : GUIItem
    {
        public event Action OnButtonPressed;

        protected string ButtonText;
        protected Rectangle ButtonRectangle { get; set; }
        protected Rectangle ButtonRectangleShadow { get { return new Rectangle(ButtonRectangle.X + 2, ButtonRectangle.Y + 2, ButtonRectangle.Width, ButtonRectangle.Height); } }
        protected static Rectangle ButtonPosition = new Rectangle(0, 66, 200, 20);
        protected static Rectangle ButtonPressedPosition = new Rectangle(0, 86, 200, 20);
        protected static Rectangle ButtonUnavailablePosition = new Rectangle(0, 46, 200, 20);

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

        // Handle is same for all buttons, so it can be moved here instead of copying in each implementation
        public override void HandleInput(InputManager input)
        {

            #region Mouse handling

            MouseState mouse = input.CurrentMouseState;

            if (ButtonRectangle.Intersects(new Rectangle(mouse.X, mouse.Y, 1, 1)) && !IsNonPressable)
            {
                ToSelectedMouseHover();

                if (input.CurrentMouseState.LeftButton == ButtonState.Pressed &&
                    input.LastMouseState.LeftButton == ButtonState.Released)
                {
                    if (OnButtonPressed != null)
                        OnButtonPressed();
                }
            }
            else if (!IsSelected && !IsNonPressable)
                ToActive();

            #endregion

        }

        public void PressButton()
        {
            OnButtonPressed();
        }
    }
}
