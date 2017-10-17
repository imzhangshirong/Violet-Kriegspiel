#--plugin=protoc-gen-grpc=grpc_csharp_plugin.exe

set PROTOCNODE=protoc --js_out=import_style=commonjs,binary:../servernode/proto   --proto_path=../protos

set PROTOCCSHARP=protoc --csharp_out=../client\Assets\Plugins\Proto  --proto_path=../protos 

set PROTOCJAVA=protoc --java_out=../serverjava\proto  --proto_path=../protos

%PROTOCNODE% Client.proto
%PROTOCCSHARP% Client.proto
%PROTOCJAVA% Client.proto
pause