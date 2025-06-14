using CommandSystem;
using Exiled.API.Features;
using System;

namespace MVPMusicBox.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class Musicbox : ICommand,IUsageProvider
    {
        public Player admin;
        public string Command => "musicbox";
        public string[] Aliases => [ "musicbox" ];
        public string Description => "调试音乐盒";
        public string[] Usage =>[ "Id","Index"];
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            admin = Player.Get(sender);
            if (arguments.Count < 1)
            {

                MVPEvent.SendingMvp(admin);
                Log.Info($"[管理员][{admin.Nickname}][{admin.UserId}] 调试了玩家 {admin.Nickname} 的音乐盒");
                response = "信息已发送!";
                return true;
            }
            else if (arguments.Count >= 1)
            {
                try
                {
                    Player player = Player.Get(arguments.At(0));
                    if (player != null && player.Sender != null)
                    {
                        MVPEvent.SendingMvp(player);
                        Log.Info($"[管理员][{admin.Nickname}][{admin.UserId}] 调试了玩家 {player.Nickname} 的音乐盒");
                    }
                }
                catch
                {
                    response = "<color=red>无法获得玩家！</color>";
                    return false;
                }
            }
            response = "<color=green>信息已发送!</color>";
            return true;
        }
    }
}
