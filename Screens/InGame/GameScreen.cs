using System;
using Microsoft.Xna.Framework;
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
    sealed class GameScreen : InGameScreen
    {
        InGameScreen GUIScreen;
        InGameScreen InventoryScreen;
        InGameScreen GameOptionScreen;

        public Minecraft Minecraft = null;
        public bool Connected = true;
        public bool Crashed = false;

        bool disposed;

        public GameScreen(GameClient gameClient, string username, string password, bool onlineMode = false)
        {
            GameClient = gameClient;

            //Minecraft = new Minecraft(username, password, onlineMode);

            Name = "GameScreen";
        }

        public bool Connect(string serverip, short port)
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

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public override void LoadContent()
        {
            GUIScreen = new GUIScreen(GameClient, Minecraft);
            ScreenManager.AddScreen(GUIScreen);

            InventoryScreen = null;

            GameOptionScreen = new GameOptionScreen(GameClient);
        }

        public override void UnloadContent()
        {
            Content.Unload();
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

        public override void HandleInput(InputState input)
        {
            if (input.IsNewKeyPress(Keys.Escape))
                ScreenManager.AddScreen(GameOptionScreen);        

            if(input.IsNewKeyPress(Keys.W))
                PlayerMove(Vector3.Backward);
        }

        public override void Update(GameTime gameTime)
        {
            if (Crashed)
                AddScreenAndCloseOthers(new ServerListScreen(GameClient));
        }

        public override void Draw(GameTime gameTime)
        {
            // Draw previous screen unless we are connected.
            if (Connected)
            {
                if (!disposed)
                {
                    disposed = true;
                }
            }
        }

        public override void Dispose()
        {
            base.Dispose();

            //Minecraft.Dispose();
            
            // Get rid of tons of trash!
            GC.Collect();
        }
    }
}
