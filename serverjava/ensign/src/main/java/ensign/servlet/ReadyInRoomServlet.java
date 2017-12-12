package ensign.servlet;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.Iterator;
import java.util.List;
import java.util.Map;
import java.util.Set;

import com.google.protobuf.ByteString;
import com.google.protobuf.TextFormat;
import com.violet.rpc.Push.GameStateChangePush;
import com.violet.rpc.Request.ReadyInRoomRequest;
import com.violet.rpc.Request.ReadyInRoomResponse;
import com.violet.rpc.Struct.ChessData;

import common.BaseServlet;
import common.ConfigurationUtil;
import common.db.DBUtil;
import connection.Server;
import ensign.pojo.RoomInfo;
import ensign.pojo.User;

public class ReadyInRoomServlet extends BaseServlet{

	@Override
	public List<Object> service(ByteString buffer, String token) throws Exception {
		List<Object> result = new ArrayList<>();
		byte[] data = buffer.toByteArray();
		ReadyInRoomRequest req = ReadyInRoomRequest.parser().parseFrom(data);
		boolean isReady = req.getIsReady();
		List<ChessData> chessList = req.getChessMapList();
		User user = ConfigurationUtil.tokenMap.get(token);
		String roomId = user.getRoomId();
		RoomInfo roomInfo = DBUtil.GetInstance().getRoomInfo(roomId);
		Map<User,List<ChessData>> chessMap = roomInfo.getChessMap();
		if (chessMap == null) {
			chessMap = new HashMap<User,List<ChessData>>();
		}
		if (isReady == true) {
			chessMap.put(user, chessList);
		} else if (chessMap.containsKey(user)){
			chessMap.remove(user);
		}
		roomInfo.setChessMap(chessMap);
		DBUtil.GetInstance().setRoomInfo(roomInfo);
		//当两个人都准备的时候，推送
		if (chessMap.size() == 2) {
			GameStateChangePush.Builder gamePush = GameStateChangePush.newBuilder();
			gamePush.setState(1);//游戏开始
			gamePush.setCounter(0);
			List<ChessData> list = new ArrayList<>();
			Set<User> set = chessMap.keySet();
			Iterator<User> it = set.iterator();
			int i = 0;
			while (it.hasNext()) {
				User key = it.next();
				for (ChessData tempChess : chessMap.get(key)) {
					ChessData.Builder chessData = ChessData.newBuilder();
					chessData.setChessRemoteId(i);
					chessData.setChessType(tempChess.getChessType());
					chessData.setPoint(tempChess.getPoint());
					chessData.setBelong(1000 + "/" + key.getId());
					list.add(chessData.build());
					i ++;
				}
			}
			gamePush.addAllChessMap(list);
			while (it.hasNext()) {
				User key = it.next();
				Server.pushToClient(key.getToken(), gamePush.build().toByteString(), "GameStateChange");
				System.out.println(TextFormat.printToUnicodeString(gamePush.build()));
			}
		} 
		ReadyInRoomResponse.Builder res = ReadyInRoomResponse.newBuilder();
		res.setIsChangeState(true);
		result.add(res.build().toByteString());
		result.add(token);
		System.out.println(TextFormat.printToUnicodeString(res.build()));
		return result;
	}
}
