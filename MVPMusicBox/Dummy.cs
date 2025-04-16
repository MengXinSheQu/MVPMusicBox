using Exiled.API.Features;
using System.Collections.Generic;

namespace MVPMusicBox
{ 
    public static class Dummy
    {
        public static Dictionary<int, AudioPlayer> List { get; set; } = new Dictionary<int, AudioPlayer>();
        public static void Clear()
        {
            foreach (AudioPlayer player in List.Values)
                player.Destroy();
            List.Clear();
        }
        public static void PlaySound(int Id, string Paths, string MusicName)
        {
            if (!List.ContainsKey(Id))
                Add(Id, "Bot");
            Log.Info($"播放音乐[{MusicName}]");
            AudioClipStorage.LoadClip(Paths + "\\" + MusicName + ".ogg", MusicName);
            var player = List[Id];
            player.AddClip(MusicName, volume: Plugin.Instance.Config.Vol);
        }
        public static void Add(int Id, string Name = "Bot")
        {
            if (List.ContainsKey(Id))
                return;
            Log.Info($"添加 [{Id}-{Name}]");
            AudioPlayer player = AudioPlayer.CreateOrGet(Name, onIntialCreation: p =>
            {
                p.AddSpeaker(Name, isSpatial: false, maxDistance: 5000f);
            });
            List.Add(Id, player);
        }
    }
}
