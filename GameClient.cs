using System.IO;
using Ionic.Zip;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MineLib.GraphicClient.Screens;


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
        public Screen PreviousScreen;

        public ZipFile MinecraftFiles;
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

            
            CurrentScreen = new MainMenuScreen(this);
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

            string path = Content.RootDirectory;
            if (File.Exists(Path.Combine(path, "minecraft.jar")))
            {
                MinecraftFiles = new ZipFile(Path.Combine(path, "minecraft.jar"));
                MinecraftTexturesStorage = new MinecraftTexturesStorage(this, MinecraftFiles);

                // Load textures or somethin'
                MinecraftTexturesStorage.GetGUITextures();
            }

            // TODO: use this.Content to load your game content here

            IsMouseVisible = true;
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || 
            //    Keyboard.GetState().IsKeyDown(Keys.Escape))
            //    Quit();

            CurrentScreen.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            #region 2D render
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointClamp,
                DepthStencilState.None, RasterizerState.CullNone);

            CurrentScreen.Draw(spriteBatch);

            spriteBatch.End();
            #endregion

            base.Draw(gameTime);
        }

        public void Quit()
        {
            CurrentScreen.Dispose();

            Exit();
        }

    }
}
