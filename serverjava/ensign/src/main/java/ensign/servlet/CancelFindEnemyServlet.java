package ensign.servlet;

import java.util.ArrayList;
import java.util.List;

import com.google.protobuf.ByteString;
import com.violet.rpc.Request.CancelFindEnemyResponse;

import common.BaseServlet;
import common.ConfigurationUtil;

public class CancelFindEnemyServlet extends BaseServlet{

	public List<Object> service(ByteString buffer, String token) throws Exception {
		List<Object> result = new ArrayList<>();
		ConfigurationUtil.tokenList.remove(token);
		CancelFindEnemyResponse.Builder res = CancelFindEnemyResponse.newBuilder();
		res.setIsCancel(true);
		result.add(res.build().toByteString());
		result.add(token);
		return result;
	}
}
