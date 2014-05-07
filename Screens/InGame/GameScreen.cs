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
    sealed class GameScreen : InGameScreen
    {
        InGameScreen GUIScreen;
        InGameScreen InventoryScreen;
        GameOptionScreen GameOptionScreen;

        public new Minecraft Minecraft = null;
        public new bool Connected = true;
        public new bool Crashed = false;

        bool disposed;

        public GameScreen(GameClient gameClient, string username, string password, bool onlineMode = false)
        {
            GameClient = gameClient;

            //Minecraft = new Minecraft(username, password, onlineMode);

            GUIScreen = new GUIScreen(GameClient, Minecraft);
            InventoryScreen = null;
            GameOptionScreen = null;

            IsActive = true;
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

            if (Crashed)
                SetScreen(new ServerListScreen(GameClient));

            #region Escape handling

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                if (!_escKeyboardIsDown)
                {
                    if (IsActive)
                    {
                        if (GameOptionScreen == null)
                            GameOptionScreen = new GameOptionScreen(GameClient);

                        IsActive = false;
                    }
                    else
                        IsActive = true;
                }
                _escKeyboardIsDown = true;
            }
            else
                _escKeyboardIsDown = false;

            #endregion

            if (Connected && IsActive)
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

                GUIScreen.Update(gameTime);
            }

            if (InventoryScreen != null && !IsActive)
                InventoryScreen.Update(gameTime);

            if (GameOptionScreen != null && !IsActive)
                GameOptionScreen.Update(gameTime);

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            // Draw previous screen unless we are connected.
            if (Connected)
            {
                if (!disposed)
                {
                    DisposePreviousScreen();
                    disposed = true;
                }

                GUIScreen.Draw(spriteBatch);

                if (InventoryScreen != null && !IsActive)
                    InventoryScreen.Draw(spriteBatch);

                if (GameOptionScreen != null && !IsActive)
                    GameOptionScreen.Draw(spriteBatch);
            }
            else
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
