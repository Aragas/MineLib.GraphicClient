using Microsoft.Xna.Framework.Input;

namespace MineLib.GraphicClient.Screens
{
    sealed class OptionScreen : Screen
    {
        public OptionScreen(GameClient gameClient)
        {
            GameClient = gameClient;
            Name = "OptionScreen";
        }

        public override void HandleInput(InputState input)
        {
            if (input.IsOncePressed(Keys.Escape))
                AddScreenAndExit(new MainMenuScreen(GameClient));
        }
    }
}
