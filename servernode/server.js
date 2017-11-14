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
        roundOrder:0,
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
        roundOrder:0,
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
        roundOrder:0,
        chessSetting:"6|1,3|2,0|3,11|4,0|5;10|6,2|7,5|8,0|9,3|10;9|11,,2|12,,4|13;1|14,5|15,,6|16,7|17;7|18,,4|19,,1|20;8|21,2|22,4|23,3|24,8|25;"
    },
];
var PlayGround = [];
var RoomCenter = {};


function canBeatTo(chessS,chessT){
    let typeS = chessS.getChesstype();
    let typeT = chessT.getChesstype();
    if (typeT < 0) return 0;//未知
    if (typeT == 1) return 2;//敌方炸弹
    if (typeS == 1) return 2;//我方炸弹
    if (typeS == 2)//我方工兵
    {
        if (typeT == 0) return 3;//敌方地雷
    }
    if (typeT == 0) return 2;//敌方地雷
    if (typeT == 11) return 3;//敌方军旗
    if(typeT > typeS)
    {
        return 1;
    }
    else if(typeT < typeS)
    {
        return 3;
    }
    else
    {
        return 2;
    }
    /*
    UNKNOW = 0;
    LOSE = 1;
    TIE = 2;//同归于尽，平手
    WIN = 3;
    CAN_MOVE = 4;
    CANNOT_MOVE = 5;
    */
}

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
function createMatchRoom(users){
    let roomId = "";
    roomId = "match:";
    for(let i=0;i<users.length;i++){
        let user = users[i];
        if(i>0)roomId+="-";
        roomId+=user.zoneId+"/"+user.userId;
    }
    if(RoomCenter[roomId]==null){
        RoomCenter[roomId] = {
            users : users,
            counter : 0,
            chessMap : [],
            roomId:roomId,
            gameState:0,
        }
        let playerInfoMap = {};
        //更新状态
        for(let i=0;i<users.length;i++){
            let user = users[i];
            user.state = 1;
            user.roundOrder = i;//更新回合顺序
            user.gameRemainTime = Config.Game.waitingReady;
            removeFromPlayGround(user.token);
            playerInfoMap[user.token]=getRpcPlayerInfo(user);
        }
        let Message = RpcServer.getRpc("EnterBattleField","Push");
        for(let i=0;i<users.length;i++){
            let user = users[i];
            let client = RpcServer.getClientItemByToken(user.token);
            let push = new Message();
            let playerList = [];
            users.map(function(item){
                if(item.token!=user.token){
                    playerList.push(playerInfoMap[item.token]);
                }
            });
            push.setPlayerlistList(playerList);
            push.setRoundorder(i);//返回自己的回合顺序
            push.setChesssettingList(parseUserChessData(user));//之前的棋子布局
            RpcServer.push(client,"EnterBattleField",push);
        }
    }
}
//处于对方2人对战坐标转换
function getTwoAgainstTranslate(chessMap){
    let newChessMap = [];
    for(let i=0;i<chessMap.length;i++){
        let newOne = chessMap[i].clone();
        let point = newOne.getPoint();
        point.setX(4-point.getX());
        point.setY(11-point.getY());
        newChessMap.push(newOne);
    }
    return newChessMap;
}

function getChessDataByRemoteId(room,remoteId){
    if(room==null)return null;
    for(let i=0;i<room.chessMap.length;i++){
        for(let j=0;j<room.chessMap[i].length;j++){
            let chess = room.chessMap[i][j];
            if(chess.getChessremoteid()==remoteId)return chess;
        }
    }
    return null;
}

function getRpcPlayerInfo(user){
    let PlayerInfo = RpcServer.getRpc("PlayerInfo","");
    let playerInfo = new PlayerInfo();
    playerInfo.setUserid(user.userId);
    playerInfo.setZoneid(user.zoneId);
    playerInfo.setUsername(user.userName);
    playerInfo.setLevel(user.level);
    playerInfo.setState(user.state);
    playerInfo.setRoundorder(user.roundOrder);
    playerInfo.setGameremaintime(user.gameRemainTime);
    return playerInfo;
}

function userReady(room,user){
    user.gameRemainTime = 0;
    user.state = 2;
    let Message = RpcServer.getRpc("PlayerStateChage","Push");
    let PlayerInfo = RpcServer.getRpc("PlayerInfo","");
    for(let i=0;i<room.users.length;i++){
        if(room.users[i].token != user.token){
            let push = new Message();
            let playerInfo = getRpcPlayerInfo(user);
            push.setPlayerinfo(playerInfo);
            let clientItem = RpcServer.getClientItemByToken(room.users[i].token);
            RpcServer.push(clientItem,"PlayerStateChage",push);
        }
    }
    setTimeout(function(){
        startGame(room);
    },100);
}
function startGame(room){
    if(room.gameState != 0)return;
    let start = 0;
    for(let i=0;i<room.users.length;i++){
        if(room.users[i].state == 2)start++;
    }
    if(start==room.users.length){
        //更新房间状态
        room.gameState = 1
        //更新用户状态
        for(let i=0;i<room.users.length;i++){
            room.users[i].gameRemainTime = 0;
            if(i==0)room.users[i].gameRemainTime = Config.Game.waitingRound;
            room.users[i].state = 3;
        }
        //更新棋子remoteId
        let baseId = Math.floor(Math.random()*10000);
        let chessMap = [];
        for(let i=0;i<room.chessMap.length;i++){
            for(let j=0;j<room.chessMap[i].length;j++){
                let chess = room.chessMap[i][j];
                chess.setChessremoteid(baseId);
                chessMap.push(chess);
                baseId++;
            }
        }
        
        let Message = RpcServer.getRpc("GameStateChage","Push");
        
        for(let i=0;i<room.users.length;i++){
            let user = room.users[i];
            let push = new Message();
            let chessMapSend = [];
            let belong = user.zoneId+"/"+user.userId;
            chessMap.map(function(item){
                let chess = item;
                if(item.getBelong()!=belong){//不是自己的都隐藏
                    chess = chess.clone();
                    chess.setChesstype(-1);
                }
                chessMapSend.push(chess);
            });
            push.setState(room.gameState);
            push.setResult(0);
            
            push.setChessmapList(chessMapSend);
            let clientItem = RpcServer.getClientItemByToken(user.token);
            RpcServer.push(clientItem,"GameStateChage",push);
        }
    }
}

function getRoomByUser(user){
    for(let key in RoomCenter){
        let room = RoomCenter[key];
        for(let i=0;i<room.users.length;i++){
            if(room.users[i].token == user.token){
                return room;
            }
        }
    }
    return null;
}

function parseUserChessData(user)
{
    let data = user.chessSetting;
    let chessList = [];
    let baseId = 0;
    let offsetY = 0;
    let ChessData = RpcServer.getRpc("ChessData","");
    let ChessPoint = RpcServer.getRpc("ChessPoint","");
    let belong = user.zoneId+"/"+user.userId;
    let group = 0;
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
                heroData.setBelong(belong);
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


RpcServer.on("Hello",function(requestData){
    let request = requestData.rpc;
    let Message = RpcServer.getRpc("Hello","Response");
    let response = new Message();
    response.setGreet("Hello,"+request.getContent());
    return response;
});
RpcServer.on("Login",function(requestData){
    let request = requestData.rpc;
    let Message = RpcServer.getRpc("Login","Response");
    let response = new Message();
    response.setServertime(Date.now());
    let user = getUserByName(request.getUsername());
    if(user!=null){
        if(user.pass==request.getPassword()){
            let PlayerInfo = RpcServer.getRpc("PlayerInfo","");
            let playerInfo = getRpcPlayerInfo(user);
            user.token = requestData.token;
            console.log("login:"+user.userName+":"+user.token);
            response.setPlayerinfo(playerInfo);
        }
        else{
            requestData.errorCode = 2;
        }
    }
    else{
        requestData.errorCode = 1;
    }
    return response;
});
RpcServer.on("FindEnemy",function(requestData){
    let request = requestData.rpc;
    let Message = RpcServer.getRpc("FindEnemy","Response");
    let response = new Message();
    let user = getUserByToken(requestData.header.getToken());
    if(user!=null){
        if(PlayGround.indexOf(user)==-1){
            PlayGround.push(user);//进入匹配池
            response.setJoingamefield(true);
            console.log("user:"+user.userName +" enter PlayGround");
            setTimeout(function(){
                if(PlayGround.indexOf(user)!=-1){
                    createMatchRoom([user,UserData[2]]);
                    setTimeout(function(){
                        mockEnemyReady(UserData[2]);
                    },5000 + Math.random()*10000);
                }
            },5000);
            
        }
        else{
            requestData.errorCode = 11;
        }
    }
    else{
        requestData.errorCode = 1;
    }
    return response;
});
RpcServer.on("CancelFindEnemy",function(requestData){
    let request = requestData.rpc;
    let Message = RpcServer.getRpc("CancelFindEnemy","Response");
    let response = new Message();
    let user = getUserByToken(requestData.header.getToken());
    if(user!=null){
        if(PlayGround.indexOf(user)!=-1){
            removeFromPlayGround(requestData.header.getToken());
            response.setIscancel(true);
            console.log("user:"+user.userName +" Leave PlayGround!!");
        }
        else{
            requestData.errorCode = 12;
        }
    }
    else{
        requestData.errorCode = 1;
    }
    return response;
});

RpcServer.on("ReadyInRoom",function(requestData){
    let request = requestData.rpc;
    let Message = RpcServer.getRpc("ReadyInRoom","Response");
    let response = new Message();
    let user = getUserByToken(requestData.header.getToken());
    if(user!=null){
        let room = getRoomByUser(user);
        if(room!=null){
            let tempList = request.getChessmapList();
            for(let i = 0;i<tempList.length;i++){
                tempList[i].setBelong(user.zoneId+"/"+user.userId);
            }
            for(let i = 0;i<room.users.length;i++){
                if(room.users[i].token == user.token){
                    room.chessMap[i] = tempList;
                    break;
                }
            }
            console.log("room:" + room.roomId + " user:" + user.userName + " > ready");
            response.setIschangestate(true);
            user.state = 2;
            setTimeout(function(){
                userReady(room,user);
            },100);
        }
        else{
            requestData.errorCode = 21;
        }
    }
    else{
        requestData.errorCode = 1;
    }
    return response;
});

RpcServer.on("MoveChess",function(requestData){
    let request = requestData.rpc;
    let Message = RpcServer.getRpc("MoveChess","Response");
    let response = new Message();
    let user = getUserByToken(requestData.header.getToken());
    if(user!=null){
        let room = getRoomByUser(user);
        if(room!=null){
            room.counter ++;////
            let source = request.getSource();
            let target = request.getTarget();
            let pointS = source.getPoint();
            let pointT = target.getPoint();
            let remoteIdS = source.getChessremoteid();
            let remoteIdT = target.getChessremoteid();
            console.log("room:" + room.roomId + " user:" + user.userName + " > move:"+remoteIdS+"("+pointS.getX()+","+pointS.getY()+")->("+pointT.getX()+","+pointT.getY()+")");
            let realChessS = getChessDataByRemoteId(room,remoteIdS);
            if(realChessS!=null){
                if(remoteIdT!=null && remoteIdT!=""){
                    let realChessT = getChessDataByRemoteId(room,remoteIdT);
                    if(realChessT!=null){
                        response.setSource(source);
                        response.setTarget(target);
                        response.setCounter(room.counter);
                        let result = canBeatTo(realChessS,realChessT);
                        console.log("Beat??->"+canBeatTo(realChessS,realChessT));
                        response.setChessmoveresult(result);
                    }
                    else{
                        requestData.errorCode = 31;
                    }
                }
                else{//检测是否可以移动
                    response.setSource(source);
                    response.setTarget(target);
                    response.setCounter(room.counter);
                    response.setChessmoveresult(4);
                }
            }
            else{
                requestData.errorCode = 31;
            }
        }
        else{
            requestData.errorCode = 21;
        }
    }
    else{
        requestData.errorCode = 1;
    }
    return response;
});

//Mock //////////////////////////////////////////////////////////////////
function mockEnemyReady(user){
    if(user!=null){
        let room = getRoomByUser(user);
        if(room!=null){
            let chessMap = parseUserChessData(user);
            for(let i = 0;i<room.users.length;i++){
                if(room.users[i].token == user.token){
                    room.chessMap[i] = chessMap;
                    break;
                }
            }
            console.log("Mock>>room:" + room.roomId + " user:" + user.userName + " > ready");
            user.state = 2;
            setTimeout(function(){
                userReady(room,user);
            },100);
        }
    }
}



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
