﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MineLib.GraphicClient.GUIItems;
using MineLib.GraphicClient.GUIItems.Button;
using MineLib.GraphicClient.GUIItems.InputBox;
using MineLib.GraphicClient.Misc;

namespace MineLib.GraphicClient.Screens
{
    public abstract class Screen
    {
        protected GameClient GameClient { get; set; }

        public string Name { get; protected set; }
        public ScreenState ScreenState { get; set; }

        internal MinecraftTexturesStorage MinecraftTexturesStorage { get { return GameClient.MinecraftTexturesStorage; } }
        internal Rectangle ScreenRectangle { get { return GameClient.Window.ClientBounds; } }

        internal ContentManager Content { get { return ScreenManager.Content; } }

        protected GUIItemManagerComponent GUIItemManager { get { return GameClient.GUIItemManager; } }
        protected ScreenManagerComponent ScreenManager { get { return GameClient.ScreenManager; } }
        internal SpriteBatch SpriteBatch { get { return ScreenManager.SpriteBatch; } }
        internal GraphicsDevice GraphicsDevice { get { return GameClient.GraphicsDevice; } }

        internal static Color MainBackgroundColor { get { return new Color(30, 30, 30, 255); } }
        internal static Color SecondaryBackgroundColor { get { return new Color(75, 75, 75, 255); } }

        protected void AddScreen(Screen screen) { ScreenManager.AddScreen(screen); }
        protected void AddScreenAndHideThis(Screen screen) { ScreenManager.AddScreen(screen); ToHidden();}
        protected void AddScreenAndCloseOthers(Screen screen) { GUIItemManager.Clear(); ScreenManager.CloseOtherScreens(screen); }
        protected void AddScreenAndExit(Screen screen) { GUIItemManager.Clear(); ScreenManager.AddScreen(screen); ExitScreen(); }
        protected ButtonMenu AddButtonMenu(string name, ButtonMenuPosition pos, Action action)
        {
            ButtonMenu button = new ButtonMenu(GameClient, name, pos);
            button.OnButtonPressed += action;
            GUIItemManager.AddGUIItem(button);
            return button;
        }
        protected ButtonMenuHalf AddButtonMenuHalf(string name, ButtonMenuHalfPosition pos, Action action)
        {
            ButtonMenuHalf button = new ButtonMenuHalf(GameClient, name, pos);
            button.OnButtonPressed += action;
            GUIItemManager.AddGUIItem(button);
            return button;
        }
        protected ButtonNavigation AddButtonNavigation(string name, ButtonNavigationPosition pos, Action action)
        {
            ButtonNavigation button = new ButtonNavigation(GameClient, name, pos);
            button.OnButtonPressed += action;
            GUIItemManager.AddGUIItem(button);
            return button;
        }
        protected InputBoxMenu AddInputBoxMenu(InputBoxMenuPosition pos)
        {
            InputBoxMenu inputBox = new InputBoxMenu(GameClient, pos);
            GUIItemManager.AddGUIItem(inputBox);
            return inputBox;
        }

        public virtual void LoadContent() { }
        public virtual void UnloadContent() { }
        public virtual void HandleInput(InputManager input) { }
        public virtual void Update(GameTime gameTime) { }
        public virtual void Draw(GameTime gameTime) { }
        public virtual void Dispose() { }

        public void ToActive() { ScreenState = ScreenState.JustNowActive;}
        public void ToBackground() { ScreenState = ScreenState.Background;}
        public void ToHidden() { ScreenState = ScreenState.Hidden;}

        protected void Exit() { GameClient.Exit(); }

        protected void ExitScreenAndClearButtons()
        {
            GUIItemManager.Clear();
            Dispose();
            ExitScreen();
        }

        protected virtual void ExitScreen()
        {
            // If the screen has a zero transition time, remove it immediately.
            ScreenManager.RemoveScreen(this);
        }
    }
}
