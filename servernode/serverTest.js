var net = require('net');  
console.log(__dirname);
var server = net.createServer();  
server.on('data',function(data){
    console.log("receive data");
});
server.on('connection', function(client) {  
    console.log("access in");
    client.on('data',function(data){
        console.log("---receive data---");
        console.log("Data:"+data.toString("base64"));
        console.log("------------------");
    });
    client.on('end', function() {
        console.log("out");
    });  
});  
server.listen(8000);  