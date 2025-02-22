using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MVPMusicBox
{
    public class MVPList
    {
        public string UserId { get; set; }
        public string MusicName { get; set; }
        public string BroadcastName { get; set; }
    }
    public class MVPMusic
    {
        public static void RegMusic()
        {
            if (!Directory.Exists($"{Paths.Exiled}\\MVPMusicBox\\Music"))
                Directory.CreateDirectory($"{Paths.Exiled}\\MVPMusicBox\\Music");
            ReadJson();
            Exiled.Events.Handlers.Server.RoundStarted += RoundStart;
            Exiled.Events.Handlers.Server.RoundEnded += RoundEnded;
            Exiled.Events.Handlers.Player.Dying += PlayerDying;
            Exiled.Events.Handlers.Player.Spawned += PlayerSpawn;
            Dummy.Reg();
        }
        public static void UnRegMusic()
        {
            Exiled.Events.Handlers.Server.RoundStarted -= RoundStart;
            Exiled.Events.Handlers.Server.RoundEnded -= RoundEnded;
            Exiled.Events.Handlers.Player.Dying -= PlayerDying;
            Exiled.Events.Handlers.Player.Spawned -= PlayerSpawn;
            Dummy.UnReg();
        }
        private static List<MVPList> MVPInfos { get; set; } = new List<MVPList>();
        public static Dictionary<Player, int> Kills { get; set; } = new Dictionary<Player, int>();
        private static void RoundStart()
        {
            Dummy.Clear();
            Kills.Clear();
            foreach (Player player in Player.List)
                Kills.Add(player, 0);
            ReadJson();
        }
        private static void PlayerSpawn(SpawnedEventArgs Args)
        {
            if (Args.Player.UserId == null || Args.Player.IsNPC)
                return;
            if (!Player.List.Contains(Args.Player))
                return;
            if (!Kills.ContainsKey(Args.Player) && Player.List.Contains(Args.Player))
                Kills.Add(Args.Player, 0);
        }
        private static void PlayerDying(DyingEventArgs Args)
        {
            if (!Player.List.Contains(Args.Attacker))
                return;
            if (Args.Player.UserId == null || Args.Player.IsNPC)
                return;
            if (Args.Player == null || Args.Attacker == null || Args.Player == Args.Attacker)
                return;
            if (Kills.ContainsKey(Args.Attacker))
                Kills[Args.Attacker]++;
            else
                Kills.Add(Args.Attacker, 1);
        }
        public static void MusicBoxPlayer(Player player)
        {  
            MVPList mvpList = MVPInfos.FirstOrDefault(x => x.UserId == player.UserId);
            if (mvpList != null)
            {
                Map.Broadcast(10, $"<size=55%>=----------------=  <color=#FFF000>MVP 时刻</color>  =----------------=</size>\n<size=45%>本回合MVP: {player.Nickname} </size>\n<size=50%>总共击杀了 {Kills[player]} 人 </size>\n<size=55%>正在播放MVP音乐: 「 {mvpList.BroadcastName} 」  </size>");
                Dummy.Add(123,$"{player.Nickname} 的音乐盒");
                Dummy.PlaySound(123, Paths.Exiled + "\\MVPMusicBox\\Music", mvpList.MusicName);
                return;
            }
            else
            {
                Map.Broadcast(10, $"<size=55%>=----------------=  <color=#FFF000>MVP 时刻</color>  =----------------=</size>\n<size=45%>本回合MVP: {player.Nickname} </size>\n<size=50%>总共击杀了 {Kills[player]} 人 </size>");
                return;
            }
        }
        private static void RoundEnded(RoundEndedEventArgs ev)
        {
            Map.ClearBroadcasts();
            Dummy.Clear();
            ReadJson();
            foreach (Player player in Player.List)
            {
                if (!Kills.ContainsKey(player))
                    Kills.Add(player, 0);
            }
            try
            {
                foreach (Player player in Kills.Keys)
                {
                    if (!Player.List.Contains(player))
                    {
                        Kills.Remove(player);
                    }
                }
            }
            catch 
            {
            
            }
            MusicBoxPlayer(Kills.OrderByDescending(x => x.Value).First().Key);
        }
        private static void ReadJson()
        {
            Server.MaxPlayerCount += 1;
            string JsonPath = $"{Paths.Exiled}\\MVPMusicBox\\Config.json";
            if (!File.Exists(JsonPath))
            {
                MVPInfos.Add(new MVPList
                {
                    UserId = "765611xxxxxxx@steam",
                    BroadcastName = "酷炫的音乐",
                    MusicName = "MyMusic"
                });
                File.WriteAllText(JsonPath, JsonConvert.SerializeObject(MVPInfos));
                Log.Info($"生成音乐盒模板至 [{JsonPath}]");
            }
            else
            {
                MVPInfos = JsonConvert.DeserializeObject<List<MVPList>>(File.ReadAllText(JsonPath));
            }
        }
    }
}
