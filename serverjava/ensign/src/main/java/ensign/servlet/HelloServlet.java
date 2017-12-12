package ensign.servlet;

import java.util.ArrayList;
import java.util.List;

import com.google.protobuf.ByteString;
import com.violet.rpc.Test.HelloRequest;
import com.violet.rpc.Test.HelloResponse;

import common.BaseServlet;

public class HelloServlet extends BaseServlet{

	public List<Object> service(ByteString buffer, String token) throws Exception {
		List<Object> result = new ArrayList<>();
		byte[] data = buffer.toByteArray();
		HelloRequest req = HelloRequest.parser().parseFrom(data);
		System.out.println(req.getContent());
		HelloResponse.Builder res = HelloResponse.newBuilder();
		res.setGreet("longwei");
		ByteString resbs = res.build().toByteString();
		result.add(resbs);
		result.add(token);
		return result;
	}
}
