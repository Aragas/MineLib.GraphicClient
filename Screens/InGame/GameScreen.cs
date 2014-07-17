using System;
using System.Threading;
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
    public enum PlayerDestination
    {
        None,
        Forward,
        ForwardLeft,
        ForwardRight,
        Backward,
        BackwardLeft,
        BackwardRight,
        Left,
        Right,
        Jump
    }

    public class PlayerInteractionValues
    {
        public bool ChatOn { get; set; }

        public bool PlayerCannotMove { get; set; }

        public bool PlayerMoving { get; set; }

        public bool PlayerCrouching { get; set; }

        public Keys[] PressedKeys { get; set; }

        public bool PlayerSprinting { get; set; }
    }

    class PlayerInteractionHandler
    {
        private readonly Minecraft _minecraft;
        private readonly PlayerInteractionValues _playerInteractionValues;

        public PlayerInteractionHandler(Minecraft minecraft, PlayerInteractionValues values)
        {
            _minecraft = minecraft;
            _playerInteractionValues = values;
        }

        public void Start()
        {
            while (!_minecraft.Crashed)
            {
                //if (_playerInteractionValues.PlayerMoving)
                if (_playerInteractionValues.PressedKeys != null)
                {
                    // Handle player moving every 50 mc (Moving speed is based on that)
                    PlayerMove(_playerInteractionValues.PressedKeys);
                    Thread.Sleep(50);
                }
            }
        }

        private void PlayerMove(Keys[] currentPressedKeys, bool onGround = true)
        {
            // Jump = 1.252 ; Speed?
            // Jump Boost I = 1.836
            // Jump Boost II = 2.517

            // Move = 4.317 ; 50 mc = 0,21585
            // Sprinting = 5.612 ; 50 mc = 0,2806

            // Crouch = 1.31 ;  50 mc = 0,0655

            // Crouch height = 0.08

            // Block player moving.
            if (_playerInteractionValues.PlayerCannotMove)
                            return;

            Vector3 previousPos = Vector3Converter.GetVector3Standart(_minecraft.Player.Position.Vector3);

            double crouch = 0;
            double x = 0, y = 0, z = 0;
            foreach (Keys key in currentPressedKeys)
            {
                #region Movement

                if (key == Keys.W)
                {
                    if (_playerInteractionValues.PlayerSprinting)
                        z = 0.2806;
                    if (_playerInteractionValues.PlayerCrouching)
                        z = 0.0655;
                    else
                        z = 0.21585;
                }

                if (key == Keys.S)
                {
                    if (_playerInteractionValues.PlayerSprinting)
                        z = -0.2806;
                    if (_playerInteractionValues.PlayerCrouching)
                        z = -0.0655;
                    else
                        z = -0.21585;
                }

                if (key == Keys.A)
                {
                    if (_playerInteractionValues.PlayerSprinting)
                        x = 0.2806;
                    if (_playerInteractionValues.PlayerCrouching)
                        x = 0.0655;
                    else
                        x = 0.21585;
                }

                if (key == Keys.D)
                {
                    if (_playerInteractionValues.PlayerSprinting)
                        x = -0.2806;
                    if (_playerInteractionValues.PlayerCrouching)
                        x = -0.0655;
                    else
                        x = -0.21585;
                }

                #endregion

                #region Other stuff

                // Not implemented
                if (key == Keys.Space)
                    //y = 1.252;
                    ;

                if (key == Keys.LeftShift)
                {
                    PlayerCrouch();
                    crouch = 0.08;
                }
                else
                {
                    PlayerUnCrouch();
                    crouch = 0;
                }

                if (key == Keys.LeftControl)
                    _playerInteractionValues.PlayerSprinting = true;
                else
                    _playerInteractionValues.PlayerSprinting = false;

                // Not implemented
                if (key == Keys.Q)
                    ;

                // Not implemented
                if (key == Keys.Enter) // Chat
                    ;

                // Not implemented
                if (key == Keys.T)
                    ;

                // TODO: Add jump

                #endregion

            }

            // Handle when no key is pressed
            if (currentPressedKeys.Length <= 0)
            {
                if (_playerInteractionValues.PlayerCrouching)
                {
                    PlayerUnCrouch();
                    crouch = 0;
                }

                _playerInteractionValues.PlayerSprinting = false;
            }

            _minecraft.SendPacket(new PlayerPositionPacket
            {
                X =         _minecraft.Player.Position.Vector3.X + x,
                HeadY =     _minecraft.Player.Position.Vector3.Y - crouch,
                FeetY =     _minecraft.Player.Position.Vector3.Y - 1.62, // For jump
                Z =         _minecraft.Player.Position.Vector3.Z + z,
                OnGround =  onGround
            });

            _minecraft.Player.Position.Vector3.Y += y;
            _minecraft.Player.Position.Vector3.X += x;
            _minecraft.Player.Position.Vector3.Z += z;

            Vector3 currentPos = Vector3Converter.GetVector3Standart(_minecraft.Player.Position.Vector3);

            _playerInteractionValues.PlayerMoving = previousPos != currentPos;
        }

        private void PlayerCrouch()
        {
            // Bug: Should we use Animation packet??
            //_minecraft.SendPacket(new AnimationPacket
            //{
            //    EntityID = _minecraft.Player.EntityID,
            //    Animation = Animation.Crouch
            //});

            _minecraft.SendPacket(new EntityActionPacket
            {
                EntityID = _minecraft.Player.EntityID,
                Action = EntityAction.Crouch,
                JumpBoost = 0
            });

            _playerInteractionValues.PlayerCrouching = true;
        }

        private void PlayerUnCrouch()
        {
            // Bug: Should we use Animation packet??
            //_minecraft.SendPacket(new AnimationPacket
            //{
            //    EntityID = _minecraft.Player.EntityID,
            //    Animation = Animation.UnCrouch
            //});

            _minecraft.SendPacket(new EntityActionPacket
            {
                EntityID = _minecraft.Player.EntityID,
                Action = EntityAction.UnCrouch,
                JumpBoost = 0
            });

            _playerInteractionValues.PlayerCrouching = false;
        }

    }

    sealed class GameScreen : InGameScreen
    {
        public PlayerInteractionHandler PlayerInteractionHandler;
        public Thread PlayerThread;

        bool disposed;
        private Texture2D _backgroundTexture;

        public GameScreen(GameClient gameClient, string username, string password, bool onlineMode = false)
        {
            GameClient = gameClient;

            Minecraft = new Minecraft(username, password, onlineMode);

            Name = "GameScreen";
        }

        public bool Connect(string serverip, short port)
        {
            Minecraft.Connect(serverip, port);

            while (!Minecraft.Connected)
            {
                if (Minecraft.Crashed)
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

            PlayerInteractionValues = new PlayerInteractionValues();
            PlayerInteractionHandler = new PlayerInteractionHandler(Minecraft, PlayerInteractionValues);
            PlayerThread = new Thread(PlayerInteractionHandler.Start) {Name = "PlayerInteractions"};
            PlayerThread.Start();

            return true;
        }

        public override void LoadContent()
        {
            ScreenManager.AddScreen(new GUIScreen(GameClient, Minecraft, PlayerInteractionValues));
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

        // Handle Player-with-world interaction here. Player-with-game in GUIScreen
        public override void HandleInput(InputState input)
        {
            PlayerInteractionValues.PressedKeys = input.CurrentKeyboardState.GetPressedKeys();

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

            Minecraft.Dispose();
            
            // Get rid of tons of trash!
            GC.Collect();
        }
    }
}
