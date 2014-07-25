using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using MineLib.Network.BaseClients;
using Newtonsoft.Json;

namespace MineLib.GraphicClient.Screens
{
    public struct Address
    {
        [JsonProperty("ip")]
        public string IP { get; set; }

        [JsonProperty("port")]
        public short Port { get; set; }
    }

    public class Server
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("address")]
        public Address Address { get; set; }

        [JsonIgnore]
        public ResponseData ServerResponse { get; set; }
    }

    public abstract class InServerListScreen : Screen
    {
        protected SoundEffect ButtonEffect { get { return Content.Load<SoundEffect>("ButtonEffect"); } }

        protected Texture2D MainBackgroundTexture { get { return MinecraftTexturesStorage.GUITextures.OptionsBackground; } }

        Thread Parser;

        bool ParserIsBusy
        {
            get { return Parser != null && Parser.IsAlive; }
        }

        protected List<Server> Servers;

        private const string ServerListFileName = @"ServerList.mlmeta";
        private string ServerListPath { get { return Path.Combine(Content.RootDirectory, ServerListFileName); } }

        internal int NetworkProtocol = 5;

        protected void LoadServerList()
        {
            if (File.Exists(ServerListPath))
            {
                try
                {
                    Servers = JsonConvert.DeserializeObject<List<Server>>(File.ReadAllText(ServerListPath)) ?? new List<Server>();
                }
                catch (JsonReaderException)
                {
                    Servers = new List<Server>();
                }

            }
            else
                Servers = new List<Server>();
        }

        protected void SaveServerList(List<Server> server)
        {
            string json = JsonConvert.SerializeObject(server, Formatting.Indented);
            File.WriteAllText(ServerListPath, json);
        }

        protected void AddServerAndSaveServerList(Server server)
        {
            LoadServerList();

            List<Server> serverList = new List<Server>(Servers);
            serverList.Add(server);

            SaveServerList(serverList);

        }

        protected void ParseServerEntries()
        {
            if (ParserIsBusy)
                return;

            Parser = new Thread(() =>
            {
                if (Servers.Count > 0)
                {
                    ServerInfoParser ServerParser = new ServerInfoParser();

                    // Getting info for each saved server
                    foreach (Server server in Servers)
                    {
                        server.ServerResponse = ServerParser.GetResponseData(server.Address.IP, server.Address.Port, NetworkProtocol);
                    }
                }
            }) {Name = "Server Entries Parser"};
            Parser.Start();
        }
        
        protected static Address StringToAddress(string address)
        {
            //string serverAddress = ServerAddressInputBox.InputBoxText;
            string host, port;

            int colonIndex = address.IndexOf(':');
            if (colonIndex != -1)
            {
                port = address.Substring(colonIndex + 1);
                host = address.Substring(0, colonIndex);

            }
            else
            {
                port = "25565";
                host = address;
            }

            return new Address
            {
                IP = host,
                Port = Int16.Parse(port)
            };

        }
    }
}
