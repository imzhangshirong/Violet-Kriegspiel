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
        playerInfo = new PlayerInfo();
        playerInfo.Level = 12;
        playerInfo.UserId = 1;
        playerInfo.UserName = "KyArvis";
        playerInfo.State = (int)PlayerState.UNREADY;
    }

    public override void Release()
    {
        base.Release();
    }

}
