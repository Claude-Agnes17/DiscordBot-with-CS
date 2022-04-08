using System;
using System.Timers;
using System.Threading.Tasks;
using System.Xml;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Collections.Generic;


namespace Discord.Commands
{
    // 모듈 클래스의 접근제어자가 public이면서 ModuleBase를 상속해야 모듈이 인식된다.
    public class BasicModule : ModuleBase<SocketCommandContext>
    {
        [Command("핑")]
        public async Task PingCommand()
        {
            await Context.Channel.SendMessageAsync($"Ping : {Context.Client.Latency}ms");
        }
        [Command("사용자정보")]
        public async Task ReturnInfo(IUser TargetUser = null)
        {
            var UserInfo = TargetUser ?? Context.Client.CurrentUser;
            EmbedBuilder Embed = new EmbedBuilder();
            Embed.AddField("**이름**", UserInfo.Username);
            Embed.AddField("**태그**", UserInfo.Discriminator);
            Embed.AddField("**계정 생성일**", UserInfo.CreatedAt);
            Embed.AddField("**계정 식별자**", UserInfo.Id);
            Embed.Color = new Color(0, 255, 0);
            await ReplyAsync("", false, Embed.Build());
        }
        [RequireUserPermission(GuildPermission.BanMembers)]
        [Command("밴")]
        public async Task DoBan(IUser TargetUser = null, string BanReason = "")
        {
            if (TargetUser == null)
            {
                await ReplyAsync("밴 사용법 : $밴 <밴 대상> <사유(선택)>");
                return;
            }
            await Context.Guild.AddBanAsync(TargetUser, 0, BanReason);
            EmbedBuilder Embed = new EmbedBuilder();
            if(BanReason == String.Empty)
            {
                Embed.Description = "**" + Context.User.Username + "님이 " + TargetUser.Username + "님을 밴 하였습니다.**";
            }
            else
            {
                Embed.Description = "**" + Context.User.Username + "님이 " + TargetUser.Username + "님을 " + BanReason + "의 이유로 밴 하였습니다.**";
            }
            Embed.Color = new Color(255, 0, 0);
            await ReplyAsync("", false, Embed.Build());
        }
        [RequireUserPermission(GuildPermission.KickMembers)]
        [Command("추방")]
        public async Task DoKiack(SocketGuildUser TargetUser = null, string KickReason = "")
        {
            if(TargetUser == null)
            {
                await ReplyAsync("추방 사용법 : $추방 <추방대상> <사유(선택)>");
                return;
            }
            await TargetUser.KickAsync(KickReason);
            EmbedBuilder Embed = new EmbedBuilder();
            if(KickReason == String.Empty)
            {
                Embed.Description = "**" + Context.User.Username + "님이 " + TargetUser.Username + "님을 추방 하였습니다.**";
            }
            else
            {
                Embed.Description = "**" + Context.User.Username + "님이 " + TargetUser.Username + "님을 " + KickReason + "의 이유로 추방하였습니다";
            }
            Embed.Color = new Color(255, 0, 0);
            await ReplyAsync("", false, Embed.Build());
        }
        [Command("현재시간")]
        public async Task NowTime()
        {
            string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            await Context.Channel.SendMessageAsync(time);
        }
    }
}