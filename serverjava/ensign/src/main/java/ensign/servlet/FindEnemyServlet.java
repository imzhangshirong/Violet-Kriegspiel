package ensign.servlet;

import java.util.ArrayList;
import java.util.List;

import com.google.protobuf.ByteString;
import com.google.protobuf.TextFormat;
import com.violet.rpc.Request.FindEnemyRequest;
import com.violet.rpc.Request.FindEnemyResponse;

import common.BaseServlet;
import common.ConfigurationUtil;

public class FindEnemyServlet extends BaseServlet{
	
	@Override
	public List<Object> service(ByteString buffer, String token) throws Exception {
		List<Object> result = new ArrayList<>();
		boolean isExist = false;
		for (String tempToken : ConfigurationUtil.tokenList) {
			if (tempToken.equals(token)) {
				isExist = true;
			}
		}
		if (!isExist) {
			ConfigurationUtil.tokenList.add(token);
		}
		FindEnemyResponse.Builder res = FindEnemyResponse.newBuilder();
		res.setJoinGameField(true);
		result.add(res.build().toByteString());
		result.add(token);
		System.out.println(TextFormat.printToUnicodeString(res.build()));
		return result;
		
	}
	
}
 