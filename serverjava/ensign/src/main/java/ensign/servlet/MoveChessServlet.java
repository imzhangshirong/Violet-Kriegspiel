package ensign.servlet;

import java.util.ArrayList;
import java.util.List;

import com.google.protobuf.ByteString;
import com.violet.rpc.Push.ChessMovePush;
import com.violet.rpc.Request.MoveChessRequest;
import com.violet.rpc.Request.MoveChessResponse;
import com.violet.rpc.Struct.ChessData;

import common.BaseServlet;
import common.ConfigurationUtil;
import common.db.DBUtil;
import common.enumeration.ChessBoardEnum;
import connection.Server;
import ensign.pojo.RoomInfo;
import ensign.pojo.User;

public class MoveChessServlet extends BaseServlet{
	
	public List<Object> service(ByteString buffer, String token) throws Exception {
		List<Object> result = new ArrayList<>();
		byte[] data = buffer.toByteArray();
		int count = 0;
		MoveChessRequest req = MoveChessRequest.parser().parseFrom(data);
		ChessData source = req.getSource();
		int sourceX = source.getPoint().getX();
		int sourceY = source.getPoint().getY();
		ChessData target = req.getTarget();
		int targetX = target.getPoint().getX();
		int targetY = target.getPoint().getY();
		User user = ConfigurationUtil.tokenMap.get(token);
		String roomId = "";
		if (user != null) {
			roomId = user.getRoomId();
		}
		if (roomId.equals("") || roomId == null) {
			System.out.println("房间号不存在");
		}
		if (sourceX < 0 || sourceX > 4 || sourceY < 0 || sourceY > 11 || targetX < 0 || targetX > 4 || targetY < 0 || targetY > 11) {
			throw new Exception();
		}
		int isCanMove = ensignService.move(source, ChessBoardEnum.getChessBoardEnumByXY(sourceX, sourceY), ChessBoardEnum.getChessBoardEnumByXY(targetX, targetY), roomId);
		ChessMovePush.Builder pushRes = ChessMovePush.newBuilder();
		pushRes.setSource(source);
		pushRes.setTarget(target);
		pushRes.setChessMoveResult(isCanMove);
		pushRes.setCounter(count);
		RoomInfo roomInfo = DBUtil.GetInstance().getRoomInfo(roomId);
		String otherToken = "";
		for (User tempUser : roomInfo.getChessMap().keySet()) {
			if (tempUser.getToken().equals(token)) continue;
			otherToken = tempUser.getToken();
		}
		Server.pushToClient(otherToken, pushRes.build().toByteString(), "ChessMove");
		MoveChessResponse.Builder res = MoveChessResponse.newBuilder();
		res.setSource(source);
		res.setTarget(target);
		res.setChessMoveResult(isCanMove);
		res.setCounter(count + 1);
		result.add(res.build().toByteString());
		result.add(token);
		return result;
	};
}
