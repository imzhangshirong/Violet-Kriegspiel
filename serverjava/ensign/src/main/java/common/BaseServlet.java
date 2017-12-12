package common;

import java.util.ArrayList;
import java.util.List;

import com.google.protobuf.ByteString;

import ensign.service.EnsignService;

public class BaseServlet {
	
	protected int rpcNum;
	
	protected EnsignService ensignService;

	public List<Object> service(ByteString buffer, String token) throws Exception{
		List<Object> list = new ArrayList<>();
		list.add(buffer);
		list.add(token);
		return list;
	}
	
	public int getRpcNum() {
		return rpcNum;
	}

	public void setRpcNum(int rpcNum) {
		this.rpcNum = rpcNum;
	}

	public EnsignService getEnsignService() {
		return ensignService;
	}

	public void setEnsignService(EnsignService ensignService) {
		this.ensignService = ensignService;
	}
	
}
