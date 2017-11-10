package common;

import com.google.protobuf.ByteString;

public class BaseServlet {
	
	protected int rpcNum;

	public int getRpcNum() {
		return rpcNum;
	}

	public void setRpcNum(int rpcNum) {
		this.rpcNum = rpcNum;
	}

	public ByteString service(RpcNetWork rpc,ByteString buffer) throws Exception{
		return buffer;
	}
}
