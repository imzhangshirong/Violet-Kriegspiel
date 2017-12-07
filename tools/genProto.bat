::--plugin=protoc-gen-grpc=grpc_csharp_plugin.exe

set PROTOCNODE=protoc --js_out=import_style=commonjs,binary:../servernode/proto   --proto_path=../protos

set PROTOCCSHARP=protoc --csharp_out=../client\Assets\Plugins\Protos  --proto_path=../protos

set PROTOCJAVA=protoc --java_out=../serverjava/ensign/src/main/java  --proto_path=../protos


%PROTOCNODE% Struct.proto
%PROTOCNODE% Request.proto
%PROTOCNODE% Push.proto
%PROTOCNODE% Test.proto

%PROTOCCSHARP% Struct.proto
%PROTOCCSHARP% Request.proto
%PROTOCCSHARP% Push.proto
%PROTOCCSHARP% Test.proto

%PROTOCJAVA% Struct.proto
%PROTOCJAVA% Request.proto
%PROTOCJAVA% Push.proto
%PROTOCJAVA% Test.proto
pause