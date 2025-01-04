using Exiled.API.Features;
using Mirror;
using PlayerRoles;
using SCPSLAudioApi.AudioCore;
using System.Collections.Generic;
using UnityEngine;
using VoiceChat;

namespace MVPMusicBox.MusicAPI
{
    public static class PlayMusic
    {
        public static List<WarppedAudio> audios = new List<WarppedAudio>();
        public static AudioPlayerBase GlobalPlayMusic(string Path,string musicname,string Name = "Unknown")
        {
            GameObject player1 = Object.Instantiate(NetworkManager.singleton.playerPrefab);
            int networkConnectionId = new System.Random().Next(250, 301);
            NetworkServer.AddPlayerForConnection(new FakeConnection(networkConnectionId), player1);
            ReferenceHub component = player1.GetComponent<ReferenceHub>();
            Player player = Player.Get(component);
            player.DisplayNickname = Name;
            AudioPlayerBase audioPlayerBase = AudioPlayerBase.Get(component);
            string str = Path + "\\" + musicname + ".ogg";
            audioPlayerBase.Enqueue(str, -1);
            audioPlayerBase.LogDebug = false;
            audioPlayerBase.Volume = 50;
            audioPlayerBase.BroadcastChannel = VoiceChatChannel.Intercom;
            audioPlayerBase.Loop = false;
            audioPlayerBase.Play(-1);
            component.roleManager.InitializeNewRole(RoleTypeId.Overwatch, RoleChangeReason.RemoteAdmin);
            audios.Add(new WarppedAudio()
            {
                Player = audioPlayerBase,
                Music = musicname,
                Verfiy = $"{musicname}-{networkConnectionId}@server"
            });
            return audioPlayerBase;
        }
    }
}
