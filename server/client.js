var Config = require('./config.js');
var Rpc = require('./module/rpc/client.js');

Rpc.init(Config.host,Config.port,Config.protobufs,Config.protoPath);
var Message = Rpc.getRpc("Hello","Request");
var request = new Message();
request.setContent('RPCTEST!!!');

Rpc.request("Hello",request,function(response){
    console.log(response.getGreet());
});