var Path = require('path');
module.exports = {
    host:"127.0.0.1",
    port:8000,
    protobufs:[
        'Client_pb.js'
    ],
    protoPath:__dirname + Path.sep + "proto",
}