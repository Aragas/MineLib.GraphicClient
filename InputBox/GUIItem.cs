using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MineLib.GraphicClient.InputBox
{
    public enum GUIItemState
    {
        Active,
        JustNowActive,
        Hidden
    }

    public abstract class GUIItem
    {
        public abstract void LoadContent();
        public abstract void UnloadContent();
        public abstract void Update(GameTime gameTime);
        public GUIItemState GUIItemState { get; set; }
        public string Name { get; set; }

        public abstract void HandleInput(InputState input);
        public abstract void Draw(GameTime gameTime);
    }
}
