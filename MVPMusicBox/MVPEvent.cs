using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MVPMusicBox
{
    public class MVPList
    {
        public string UserId { get; set; }
        public List<string> MusicName { get; set; }
    }
    public class MVPMusic
    {
        public static void RegMusic()
        {
            if (!Directory.Exists($"{Paths.Exiled}\\MVPMusicBox\\Music"))
                Directory.CreateDirectory($"{Paths.Exiled}\\MVPMusicBox\\Music");
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
        private static List<MVPList> MVPInfos { get; set; } = new List<MVPList>();
        public static Dictionary<Player, int> Kills { get; set; } = new Dictionary<Player, int>();
        private static void RoundStart()
        {
            Dummy.Clear();
            Kills.Clear();
            foreach (Player player in Player.List)
                Kills.Add(player, 0);
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
            if (Args.Player == null || Args.Attacker == null || Args.Player == Args.Attacker)
                return;
            if (Args.Attacker.UserId == null || Args.Attacker.IsNPC)
                return;
            if (Kills.ContainsKey(Args.Attacker))
                Kills[Args.Attacker]++;
            else
                Kills.Add(Args.Attacker, 1);
        }
        public static void MusicBoxPlayer(Player player,int index = -1)
        {  
            MVPList mvpList = MVPInfos.FirstOrDefault(x => x.UserId == player.UserId);
            if (mvpList != null)
            {
                if (index == -1)
                    index = new Random().Next(0, mvpList.MusicName.Count);
                Map.Broadcast(10, $"<size=55%>=----------------=  <color=#FFF000>MVP 时刻</color>  =----------------=</size>\n<size=45%>本回合MVP: {player.Nickname} </size>\n<size=50%>总共击杀了 {Kills[player]} 人 </size>\n<size=55%>正在播放MVP音乐: 「 {mvpList.MusicName[index]} 」  </size>");
                Dummy.Add(123,$"{player.Nickname} 的音乐盒");
                Dummy.PlaySound(123, $"{Paths.Exiled}\\MVPMusicBox\\Music\\{player.UserId}", mvpList.MusicName[index]);
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
            foreach (Player player in Player.List.Where(x => !x.IsNPC))
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
    }
}
