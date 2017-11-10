package ensign.servlet;

import com.google.protobuf.ByteString;
import com.violet.rpc.Client.HelloRequest;

import common.BaseServlet;
import common.RpcNetWork;

public class HelloServlet extends BaseServlet{

	public ByteString service(RpcNetWork rpc, ByteString buffer) throws Exception {
		byte[] data = buffer.toByteArray();
		HelloRequest req = HelloRequest.parser().parseFrom(data);
		System.out.println(req.getContent());
		
		return buffer;
	}
}
