var Path = require('path');
module.exports = {
    
    host:"127.0.0.1",
    port:8000,
    cleanClientItemTimeout:10000,
    matchEnemyTimeout:200,
    protobufs:[
        'Struct_pb.js',        
        'Request_pb.js',
        'Push_pb.js',
        'Test_pb.js',
    ],
    protoPath:__dirname + Path.sep + "proto",
}