package ensign.servlet;

import java.util.ArrayList;
import java.util.List;

import com.google.protobuf.ByteString;
import com.google.protobuf.TextFormat;
import com.violet.rpc.Request.LoginRequest;
import com.violet.rpc.Request.LoginResponse;
import com.violet.rpc.Struct.PlayerInfo;

import common.BaseServlet;
import common.ConfigurationUtil;
import common.db.DBUtil;
import ensign.pojo.User;

public class LoginServlet extends BaseServlet{
	
	public List<Object> service(ByteString buffer, String token) throws Exception {
		List<Object> result = new ArrayList<>();
		byte[] data = buffer.toByteArray();
		LoginRequest req = LoginRequest.parser().parseFrom(data);
		String userName = req.getUserName();
		String password = req.getPassword();
		
		User user = DBUtil.GetInstance().getUserByName(userName);
		if (user == null) {
			user = new User();
			user.setUserName(userName);
			user.setPassword(password);
			DBUtil.GetInstance().saveUserByDB(user);
			user.setToken("1000" + "_" + user.getId());
			DBUtil.GetInstance().updateUser(user);
		}
		ConfigurationUtil.tokenMap.put(user.getToken(), user);
		
		PlayerInfo.Builder playerInfo = PlayerInfo.newBuilder();
		playerInfo.setUserId(user.getId());
		playerInfo.setZoneId(1000);
		playerInfo.setUserName(userName);
		
		LoginResponse.Builder res = LoginResponse.newBuilder();
		res.setPlayerInfo(playerInfo.build());
		res.setServerTime(System.currentTimeMillis());
		result.add(res.build().toByteString());
		result.add(user.getToken());
		System.out.println(TextFormat.printToUnicodeString(res.build()));
		return result;
		
	}
}
