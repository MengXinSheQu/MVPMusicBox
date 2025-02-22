using Exiled.API.Features;

namespace MVPMusicBox
{
    public class Plugin : Plugin<Config>
    {
        public override string Author { get; } = "萌新社区服务器";
        public override string Name { get; } = "MVPMusicBox";
        public override void OnEnabled()
        {
            MVPMusic.RegMusic();
        }
        public override void OnDisabled()
        {
            MVPMusic.UnRegMusic();
        }
    }
}
