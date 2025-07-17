using AudioApi.Dummies;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using MVPMusicBox.API;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MVPMusicBox
{
    public class MVPEvent
    {
        internal static MVPEvent Instance { get; } = new();
        public void RegEvent()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers += OnWaittingForPlayers;
            Exiled.Events.Handlers.Server.RoundEnded += OnRoundEnded;
            Exiled.Events.Handlers.Player.Dying += OnDying;
            Exiled.Events.Handlers.Player.Spawned += OnSpawned;
            Exiled.Events.Handlers.Server.RestartingRound += OnRestaringRound;
        }
        public void UnRegEvent()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers -= OnWaittingForPlayers;
            Exiled.Events.Handlers.Server.RoundEnded -= OnRoundEnded;
            Exiled.Events.Handlers.Player.Dying -= OnDying;
            Exiled.Events.Handlers.Player.Spawned -= OnSpawned;
            Exiled.Events.Handlers.Server.RestartingRound -= OnRestaringRound;
        }
        private static List<MvpInfomation> MVPInfos { get; set; } = [];
        private static Dictionary<Player, int> Kills { get; set; } = [];
        private void OnWaittingForPlayers()
        {
            VoiceDummy.Remove(123);
            Kills.Clear();
            ReadJson();
        }
        private void OnRestaringRound() => VoiceDummy.Remove(123);
        private void OnSpawned(SpawnedEventArgs ev)
        {
            if (ev.Player.IsNPC)
                return;
            if (!Kills.ContainsKey(ev.Player))
                Kills.Add(ev.Player, 0);
        }
        private void OnDying(DyingEventArgs ev)
        {
            if (ev.Player.IsNPC)
                return;
            if (ev.Player == null || ev.Attacker == null || ev.Player == ev.Attacker)
                return;
            if (Kills.ContainsKey(ev.Attacker))
                Kills[ev.Attacker]++;
            else
                Kills[ev.Attacker] = 0;
        }
        private void OnRoundEnded(RoundEndedEventArgs __)
        {
            Map.ClearBroadcasts();
            VoiceDummy.Clear();
            ReadJson();
            foreach (Player player in Player.List.Where(x => !x.IsNPC))
            {
                if (!Kills.ContainsKey(player))
                    Kills[player] = 0;
            }
            var p = Kills;
            SendingMvp(p.OrderByDescending(x => x.Value).FirstOrDefault().Key);
        }
        private static void ReadJson()
        {
            if (!File.Exists($@"{APIPaths.音乐盒}\Config.json"))
            {
                MVPInfos.Add(new() { UserId = "765611xxxxxxxx@steam", BroadcastName = "我的炫酷音乐", MusicName = "音乐文件名称 不要加后缀名!" });
                File.WriteAllText($@"{APIPaths.音乐盒}\Configjson", JsonConvert.SerializeObject(MVPInfos));
            }
            else
                MVPInfos = JsonConvert.DeserializeObject<List<MvpInfomation>>(File.ReadAllText($@"{APIPaths.音乐盒}\Config.json"));
        }
        internal static void SendingMvp(Player player)
        {
            if (player == null)
            {
                Map.Broadcast(10, $"<size=55%>=----------------=  <color=#FFF000>MVP 时刻</color>  =----------------=</size>\n<size=45%>本回合没有人是MVP</size>");
            }
            else
            {
                Map.ClearBroadcasts();
                MvpInfomation mvpList = MVPInfos.FirstOrDefault(x => x.UserId == player.UserId);
                if (mvpList != null)
                {
                    Log.Info($"[MVP][玩家 {player.Nickname}][击杀 {Kills[player]} 人][播放音乐 {mvpList.MusicName} ][播报名 {mvpList.BroadcastName}]");
                    Map.Broadcast(10, $"<size=55%>=----------------=  <color=#FFF000>MVP 时刻</color>  =----------------=</size>\n<size=45%>本回合MVP: {player.Nickname} </size>\n<size=50%>总共击杀了 {Kills[player]} 人 </size>\n<size=55%>正在播放MVP音乐: 「 {mvpList.BroadcastName} 」  </size>");
                    VoiceDummy.Add(123, $"{player.Nickname} 的音乐盒");
                    VoiceDummy.Play(123, APIPaths.音乐盒, mvpList.MusicName);
                    return;
                }
                else
                {
                    Log.Info($"[MVP][玩家 {player.Nickname}][击杀 {Kills[player]} 人]");
                    Map.Broadcast(10, $"<size=55%>=----------------=  <color=#FFF000>MVP 时刻</color>  =----------------=</size>\n<size=45%>本回合MVP: {player.Nickname} </size>\n<size=50%>总共击杀了 {Kills[player]} 人 </size>");
                    return;
                }
            }
        }
    }
}
