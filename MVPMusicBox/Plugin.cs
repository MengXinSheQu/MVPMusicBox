using Exiled.API.Features;
using MVPMusicBox.API;
using System.IO;

namespace MVPMusicBox
{
    public class Plugin : Plugin<Config>
    {
        public static Plugin Instance { get; set; }
        public override string Author { get; } = "萌新社区开发团队";
        public override string Name { get; } = "MVPMusicBox";
        public override void OnEnabled()
        {
            if (!Directory.Exists(APIPaths.音乐盒))
                Directory.CreateDirectory(APIPaths.音乐盒);
            if (!Directory.Exists(APIPaths.音乐盒音乐文件))
                Directory.CreateDirectory(APIPaths.音乐盒音乐文件);
            Instance = this;
            MVPEvent.Instance.RegEvent();
        }
        public override void OnDisabled()
        {
            MVPEvent.Instance.UnRegEvent();
            Instance = null;
        }
    }
}
