using Exiled.API.Interfaces;
using System.ComponentModel;

namespace MVPMusicBox
{
    public class Config : IConfig
    {
        [Description("启动MVP音乐盒")]
        public bool IsEnabled { get; set ; } = true;
        [Description("启动MVP音乐盒-Debug")]
        public bool Debug { get; set; } = false;
        [Description("音乐大小")]
        public float Vol { get; set; } = 100f;
    }
}
