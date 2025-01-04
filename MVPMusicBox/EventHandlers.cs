using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MVPMusicBox
{
    public class EventHandlers
    {
        public class MVPList
        {
            public string UserId { get; set; }
            public string MusicName { get; set; }
            public string BroadcastName { get; set; }
        }
        public class MVPMusic
        {
            public static bool MusicBox = true;
            public static List<MVPList> MVPInfos = new List<MVPList>();
            public static Dictionary<Player, int> Kills = new Dictionary<Player, int>();
            public static void RegMusic()
            {
                ReadJson();
                Exiled.Events.Handlers.Server.RoundStarted += RoundStart;
                Exiled.Events.Handlers.Server.RoundEnded += RoundEnded;
                Exiled.Events.Handlers.Player.Dying += PlayerDying;
                Exiled.Events.Handlers.Player.Spawned += PlayerSpawn;
            }
            public static void UnRegMusic()
            {
                Exiled.Events.Handlers.Server.RoundStarted -= RoundStart;
                Exiled.Events.Handlers.Server.RoundEnded -= RoundEnded;
                Exiled.Events.Handlers.Player.Dying -= PlayerDying;
                Exiled.Events.Handlers.Player.Spawned -= PlayerSpawn;
            }
            public static void RoundStart()
            {
                foreach (Player player in Player.List)
                {
                    Kills[player] = 0;
                }
                Kills.Clear();
                ReadJson();
            }
            public static void PlayerSpawn(SpawnedEventArgs Args)
            {
                if (!Kills.ContainsKey(Args.Player) && Args.Player.IsVerified && !Args.Player.IsNPC)
                {
                    Kills.Add(Args.Player, 0);
                }
            }
            public static void PlayerDying(DyingEventArgs Args)
            {
                if (Args.Player != null && Args.Attacker != null)
                {
                    if (Args.Player != Args.Attacker)
                    {
                        if (Kills.ContainsKey(Args.Attacker))
                        {
                            Kills[Args.Attacker]++;
                        }
                        else
                        {
                            Kills[Args.Attacker] = 1;
                        }
                    }
                }
            }
            public static void MusicBoxPlayer(Player player)
            {
                MVPList mvpList = MVPInfos.FirstOrDefault(x => x.UserId == player.UserId);
                if (mvpList != null)
                {
                    Log.Info($"[MVP][玩家 {player.Nickname}][击杀 {Kills[player]} 人][播放音乐 {mvpList.MusicName} ][播报名 {mvpList.BroadcastName}]");
                    Map.Broadcast(10, $"<size=55%>=----------------=  <color=#FFF000>MVP 时刻</color>  =----------------=</size>\n<size=45%>本回合MVP: {player.Nickname} </size>\n<size=50%>总共击杀了 {Kills[player]} 人 </size>\n<size=55%>正在播放MVP音乐: 「 {mvpList.BroadcastName} 」  </size>");
                    MusicAPI.PlayMusic.GlobalPlayMusic(Paths.Exiled + "\\音乐盒\\音乐文件", mvpList.MusicName, $"{player.Nickname}的音乐盒");
                    return;
                }
                else
                {
                    Log.Info($"[MVP][玩家 {player.Nickname}][击杀 {Kills[player]} 人]");
                    Map.Broadcast(10, $"<size=55%>=----------------=  <color=#FFF000>MVP 时刻</color>  =----------------=</size>\n<size=45%>本回合MVP: {player.Nickname} </size>\n<size=50%>总共击杀了 {Kills[player]} 人 </size>");
                    return;
                }
            }
            public static void RoundEnded(RoundEndedEventArgs ev)
            {
                if (MusicBox == false)
                    return;
                MusicBox = false;
                ReadJson();
                foreach (Player player in Player.List)
                    if (!Kills.ContainsKey(player))
                        Kills.Add(player, 0);
                MusicBoxPlayer(Kills.OrderByDescending(x => x.Value).First().Key);
            }
            public static void ReadJson()
            {
                string JsonPath = Paths.Exiled + "\\音乐盒\\Config.json";
                if (!Directory.Exists(Paths.Exiled + "\\音乐盒"))
                {
                    Directory.CreateDirectory(Paths.Exiled + "\\音乐盒");
                }
                if (!Directory.Exists(Paths.Exiled + "\\音乐盒\\音乐文件"))
                {
                    Directory.CreateDirectory(Paths.Exiled + "\\音乐盒\\音乐文件");
                }
                if (!File.Exists(JsonPath))
                {
                    MVPInfos.Add(new MVPList()
                    {
                        UserId = "76561199208302036@steam",
                        MusicName = "76561199208302036",
                        BroadcastName = "[DEBUG]",
                    });
                    File.WriteAllText(JsonPath, JsonConvert.SerializeObject((object)MVPInfos));
                    Log.Info($"生成音乐盒模板至 => [{JsonPath}]");
                }
                else
                {
                    MVPInfos = JsonConvert.DeserializeObject<List<MVPList>>(File.ReadAllText(JsonPath));
                    Log.Info($"读取音乐盒数量：{MVPInfos.Count}");
                }
            }
        }
    }
}
