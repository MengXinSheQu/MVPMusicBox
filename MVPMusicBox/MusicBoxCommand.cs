using CommandSystem;
using Exiled.API.Features;
using RemoteAdmin;
using System;

namespace MVPMusicBox
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class MusicBoxCommand : ICommand
    {
        public Player admin;
        public string Command { get; } = "musicbox";
        public string[] Aliases { get; } = new string[1] { "musicbox" };
        public string Description { get; } = "调试音乐盒 格式:musicbox [玩家Id(不填默认自己)]";
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (sender is PlayerCommandSender playerCommandSender)
            {
                admin = Player.Get(playerCommandSender.PlayerId);
                if (arguments.Count < 1)
                {
                    
                    MVPMusic.MusicBoxPlayer(admin);
                    Log.Info($"[管理员][{admin.Nickname}][{admin.UserId}][调试音乐盒]");
                    response = "信息已发送!";
                    return true;
                }
                if (arguments.Count >= 1) 
                {
                    try 
                    {
                        Player player = Player.Get(arguments.At(0));
                        if (player != null && player.Sender != null)
                        {
                            MVPMusic.MusicBoxPlayer(player);
                            Log.Info($"[管理员][{admin.Nickname}][{admin.UserId}] 调试了玩家 {player.Nickname} 的音乐盒");
                        }
                    }
                    catch 
                    {
                        response = "<color=red>无法获得玩家！</color>";
                        return false;
                    }
                }

            }
            response = "信息已发送!";
            return true;
        }
    }
}
