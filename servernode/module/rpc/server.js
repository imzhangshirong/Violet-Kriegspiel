'use strict';
var Net = require('net');
var RpcServer = Net.createServer();
var Path = require('path');
var Crypto = require('crypto');
var isInit = false;
var _Request = null;
var _Response = null;
var _messages = [];
var _bindRpcList = {};
var dataQueue = [];
var clients = [];

function md5 (text) {
  return Crypto.createHash('md5').update(text).digest('hex');
};

RpcServer.bufferSize = 16;
RpcServer.on("connection", function (client) {
    try{
        clients.push(
            {
                client:client,
                token:md5(Date.now()+"salt"),
            }
        );
        console.log("access in:"+clients.length+"|"+clients[clients.length-1].token);
        client.on("data", function (data) {
            let socketData = parseSocketBinaryData(data);
            let requestData = dealResquest(socketData.data);
            if(requestData==null)return;
            let clientItemT = getClientItemByToken(requestData.header.getToken());
            let clientItemC = getClientItemByClient(client);
            if(clientItemT!=null && clientItemC!=null){//响应重连的Token
                clientItemT.client = clientItemC.client;
                clientItemC.client = null;
            }
            requestData.client = client;
            if (requestData != null && _bindRpcList[requestData.msg] != null) {
                requestData.token = clientItemC.token;
                let result = _bindRpcList[requestData.msg](requestData);
                let binaryData = returnData(result, requestData);
                if(binaryData!=null){
                    let bufferArraySend = buildSocketBinaryData({
                        version: 1,
                        dataLength: binaryData.length,
                        data: binaryData,
                    });
                    //console.log(bufferArraySend);
                    if (result != null) client.write(new Buffer(bufferArraySend));
                }
            }
        });
        client.on("close",function(){
            clientOut(client);
        });
        client.on("end", function () {
            clientOut(client);
        });
        client.on("error",function(err){
            clientOut(client);
        });
    }
    catch(e){

    }
});

RpcServer.on("close",function(){

});

RpcServer.on("error",function(err){
    
});

function clientOut(client){
    let clientItem = getClientItemByClient(client);
    if(clientItem!=null){
        clientItem.client = null;
    }
    if(_messages["_clientOffline"]!=null)_messages["_clientOffline"](clientItem.token);
    console.log("out:"+clients.length);
}

function cleanClientItem(){
    clients = clients.filter(function(item){
        return item.client!=null;
    });
    console.log("AfterClean:"+clients.length);
}

function getClientItemByToken(token){
    let clientItem = clients.filter(function(item){
        return item.token==token;
    });
    if(clientItem.length!=0)return clientItem[0];
    return null;
}

function getClientItemByClient(client){
    let clientItem = clients.filter(function(item){
        return item.client==client;
    });
    if(clientItem.length!=0)return clientItem[0];
    return null;
}

function parseSocketBinaryData(dataOrigin) {
    let bufferArray = new Uint8Array(dataOrigin);
    let headerLength = 10;
    let socketData = null;
    if (bufferArray[0] == 255 && bufferArray[1] < 255) {
        socketData = {};
        socketData.version = bufferArray[1];
        switch (socketData.version) {
            case 1://按协议版本解析
                socketData.dataLength = getDataLength(bufferArray, 2, 3);
                break;
        }
        let data = new Uint8Array(socketData.dataLength);
        for (let i = 0; i < data.length; i++) {
            data[i] = bufferArray[headerLength + i];
        }
        socketData.data = data;
    }
    else {
        //非完整包
    }
    return socketData;

    function getDataLength(bufferArray, offset, length) {
        let dataLength = 0;
        let base = 1;
        for (let i = length - 1; i >= 0; i--) {
            dataLength += bufferArray[offset + i] * base;
            base *= 256;
        }
        return dataLength;
    }
}

function buildSocketBinaryData(socketData) {
    let headerLength = 10;
    let bufferArray = new Uint8Array(headerLength + socketData.dataLength + 2);
    //头标识
    bufferArray[0] = 255;
    //socket结束标识
    bufferArray[bufferArray.length - 2] = 0;
    bufferArray[bufferArray.length - 1] = 254;
    //开始构建
    bufferArray[1] = socketData.version;//协议版本号
    switch (socketData.version) {
        case 1:
            writeLengthData(bufferArray, socketData.dataLength, 2, 3);//写出数据长度信息
            break;
    }
    //写出数据
    for (let i = 0; i < socketData.data.length; i++) {
        bufferArray[headerLength + i] = socketData.data[i];
    }
    //
    return bufferArray;


    function writeLengthData(bufferArray, dataLength, offset, length) {
        let l = dataLength;
        let d = 0;
        for (let i = length - 1; i > 0; i--) {
            d = l % 256;
            l = Math.floor(l / 256);
            bufferArray[offset + i] = d;
        }
    }
}

function dealResquest(_resquestBytes) {
    let headerReq = _Request.deserializeBinary(new Uint8Array(_resquestBytes));//data为buffer需要转换
    let msg = headerReq.getRpc();
    if (msg == "") return;
    let data = headerReq.getData();
    let reqBuilder = getRpcBuilder(msg, "Request");
    if (reqBuilder == null) return;
    let req = reqBuilder.deserializeBinary(data);
    let rpcCallback = {};
    if (_bindRpcList[msg] != null) {
        rpcCallback.msg = msg;
        rpcCallback.rpc = req;
        rpcCallback.header = headerReq;
        rpcCallback.errorCode = 0;
    }
    else {
        console.error("SERVER:Unknow RPC:"+msg+" request");
        rpcCallback = null;
    }
    return rpcCallback;
}
function getRpcBuilder(msg, type) {
    let reqBuilder = null;
    for (let i = 0; i < _messages.length; i++) {
        if (_messages[i][msg + type]) {
            reqBuilder = _messages[i][msg + type];
        }
    }
    return reqBuilder;
}
function returnData(rpcResponse, requestData) {
    if (requestData == null) return null;
    let headerRes = new _Response();
    let clientItem = getClientItemByClient(requestData.client);
    if(clientItem == null){
        return null;
    }
    else{
        console.log("Response "+requestData.header.getRpc()+"."+requestData.header.getUnique()+" > Token:"+clientItem.token);
        headerRes.setToken(clientItem.token);
        headerRes.setRpc(requestData.msg);
        headerRes.setData(rpcResponse.serializeBinary());
        headerRes.setUnique(requestData.header.getUnique());
        headerRes.setCode(requestData.errorCode);
    }
    return headerRes.serializeBinary();
}
function init(port, protobufs, protoPath) {
    this.port = port;
    //加载所有的protogen生成的文件
    let _messagesPath = protobufs.map(function (item) {
        return protoPath + Path.sep + item;
    });
    _messages = _messagesPath.map(function (item) {
        return require(item);
    });
    //关闭之前的连接
    RpcServer.close();
    _Request = null;
    _Response = null;
    for (let i = 0; i < _messages.length; i++) {
        if (_Request == null && _messages[i]["_Request"]) {
            _Request = _messages[i]["_Request"]
        }
        if (_Response == null && _messages[i]["_Response"]) {
            _Response = _messages[i]["_Response"]
        }
    }

    isInit = true;
}
function on(msg, callback) {
    _bindRpcList[msg] = callback;
}
function push(clientItem,rpcName,rpc){
    let headerRes = new _Response();
    headerRes.setToken(clientItem.token);
    headerRes.setRpc(rpcName);
    headerRes.setData(rpc.serializeBinary());
    let binaryData = headerRes.serializeBinary();
    if(binaryData!=null){
        let bufferArraySend = buildSocketBinaryData({
            version: 1,
            dataLength: binaryData.length,
            data: binaryData,
        });
        //console.log(bufferArraySend);
        clientItem.client.write(new Buffer(bufferArraySend));
    }
}

module.exports = {
    init: init,
    listen: function () {
        RpcServer.listen(this.port);
    },
    on: on,
    isInit: function () { return isInit; },
    close: function () {
        RpcServer.close();
    },
    getRpc: getRpcBuilder,
    getClients: function(){
        return clients;
    },
    push: push,
    cleanClientItem: cleanClientItem,
    getClientItemByToken: getClientItemByToken,
}