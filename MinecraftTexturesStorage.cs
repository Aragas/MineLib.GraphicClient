﻿using System.IO;
using Ionic.Zip;
using Microsoft.Xna.Framework.Graphics;

namespace MineLib.GraphicClient
{
    public struct GUITextures
    {
        // Background folder.
        public Texture2D Panorama0;
        public Texture2D Panorama1;
        public Texture2D Panorama2;
        public Texture2D Panorama3;
        public Texture2D Panorama4;
        public Texture2D Panorama5;

        // Title folder.
        public Texture2D Mojang;
        public Texture2D Minecraft;

        // CreativeInventory folder.
        public Texture2D Tabs;
        public Texture2D TabItems;
        public Texture2D TabItemSearch;
        public Texture2D TabInventory;

        // Container folder.
        public Texture2D Villager;
        public Texture2D StatsIcons;
        public Texture2D Inventory;
        public Texture2D Horse;
        public Texture2D Hopper;
        public Texture2D Generic54;
        public Texture2D Furnace;
        public Texture2D EnchantingTable;
        public Texture2D Dispenser;
        public Texture2D CraftingTable;
        public Texture2D BrewingStand;
        public Texture2D Beacon;
        public Texture2D Anvil;

        // Achievement folder.
        public Texture2D AchievementIcons;
        public Texture2D AchievementBackground;

        // -- Main folder.
        public Texture2D Widgets;
        public Texture2D StreamIndicator;
        public Texture2D ResourcePacks;
        public Texture2D OptionsBackground;
        public Texture2D Icons;
        public Texture2D DemoBackground;
        public Texture2D Book;
    }

    public class MinecraftTexturesStorage
    {
        GameClient Client;
        ZipFile Minecraft;

        public GUITextures GUITextures;

        public MinecraftTexturesStorage(GameClient client,ZipFile minecraft)
        {
            Client = client;
            Minecraft = minecraft;
        }

        public GUITextures GetGUITextures()
        {
            GUITextures =  new GUITextures();
            MemoryStream ms = new MemoryStream();
            string selectCriteria = "name = assets/minecraft/textures/gui/*";
            foreach (ZipEntry entry in Minecraft.SelectEntries(selectCriteria))
            {
                switch (entry.FileName)
                {
                    #region Background folder.
                    case "assets/minecraft/textures/gui/title/background/panorama_0.png":
                        ms = new MemoryStream();
                        entry.Extract(ms);
                        GUITextures.Panorama0 = Texture2D.FromStream(Client.GraphicsDevice, ms);
                        break;

                    case "assets/minecraft/textures/gui/title/background/panorama_1.png":
                        ms = new MemoryStream();
                        entry.Extract(ms);
                        GUITextures.Panorama1 = Texture2D.FromStream(Client.GraphicsDevice, ms);
                        break;

                    case "assets/minecraft/textures/gui/title/background/panorama_2.png":
                        ms = new MemoryStream();
                        entry.Extract(ms);
                        GUITextures.Panorama2 = Texture2D.FromStream(Client.GraphicsDevice, ms);
                        break;

                    case "assets/minecraft/textures/gui/title/background/panorama_3.png":
                        ms = new MemoryStream();
                        entry.Extract(ms);
                        GUITextures.Panorama3 = Texture2D.FromStream(Client.GraphicsDevice, ms);
                        break;

                    case "assets/minecraft/textures/gui/title/background/panorama_4.png":
                        ms = new MemoryStream();
                        entry.Extract(ms);
                        GUITextures.Panorama4 = Texture2D.FromStream(Client.GraphicsDevice, ms);
                        break;

                    case "assets/minecraft/textures/gui/title/background/panorama_5.png":
                        ms = new MemoryStream();
                        entry.Extract(ms);
                        GUITextures.Panorama5 = Texture2D.FromStream(Client.GraphicsDevice, ms);
                        break;
                    #endregion

                    #region Title folder.
                    case "assets/minecraft/textures/gui/title/mojang.png":
                        ms = new MemoryStream();
                        entry.Extract(ms);
                        GUITextures.Mojang = Texture2D.FromStream(Client.GraphicsDevice, ms);
                        break;

                    case "assets/minecraft/textures/gui/title/minecraft.png":
                        ms = new MemoryStream();
                        entry.Extract(ms);
                        GUITextures.Minecraft = Texture2D.FromStream(Client.GraphicsDevice, ms);
                        break;
                    #endregion

                    #region CreativeInventory folder.
                    case "assets/minecraft/textures/gui/container/creative_inventory/tabs.png":
                        ms = new MemoryStream();
                        entry.Extract(ms);
                        GUITextures.Tabs = Texture2D.FromStream(Client.GraphicsDevice, ms);
                        break;

                    case "assets/minecraft/textures/gui/container/creative_inventory/tab_items.png":
                        ms = new MemoryStream();
                        entry.Extract(ms);
                        GUITextures.TabItems = Texture2D.FromStream(Client.GraphicsDevice, ms);
                        break;

                    case "assets/minecraft/textures/gui/container/creative_inventory/tab_item_search.png":
                        ms = new MemoryStream();
                        entry.Extract(ms);
                        GUITextures.TabItemSearch = Texture2D.FromStream(Client.GraphicsDevice, ms);
                        break;

                    case "assets/minecraft/textures/gui/container/creative_inventory/tab_inventory.png":
                        ms = new MemoryStream();
                        entry.Extract(ms);
                        GUITextures.TabInventory = Texture2D.FromStream(Client.GraphicsDevice, ms);
                        break;
                    #endregion

                    #region Container folder.
                    case "assets/minecraft/textures/gui/container/villager.png":
                        ms = new MemoryStream();
                        entry.Extract(ms);
                        GUITextures.Villager = Texture2D.FromStream(Client.GraphicsDevice, ms);
                        break;

                    case "assets/minecraft/textures/gui/container/stats_icons.png":
                        ms = new MemoryStream();
                        entry.Extract(ms);
                        GUITextures.StatsIcons = Texture2D.FromStream(Client.GraphicsDevice, ms);
                        break;

                    case "assets/minecraft/textures/gui/container/inventory.png":
                        ms = new MemoryStream();
                        entry.Extract(ms);
                        GUITextures.Inventory = Texture2D.FromStream(Client.GraphicsDevice, ms);
                        break;

                    case "assets/minecraft/textures/gui/container/horse.png":
                        ms = new MemoryStream();
                        entry.Extract(ms);
                        GUITextures.Horse = Texture2D.FromStream(Client.GraphicsDevice, ms);
                        break;

                    case "assets/minecraft/textures/gui/container/hopper.png":
                        ms = new MemoryStream();
                        entry.Extract(ms);
                        GUITextures.Hopper = Texture2D.FromStream(Client.GraphicsDevice, ms);
                        break;

                    case "assets/minecraft/textures/gui/container/generic_54.png":
                        ms = new MemoryStream();
                        entry.Extract(ms);
                        GUITextures.Generic54 = Texture2D.FromStream(Client.GraphicsDevice, ms);
                        break;

                    case "assets/minecraft/textures/gui/container/furnace.png":
                        ms = new MemoryStream();
                        entry.Extract(ms);
                        GUITextures.Furnace = Texture2D.FromStream(Client.GraphicsDevice, ms);
                        break;

                    case "assets/minecraft/textures/gui/container/enchanting_table.png":
                        ms = new MemoryStream();
                        entry.Extract(ms);
                        GUITextures.EnchantingTable = Texture2D.FromStream(Client.GraphicsDevice, ms);
                        break;

                    case "assets/minecraft/textures/gui/container/dispenser.png":
                        ms = new MemoryStream();
                        entry.Extract(ms);
                        GUITextures.Dispenser = Texture2D.FromStream(Client.GraphicsDevice, ms);
                        break;

                    case "assets/minecraft/textures/gui/container/crafting_table.png":
                        ms = new MemoryStream();
                        entry.Extract(ms);
                        GUITextures.CraftingTable = Texture2D.FromStream(Client.GraphicsDevice, ms);
                        break;

                    case "assets/minecraft/textures/gui/container/brewing_stand.png":
                        ms = new MemoryStream();
                        entry.Extract(ms);
                        GUITextures.BrewingStand = Texture2D.FromStream(Client.GraphicsDevice, ms);
                        break;

                    case "assets/minecraft/textures/gui/container/beacon.png":
                        ms = new MemoryStream();
                        entry.Extract(ms);
                        GUITextures.Beacon = Texture2D.FromStream(Client.GraphicsDevice, ms);
                        break;

                    case "assets/minecraft/textures/gui/container/anvil.png":
                        ms = new MemoryStream();
                        entry.Extract(ms);
                        GUITextures.Anvil = Texture2D.FromStream(Client.GraphicsDevice, ms);
                        break;
                    #endregion

                    #region Achievement folder.
                    case "assets/minecraft/textures/gui/achievement/achievement_icons.png":
                        ms = new MemoryStream();
                        entry.Extract(ms);
                        GUITextures.AchievementIcons = Texture2D.FromStream(Client.GraphicsDevice, ms);
                        break;

                    case "assets/minecraft/textures/gui/achievement/achievement_background.png":
                        ms = new MemoryStream();
                        entry.Extract(ms);
                        GUITextures.AchievementBackground = Texture2D.FromStream(Client.GraphicsDevice, ms);
                        break;
                    #endregion

                    #region Main folder.
                    case "assets/minecraft/textures/gui/widgets.png":
                        ms = new MemoryStream();
                        entry.Extract(ms);
                        GUITextures.Widgets = Texture2D.FromStream(Client.GraphicsDevice, ms);
                        break;

                    case "assets/minecraft/textures/gui/stream_indicator.png":
                        ms = new MemoryStream();
                        entry.Extract(ms);
                        GUITextures.StreamIndicator = Texture2D.FromStream(Client.GraphicsDevice, ms);
                        break;

                    case "assets/minecraft/textures/gui/resource_packs.png":
                        ms = new MemoryStream();
                        entry.Extract(ms);
                        GUITextures.ResourcePacks = Texture2D.FromStream(Client.GraphicsDevice, ms);
                        break;

                    case "assets/minecraft/textures/gui/options_background.png":
                        ms = new MemoryStream();
                        entry.Extract(ms);
                        GUITextures.OptionsBackground = Texture2D.FromStream(Client.GraphicsDevice, ms);
                        break;

                    case "assets/minecraft/textures/gui/icons.png":
                        ms = new MemoryStream();
                        entry.Extract(ms);
                        GUITextures.Icons = Texture2D.FromStream(Client.GraphicsDevice, ms);
                        break;

                    case "assets/minecraft/textures/gui/demo_background.png":
                        ms = new MemoryStream();
                        entry.Extract(ms);
                        GUITextures.DemoBackground = Texture2D.FromStream(Client.GraphicsDevice, ms);
                        break;

                    case "assets/minecraft/textures/gui/book.png":
                        ms = new MemoryStream();
                        entry.Extract(ms);
                        GUITextures.Book = Texture2D.FromStream(Client.GraphicsDevice, ms);
                        break;
                    #endregion
                }
            }
            return GUITextures;
        }
    }
}
