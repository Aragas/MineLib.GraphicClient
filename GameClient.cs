using System.IO;
using Ionic.Zip;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MineLib.GraphicClient.Screens;

using Screen = MineLib.GraphicClient.Screens.Screen;


namespace MineLib.GraphicClient
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class GameClient : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public Screen CurrentScreen { get; private set; }
        public Screen PreviousScreen { get; private set; }

        public MinecraftTexturesStorage MinecraftTexturesStorage;

        // Client settings, temporary storage
        public string Login = "TestBot";
        public string Password = "";
        public bool OnlineMode = false;

        public GameClient()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            //graphics.IsFullScreen = true;
            //graphics.PreferredBackBufferWidth = 1280;
            //graphics.PreferredBackBufferHeight = 720;
            graphics.ApplyChanges();
            
            CurrentScreen = new MainMenuScreen(this) {IsActive = true};
        }


        public void SetScreen(Screen screen)
        {
            DisposePreviousScreen();

            PreviousScreen = CurrentScreen;
            CurrentScreen = screen;
        }

        public void DisposePreviousScreen()
        {
            if (PreviousScreen != null)
            {
                PreviousScreen.Dispose();
                PreviousScreen = null;
            }
        }


        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            IsMouseVisible = true;

            #region Load resources from minecraft.jar
            string path = Content.RootDirectory;
            if (File.Exists(Path.Combine(path, "minecraft.jar")))
            {
                ZipFile minecraftFiles = new ZipFile(Path.Combine(path, "minecraft.jar"));
                MinecraftTexturesStorage = new MinecraftTexturesStorage(this, minecraftFiles);

                // Load textures or somethin'
                MinecraftTexturesStorage.GetGUITextures();
            }
            #endregion

        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            CurrentScreen.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            GraphicsDevice.Clear(Color.CornflowerBlue);

            #region 2D render
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointClamp,
                DepthStencilState.None, RasterizerState.CullNone);

            CurrentScreen.Draw(spriteBatch);

            spriteBatch.End();
            #endregion
        }

        public void Quit()
        {
            CurrentScreen.Dispose();

            Exit();
        }

    }
}
