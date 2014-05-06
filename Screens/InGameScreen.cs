using MineLib.ClientWrapper;

namespace MineLib.GraphicClient.Screens
{
    public abstract class InGameScreen : Screen
    {
        public Minecraft Minecraft { get; set; }

        public bool Connected { get { return Minecraft.Connected; } }
        public bool Crashed { get { return Minecraft.Crashed; } }
    }
}
