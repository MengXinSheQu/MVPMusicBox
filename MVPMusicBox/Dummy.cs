using Exiled.API.Features;
using Exiled.API.Features.Components;
using Exiled.Events.EventArgs.Player;
using Mirror;
using SCPSLAudioApi.AudioCore;
using System.Collections.Generic;
using UnityEngine;
using VoiceChat;

namespace MVPMusicBox
{ 
    public static class Dummy
    {
        public static Dictionary<int, Player> List { get; set; } = new Dictionary<int, Player>();
        public static void Clear()
        {
            foreach (Player player in List.Values)
            {
                var audiobase = AudioPlayerBase.Get(player.ReferenceHub);
                if (audiobase != null && audiobase.CurrentPlay != null)
                {
                    audiobase.Stoptrack(true);
                    audiobase.OnDestroy();
                }
            }
            foreach (int Id in List.Keys)
                NetworkServer.Destroy(List[Id].GameObject);
            List.Clear();
        }
        public static void PlaySound(int Id, string Paths, string MusicName, float Volume = 50f)
        {
            if (!List.ContainsKey(Id))
                Add(Id, "Bot");
            ReferenceHub component = List[Id].ReferenceHub;
            AudioPlayerBase audioPlayerBase = AudioPlayerBase.Get(component);
            string str = Paths + "\\" + MusicName + ".ogg";
            audioPlayerBase.Enqueue(str, -1);
            audioPlayerBase.LogDebug = false;
            audioPlayerBase.BroadcastChannel = VoiceChatChannel.Intercom;
            audioPlayerBase.Volume = Volume;
            audioPlayerBase.Loop = false;
            audioPlayerBase.Play(0);
        }
        public static void Add(int Id, string Name = "Bot")
        {
            if (List.ContainsKey(Id))
                return; 
            GameObject player1 = Object.Instantiate(NetworkManager.singleton.playerPrefab);
            NetworkServer.AddPlayerForConnection(new FakeConnection(Id), player1);
            ReferenceHub component = player1.GetComponent<ReferenceHub>();
            Player player = Player.Get(component);
            player.ReferenceHub.nicknameSync.Network_myNickSync = Name;
            List.Add(Id, player);
        }
        private static void Fixer(BanningEventArgs ev)
        {
            if (List.ContainsValue(ev.Target) || ev.Target.IsHost)
                ev.IsAllowed = false;
        }
        private static void Fixer(KickingEventArgs ev)
        {
            if (List.ContainsValue(ev.Target) || ev.Target.IsHost)
                ev.IsAllowed = false;
        }
        private static void Fixer() => Clear(); 
        public static void Reg()
        {
            Exiled.Events.Handlers.Player.Banning += Fixer;
            Exiled.Events.Handlers.Player.Kicking += Fixer;
            Exiled.Events.Handlers.Server.RestartingRound += Fixer;
        }
        public static void UnReg()
        {
            Exiled.Events.Handlers.Player.Banning -= Fixer;
            Exiled.Events.Handlers.Player.Kicking -= Fixer;
            Exiled.Events.Handlers.Server.RestartingRound -= Fixer;
        }
    }
}
