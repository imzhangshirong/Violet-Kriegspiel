using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class PlayerPackage : Package<PlayerPackage>
{
    public PlayerInfo playerInfo = new PlayerInfo() {
        userId = 1,
        userName = "KyArvis",
        level = 1,
    };
    public override void Init(object data)
    {
        //throw new NotImplementedException();
    }

    public override void Release()
    {
        //throw new NotImplementedException();
    }

}

public class PlayerInfo
{
    public string userName;
    public int userId;
    public int level;
}