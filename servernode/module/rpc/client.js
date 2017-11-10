'use strict';
var Net = require('net');
var Client = new Net.Socket();
var Path = require('path');
var isInit = false;
var _Request = null;
var _Response = null;
var _messages = [];
function request(msg, rpc, callback) {
    if (msg == "") return;
    let reqBuilder = getRpcBuilder(msg, "Request");
    if (reqBuilder == null) return;
    let req = new reqBuilder();
    let headerReq = new _Request();
    headerReq.setToken(Math.random().toString());
    headerReq.setRpc(msg);
    headerReq.setData(rpc.serializeBinary());
    Client.on("data", function (data) {//data为buffer需要转换
        let socketData = parseSocketBinaryData(data);
        let headerRes = _Response.deserializeBinary(socketData.data);
        if (headerRes != null && headerRes.getRpc() == msg) {
            let data = headerRes.getData();
            let reqBuilder = getRpcBuilder(msg, "Response");
            callback(reqBuilder.deserializeBinary(data));
        }
        Client.end();
    });
    Client.connect(this.port, this.host, function () {
        let binaryData = headerReq.serializeBinary();
        let bufferArraySend = buildSocketBinaryData({
            version: 1,
            dataLength: binaryData.length,
            data: binaryData,
        });
        Client.write(new Buffer(bufferArraySend));
    });
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

function getRpcBuilder(msg, type) {
    let reqBuilder = null;
    for (let i = 0; i < _messages.length; i++) {
        if (_messages[i][msg + type]) {
            reqBuilder = _messages[i][msg + type];
        }
    }
    return reqBuilder;
}
function init(host, port, protobufs, protoPath) {
    this.host = host;
    this.port = port;
    //加载所有的protogen生成的文件
    let _messagesPath = protobufs.map(function (item) {
        return protoPath + Path.sep + item;
    });
    _messages = _messagesPath.map(function (item) {
        return require(item);
    });
    //关闭之前的连接
    Client.end();
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
module.exports = {
    init: init,
    request: request,
    isInit: function () { return isInit; },
    getRpc: getRpcBuilder,
}