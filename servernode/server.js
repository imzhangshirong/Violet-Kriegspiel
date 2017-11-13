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



////////////////////////////////////////////////////////////////////
RpcServer.init(Config.port,Config.protobufs,Config.protoPath);
setInterval(function(){
    RpcServer.cleanClientItem();
},Config.cleanClientItemTimeout);


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
        PlayGround.push(user);//进入匹配池
        response.setJoingamefield(true);
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
