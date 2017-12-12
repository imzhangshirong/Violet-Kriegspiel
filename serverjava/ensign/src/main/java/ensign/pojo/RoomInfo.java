package ensign.pojo;

import java.util.List;
import java.util.Map;

import com.violet.rpc.Struct.ChessData;

public class RoomInfo {

	private String roomId;
	
	private List<User> userList;
	
	private Map<User,List<ChessData>> chessMap;

	public String getRoomId() {
		return roomId;
	}

	public void setRoomId(String roomId) {
		this.roomId = roomId;
	}

	public List<User> getUserList() {
		return userList;
	}

	public void setUserList(List<User> userList) {
		this.userList = userList;
	}

	public Map<User, List<ChessData>> getChessMap() {
		return chessMap;
	}

	public void setChessMap(Map<User, List<ChessData>> chessMap) {
		this.chessMap = chessMap;
	}

}
