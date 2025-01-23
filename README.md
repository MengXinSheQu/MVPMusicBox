English introduce:
#MVPMusicBox - MVP Music Box Plugin 🎵

##* * Introduction:**

This plugin has added MVP music box 🎵。

When the total number of player kills (excluding suicide) reaches the first turn, the player MVP music box will be played.

If the player does not have an MVP music box, only information will be displayed without playing or prompting.

##* * Usage:**
e
Extract 'dependencies exported. rar' to 'EXILED \ plugins \ diplomacies'

Then place 'MVPMusicBox. dell' into 'EXILED \ Plugins'

The server will load the plugin

##JSON configuration tutorial:**

When the plugin is loaded, it will generate a 'Music Box' folder in the EXILED directory 📂

A 'Config. json' file will be generated in this folder 📄 And the 'Music Files' folder 📂

`Config. json 'is used to store player music box configuration files 📄

`Put music files inside the music file 🎵

`Config. json ` configuration example:

```
[
{
"UserId": "[UserId]", // Player ends with a 64 digit number and adds @ team
"MusicName": "MyMusic", // The music name loaded from the 'Music Files' folder does not require a suffix
BroadcastName ":" [DEBUG] "//The name of the in-game prompt, for example: [DEBUG] is currently playing
}
]
```
During turn initiation and plugin loading, the plugin will perform a JSON read once


##* * Music file requirements:**

The music should be Ogg audio with a single track 48000 sampling rate

The time should be within 10 seconds

中文介绍：
# MVPMusicBox - MVP音乐盒插件🎵

## **介绍:**

该插件添加了MVP音乐盒🎵。

当玩家击杀总数(不包括自杀)为回合第一时,会播放玩家MVP音乐盒。

如果玩家没有MVP音乐盒,则仅显示信息而不会进行播放与提示。

## **使用:**
e
将`dependencies-exiled.rar`解压至`EXILED\Plugins\dependencies`内

然后将`MVPMusicBox.dll`放至`EXILED\Plugins`内

服务器就会加载该插件了

## **JSON配置教程:**

插件进行加载时会在EXILED目录生成`音乐盒`文件夹📂

在该文件夹内会生成`Config.json`文件📄和`音乐文件`文件夹📂

`Config.json`用于储存玩家音乐盒配置文件📄

`音乐文件`内放入音乐文件🎵

`Config.json`配置示例:

```
[
  {
    "UserId": "[UserId]", //玩家64位 数字结尾加上@steam
    "MusicName": "MyMusic", //从'音乐文件'文件夹加载的音乐名称,不需要后缀名
    "BroadcastName": "[DEBUG]" //游戏内提示的名称,例如:正在播放[DEBUG]
  }
]
```
在回合开启和插件加载时插件会进行一次Json读取


## **音乐文件要求:**

音乐应为单轨道 48000采样率的ogg音频

时间应在10秒内
