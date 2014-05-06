using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MineLib.ClientWrapper;
using MineLib.Network.Enums;
using MineLib.Network.Packets;
using MineLib.Network.Packets.Client;
using MineLib.Network.Packets.Client.Login;

public static class Vector3Converter
{
    public static Vector3 GetVector3Standart(MineLib.Network.Data.Vector3 vector3)
    {
        return new Vector3((float)vector3.X, (float)vector3.Y, (float)vector3.Z);
    }

    public static MineLib.Network.Data.Vector3 GetVector3Standart(Vector3 vector3)
    {
        return new MineLib.Network.Data.Vector3(vector3.X, vector3.Y, vector3.Z);
    }
}

namespace MineLib.GraphicClient.Screens
{
    sealed class GameScreen : Screen, IDisposable
    {
        public override GameClient GameClient { get; set; }

        Rectangle _screenRectangle;

        Minecraft Minecraft;

        public bool IsActive = true;
        public bool Connected = false;

        Screen guiScreen;
        InventoryScreen inventoryScreen;
        Screen gameOptionScreen;

        public GameScreen(GameClient gameClient, string username, string password, bool onlineMode = false)
        {
            GameClient = gameClient;

            Minecraft = new Minecraft(username, password, onlineMode);

            guiScreen = new GUIScreen(GameClient, Minecraft);
        }

        public GameScreen Connect(string serverip, short port)
        {
            try
            {
                Minecraft.Connect(serverip, port);

                while (!Minecraft.Connected)
                {
                    if(Minecraft.Crashed)
                        throw new Exception("Connection error");
                }

                Minecraft.SendPacket(new HandshakePacket
                {
                    ProtocolVersion = 5,
                    ServerAddress = Minecraft.ServerIP,
                    ServerPort = Minecraft.ServerPort,
                    NextState = NextState.Login,
                });

                Minecraft.SendPacket(new LoginStartPacket {Name = Minecraft.ClientName});

                while (Minecraft.State != ServerState.Play)
                {
                    if (Minecraft.Crashed)
                        throw new Exception("Connection error");
                }

                Minecraft.SendPacket(new ClientStatusPacket {Status = ClientStatus.Respawn});

                Connected = true;
                return this;
            }
            catch (Exception)
            {
                GameClient.SetScreen(new ServerListScreen(GameClient));
                GameClient.DisposePreviousScreen();
            }
            return this;
        }

        public override void LoadContent()
        {
            base.LoadContent();

            _screenRectangle = GameClient.Window.ClientBounds;
        }

        void PlayerMove(Vector3 position, double crouch = 1.62, bool onGround = true)
        {
            // -- Debugging
            position = Vector3Converter.GetVector3Standart(Minecraft.Player.Position.Vector3);
            // -- Debugging

            Minecraft.SendPacket(new PlayerPositionPacket
            {
                X = position.X + 1,
                HeadY = position.Y,
                FeetY = position.Y - crouch,
                Z = position.Z,
                OnGround = onGround
            });

            Minecraft.SendPacket(new PlayerPositionPacket
            {
                X = position.X + 1,
                HeadY = position.Y,
                FeetY = position.Y - crouch,
                Z = position.Z,
                OnGround = onGround
            });

            Minecraft.Player.Position.Vector3.X += 1;
        }

        bool _escKeyboardIsDown;
        bool _wKeyboardIsDown;
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if(Minecraft.Crashed)
                SetScreen(new ServerListScreen(GameClient));

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                if (!_escKeyboardIsDown)
                {
                    if (IsActive)
                    {
                        IsActive = false;
                        gameOptionScreen = new GameOptionScreen(GameClient);
                    }
                    else
                        IsActive = true;
                }
                _escKeyboardIsDown = true;
            }
            else
                _escKeyboardIsDown = false;


            if (IsActive)
            {
                #region PlayerKeyboard

                if (Keyboard.GetState().IsKeyDown(Keys.W))
                {
                    if (!_wKeyboardIsDown)
                    {
                        PlayerMove(Vector3.Backward);
                    }
                    _wKeyboardIsDown = true;
                }
                else
                    _wKeyboardIsDown = false;

                if (Keyboard.GetState().IsKeyDown(Keys.A))
                    ;

                if (Keyboard.GetState().IsKeyDown(Keys.S))
                    ;

                if (Keyboard.GetState().IsKeyDown(Keys.D))
                    ;

                #endregion

                guiScreen.Update(gameTime);
            }

            if (inventoryScreen != null)
                inventoryScreen.Update(gameTime);

            if (gameOptionScreen != null)
                gameOptionScreen.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if (Connected)
            {
                guiScreen.Draw(spriteBatch);

                if (!IsActive)
                {
                    if (inventoryScreen != null)
                        inventoryScreen.Draw(spriteBatch);
                    if (gameOptionScreen != null)
                        gameOptionScreen.Draw(spriteBatch);
                }
            }
            else
                //GameClient.GraphicsDevice.Clear(Color.Black);
                GameClient.PreviousScreen.Draw(spriteBatch);
        }

        public override void Dispose()
        {
            base.Dispose();

            Minecraft.Dispose();
            
            // Get rid of tons of trash!
            GC.Collect();
        }
    }
}
