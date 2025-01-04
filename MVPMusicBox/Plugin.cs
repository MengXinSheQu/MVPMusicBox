using Exiled.API.Features;

namespace MVPMusicBox
{
    public class Plugin : Plugin<Config>
    {
        public override string Author { get; } = "萌新社区服务器";
        public override string Name { get; } = "MVPMusicBox";
        public override void OnEnabled()
        {
            Log.Info("加载MVP音乐盒插件! By 萌新社区服务器");
            EventHandlers.MVPMusic.RegMusic();
            base.OnEnabled();
        }
        public override void OnDisabled()
        {
            EventHandlers.MVPMusic.UnRegMusic();
            base.OnDisabled();
        }
    }
}
