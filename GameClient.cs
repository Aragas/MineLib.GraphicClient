using System.IO;
using Ionic.Zip;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MineLib.GraphicClient.GUIItems;
using MineLib.GraphicClient.Misc;
using MineLib.GraphicClient.Screens;

namespace MineLib.GraphicClient
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class GameClient : Game
    {
        public AudioManagerComponent AudioManager;
        public ScreenManagerComponent ScreenManager;
        public GUIItemManagerComponent GUIItemManager;

        FPSCounterComponent FPS;
        SpriteFont FPSFont;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public MinecraftTexturesStorage MinecraftTexturesStorage;

        // Client settings, temporary storage
        public static string Login = "TestBot";
        public static string Password = "";
        public static bool OnlineMode = false;

        public PlayerData Player = new PlayerData
        {
            Username = Login,
            Password = Password,
            OnlineMode = OnlineMode
        };

        public GameClient()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            //graphics.IsFullScreen = true;
            //graphics.PreferredBackBufferWidth = 1280;
            //graphics.PreferredBackBufferHeight = 720;
            graphics.ApplyChanges();

            // Not implemented
            //AudioManager = new AudioManagerComponent(this, new DirectoryInfo(Content.RootDirectory + @"\Audio"));
            //Components.Add(AudioManager);

            // Order is important
            ScreenManager = new ScreenManagerComponent(this);
            Components.Add(ScreenManager);

            GUIItemManager = new GUIItemManagerComponent(this);
            Components.Add(GUIItemManager);
        }

        protected override void Initialize()
        {
            base.Initialize();

            ScreenManager.AddScreen(new MainMenuScreen(this));
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            IsMouseVisible = true;

            FPSFont = Content.Load<SpriteFont>("VolterGoldfish");

            FPS = new FPSCounterComponent(this, spriteBatch, FPSFont);
            Components.Add(FPS);

            #region Load resources from minecraft.jar
            string path = Content.RootDirectory;
            if (File.Exists(Path.Combine(path, "minecraft.jar")))
            {
                ZipFile minecraftFiles = new ZipFile(Path.Combine(path, "minecraft.jar"));
                MinecraftTexturesStorage = new MinecraftTexturesStorage(this, minecraftFiles);

                // Load textures or somethin'
                MinecraftTexturesStorage.ParseGUITextures();
            }
            #endregion

        }

        protected override void UnloadContent()
        {
            base.UnloadContent();

        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            base.Draw(gameTime);

            spriteBatch.End();

        }
    }
}
