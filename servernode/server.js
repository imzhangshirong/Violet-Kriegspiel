var Config = require('./config.js');
var RpcServer = require('./module/rpc/server.js');

var UserData = [
    {
        userName:"qwe",
        userId:1,
        zoneId:1,
        level:16,
        pass:"123",
        token:"",
        state:0,
        gameRemainTime:0,
        chessSetting:"6|1,3|2,0|3,11|4,0|5;10|6,2|7,5|8,0|9,3|10;9|11,,2|12,,4|13;1|14,5|15,,6|16,7|17;7|18,,4|19,,1|20;8|21,2|22,4|23,3|24,8|25;"
    },
    {
        userName:"asd",
        userId:2,
        zoneId:1,
        level:6,
        pass:"456",
        token:"",
        state:0,
        gameRemainTime:0,
        chessSetting:"6|1,3|2,0|3,11|4,0|5;10|6,2|7,5|8,0|9,3|10;9|11,,2|12,,4|13;1|14,5|15,,6|16,7|17;7|18,,4|19,,1|20;8|21,2|22,4|23,3|24,8|25;"
    },
    {
        userName:"MiaoMiaomiao",
        userId:3,
        zoneId:1,
        level:3,
        pass:"123",
        token:"",
        state:0,
        gameRemainTime:0,
        chessSetting:"6|1,3|2,0|3,11|4,0|5;10|6,2|7,5|8,0|9,3|10;9|11,,2|12,,4|13;1|14,5|15,,6|16,7|17;7|18,,4|19,,1|20;8|21,2|22,4|23,3|24,8|25;"
    },
];
var PlayGround = [];
var RoomCenter = {};



function getUserByToken(token){
    let items = UserData.filter(function(item){
        return item.token == token;
    });
    if(items.length>0)return items[0];
    return null;
}
function getUserByName(name){
    let items = UserData.filter(function(item){
        return item.userName == name;
    });
    if(items.length>0)return items[0];
    return null;
}
function removeFromPlayGround(token){
    PlayGround = PlayGround.filter(function(item){
        return item.token != token;
    });
}
function createMatchRoom(user1,user2){
    let roomId = "";
    roomId = "match:"+user1.zoneId+"/"+user1.userId+"-"+user2.zoneId+"/"+user2.userId;
    if(RoomCenter[roomId]==null){
        RoomCenter[roomId] = {
            users : [user1,user2],
            counter : 0,
            chessMap : {},

        }
        let client1 = RpcServer.getClientItemByToken(user1.token);
        let client2 = RpcServer.getClientItemByToken(user2.token);
        let Message = RpcServer.getRpc("EnterBattleField","Push");
        let PlayerInfo = RpcServer.getRpc("PlayerInfo","");
        let push = new Message();
        let playerInfo = new PlayerInfo();
        user2.state = 1;
        playerInfo.setUserid(user2.userId);
        playerInfo.setZoneid(user2.zoneId);
        playerInfo.setUsername(user2.userName);
        playerInfo.setLevel(user2.level);
        playerInfo.setState(user2.state);
        removeFromPlayGround(user2.token);
        push.setPlayerlistList([playerInfo]);
        push.setRoundorder(0);
        push.setChesssettingList(parseChessDataFromString(user1.chessSetting,0));
        RpcServer.push(client1,"EnterBattleField",push);
    }
}

function parseChessDataFromString(data,group)
{
    let chessList = [];
    let baseId = 0;
    let offsetY = 0;
    let ChessData = RpcServer.getRpc("ChessData","");
    let ChessPoint = RpcServer.getRpc("ChessPoint","");
    switch (group)
    {
        case 0:
            baseId = 0;
            offsetY = 0;
            break;
        case 1:
            baseId = 100;
            offsetY = 6;
            break;
    }
    let rows = data.split(';');
    for(let i = 0; i < rows.length && i < 6; i++)
    {
        let chessTypeIds = rows[i].split(',');
        for(let j = 0; j < chessTypeIds.length; j++)
        {
            let id = chessTypeIds[j];
            if (id != "")
            {
                let type = -1;
                let remoteId = 0;
                if (id.indexOf("|") > -1)
                {
                    let ids = id.split('|');
                    type = Number(ids[0]);
                    remoteId = Number(ids[1]);
                }
                else
                {
                    type = Number(id);
                }
                let heroData = new ChessData();
                heroData.setChesstype(type);
                heroData.setChessremoteid(remoteId);
                heroData.setGroup(group);
                let point = new ChessPoint();
                switch (group)
                {
                    case 0:
                        point.setX(j);
                        point.setY(offsetY + i);
                        break;
                    case 1://敌人在我方是反的
                        point.setX(4 - j);
                        point.setY(offsetY + 5 - i);
                        break;
                }
                heroData.setPoint(point);
                chessList.push(heroData);
            }
        }
    }
    return chessList;

}


////////////////////////////////////////////////////////////////////
setInterval(function(){
    let player1Id = Math.floor(Math.random() * PlayGround.length);
    let player2Id = Math.floor(Math.random() * PlayGround.length);
    if(player1Id!=player2Id){

    }
},Config.matchEnemyTimeout);

RpcServer.init(Config.port,Config.protobufs,Config.protoPath);
setInterval(function(){
    RpcServer.cleanClientItem();
},Config.cleanClientItemTimeout);

RpcServer.on("_clientOffline",function(token){

});


RpcServer.on("Hello",function(resquestData){
    let resquest = resquestData.rpc;
    let Message = RpcServer.getRpc("Hello","Response");
    let response = new Message();
    response.setGreet("Hello,"+resquest.getContent());
    return response;
});
RpcServer.on("Login",function(resquestData){
    let resquest = resquestData.rpc;
    let Message = RpcServer.getRpc("Login","Response");
    let response = new Message();
    response.setServertime(Date.now());
    let user = getUserByName(resquest.getUsername());
    if(user!=null){
        if(user.pass==resquest.getPassword()){
            let PlayerInfo = RpcServer.getRpc("PlayerInfo","");
            let playerInfo = new PlayerInfo();
            playerInfo.setUserid(user.userId);
            playerInfo.setZoneid(user.zoneId);
            playerInfo.setUsername(user.userName);
            playerInfo.setState(0);
            playerInfo.setLevel(user.level);
            user.token = resquestData.token;
            console.log("login:"+user.userName+":"+user.token);
            response.setPlayerinfo(playerInfo);
        }
        else{
            resquestData.errorCode = 2;
        }
    }
    else{
        resquestData.errorCode = 1;
    }
    return response;
});
RpcServer.on("FindEnemy",function(resquestData){
    let resquest = resquestData.rpc;
    let Message = RpcServer.getRpc("FindEnemy","Response");
    let response = new Message();
    let user = getUserByToken(resquestData.header.getToken());
    if(user!=null){
        if(PlayGround.indexOf(user)==-1){
            PlayGround.push(user);//进入匹配池
            response.setJoingamefield(true);
            console.log("user:"+user.userName +" enter PlayGround");
            setTimeout(function(){
                createMatchRoom(user,UserData[2]);
            },5000);
            
        }
        else{
            resquestData.errorCode = 11;
        }
    }
    else{
        resquestData.errorCode = 1;
    }
    return response;
});
RpcServer.on("CancelFindEnemy",function(resquestData){
    let resquest = resquestData.rpc;
    let Message = RpcServer.getRpc("CancelFindEnemy","Response");
    let response = new Message();
    let user = getUserByToken(resquestData.header.getToken());
    if(user!=null){
        if(PlayGround.indexOf(user)!=-1){
            removeFromPlayGround(resquestData.header.getToken());
            response.setIscancel(true);
            console.log("user:"+user.userName +" Leave PlayGround!!");
        }
        else{
            resquestData.errorCode = 12;
        }
    }
    else{
        resquestData.errorCode = 1;
    }
    return response;
});


//Mock Push//////////////////////////////////////////////////////////////////
var pushAction={
    "EBF":{
        "0":function(client){
            let Message = RpcServer.getRpc("EnterBattleField","Push");
            let push = new Message();
            let PlayerInfo = RpcServer.getRpc("PlayerInfo","");
            let playerInfo = new PlayerInfo();
            playerInfo.setUserid(2);
            playerInfo.setZoneid(1);
            playerInfo.setUsername();
            playerInfo.setState(0);
            playerInfo.setLevel(12);
            RpcServer.push(client,"EnterBattleField",push);
        },
        "1":function(){
            
        },
    }
}
var Readline = require('readline');
rl = Readline.createInterface({
    input: process.stdin,
    output: process.stdout
});
rl.on('line', function(data) {
    var inputs = data;
    var p = inputs.indexOf(":");
    var o = inputs.indexOf(">");
    if(p==-1 || o==-1)return;
    var type = inputs.substr(0,p);
    var num = inputs.substr(p+1,o-p-1);
    var clientId = inputs.substr(o+1,inputs.length-o);
    let clients = RpcServer.getClients();
    if(pushAction[type] && pushAction[type][num]){
        pushAction[type][num](clients[clientId]);
        console.log("Push "+type+":"+num+">"+clients[clientId].token);
    }
});



RpcServer.listen();
