var Config = require('./config.js');
var RpcServer = require('./module/rpc/server.js');

RpcServer.init(Config.port,Config.protobufs,Config.protoPath);

RpcServer.on("Hello",function(resquest){
    let Message = RpcServer.getRpc("Hello","Response");
    let response = new Message();
    response.setGreet("Hello,"+resquest.getContent());
    return response;
});

RpcServer.listen();
