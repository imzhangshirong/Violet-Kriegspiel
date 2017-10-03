set PROTOCNODE=protoc --js_out=import_style=commonjs,binary:../server/proto --plugin=protoc-gen-grpc=grpc_node_plugin.exe  --proto_path=../protos

set PROTOCCSHARP=protoc --csharp_out=../client\Assets\Plugins\Proto --plugin=protoc-gen-grpc=grpc_csharp_plugin.exe --proto_path=../protos  --grpc_out=./

%PROTOCNODE% Client.proto
%PROTOCCSHARP% Client.proto
pause