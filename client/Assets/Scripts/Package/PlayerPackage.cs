using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Com.Violet.Rpc;
public class PlayerPackage : Package<PlayerPackage>
{
    public PlayerInfo playerInfo;
    public override void Init(object data)
    {
        base.Init(data);
        //throw new NotImplementedException();
        LoginResponse responseData = (LoginResponse)data;
        playerInfo = responseData.PlayerInfo;
        Debuger.Warn("User:"+playerInfo.UserName+" LoginSet:"+playerInfo.ZoneId+"/"+playerInfo.UserId);
    }

    public override void Release()
    {
        base.Release();
    }

}
