package ensign.service;

import java.util.List;

import org.apache.ibatis.session.SqlSession;

import com.violet.rpc.Struct.ChessData;

import common.BaseService;
import common.Constats;
import common.db.DB;
import common.db.DBUtil;
import common.enumeration.ChessBoardEnum;
import ensign.dao.UserMapper;
import ensign.pojo.RoomInfo;
import ensign.pojo.User;

public class EnsignService extends BaseService{

	//棋子移动
	public int move(ChessData chess, ChessBoardEnum source, ChessBoardEnum target, String roomId) {
		//出发棋子相关信息
		int chessType = chess.getChessType();
		ChessData targetChess = getChess(target.getX(), target.getY(), roomId);
		
		//军旗，地雷，大本营不能移动
		if (chessType == Constats.JUN_QI || chessType == Constats.DI_LEI || source.getType() == Constats.DA_BEN_YING) {
			return Constats.CANNOT_MOVE;
		}
		
		//铁路走铁路
		if (source.getType() == Constats.TIE_LU && target.getType() == Constats.TIE_LU && searchRoad(chess, source, target, roomId)) {
			if (targetChess != null) {
				return compare(chess, targetChess);
			} else {
				return Constats.CAN_MOVE;
			}
		} else if (source.getType() == Constats.XING_YING || target.getType() == Constats.XING_YING) {
			if (Math.abs(source.getX() - target.getX()) < 2 && Math.abs(source.getY() - target.getY()) < 2) {
				if (targetChess != null) {
					return compare(chess, targetChess);
				} else {
					return Constats.CAN_MOVE;
				}
			}
		} else {
			if ((source.getX() == target.getX() && Math.abs(source.getY() - target.getY()) < 2) || (source.getY() == target.getY() && Math.abs(source.getX() - target.getX()) < 2)) {
				if ((source.getX() == 1 || source.getX() == 3) && (source.getY() == 5 || source.getY() == 6)) {
					if (source.getY() == 5 && target.getY() != 6 || source.getY() == 6 && target.getY() != 5) {
						if (targetChess != null) {
							return compare(chess, targetChess);
						} else {
							return Constats.CAN_MOVE;
						}
					}
				} else {
					if (targetChess != null) {
						return compare(chess, targetChess);
					} else {
						return Constats.CAN_MOVE;
					}
				}
			}
		}
		return Constats.CANNOT_MOVE;
	}
	//吃子规则
	public int compare(ChessData chess1, ChessData chess2) {
		int type1 = chess1.getChessType();
		int type2 = chess2.getChessType();
		if (chess1.getBelong().equals(chess2.getBelong())) {
			return Constats.CANNOT_MOVE;
		}
		if (type1 == Constats.ZHA_DAN || type2 == Constats.ZHA_DAN || type1 == type2) {
			return Constats.EQUAL;
		} else if((type1 < type2 && type2 != Constats.JUN_QI) || (type1 != Constats.GONG_BING && type2 == Constats.DI_LEI)) {
			return Constats.LOSE;
		} 
		return Constats.WIN;
	}
	
	//判断输赢
	public boolean winOrLose(int type) {
		if(type == Constats.JUN_QI) {
			return true;
		}
		return false;
	}
	
	//根据坐标获取棋子
	private ChessData getChess(int x, int y, String roomId) {
		RoomInfo roomInfo = DBUtil.GetInstance().getRoomInfo(roomId);
		for (List<ChessData> chessList : roomInfo.getChessMap().values()) {
			for (ChessData chessData : chessList) {
				if (chessData.getPoint().getX() == x && chessData.getPoint().getY() == y) {
					return chessData;
				}
			}
		}
		return null;
	}
	
	//铁路寻路算法
	private boolean searchRoad(ChessData chess, ChessBoardEnum source, ChessBoardEnum target, String roomId) {
		
		//若为工兵
		if (chess.getChessType() == Constats.GONG_BING) {
			return gongBingMove(source, target, roomId);
		} else {
			if (source.getX() == target.getX() || source.getY() == target.getY()) {
				return feiGongBingMove(source, target, roomId);
			}
		}
		return false;
	}
	
	private boolean gongBingMove(ChessBoardEnum from, ChessBoardEnum to, String roomId) {
		if (from == null || to == null) {
			return false;
		}
		if (from.getType() != Constats.TIE_LU || to.getType() != Constats.TIE_LU) {
			return false;
		}
		if (from == to) return true;
		if (getChess(from.getX(), from.getY(), roomId) != null) return false;
		if (from.getX() == 1 || from.getX() == 3) {
			return gongBingMove(ChessBoardEnum.getChessBoardEnumByXY(from.getX() + 1, from.getY()), to, roomId) ||
					gongBingMove(ChessBoardEnum.getChessBoardEnumByXY(from.getX() - 1, from.getY()), to, roomId);
		}
		return gongBingMove(ChessBoardEnum.getChessBoardEnumByXY(from.getX() + 1, from.getY()), to, roomId) ||
				gongBingMove(ChessBoardEnum.getChessBoardEnumByXY(from.getX() - 1, from.getY()), to, roomId) ||
				gongBingMove(ChessBoardEnum.getChessBoardEnumByXY(from.getX(), from.getY() + 1), to, roomId) ||
				gongBingMove(ChessBoardEnum.getChessBoardEnumByXY(from.getX(), from.getY() - 1), to, roomId);
	}
	
	private boolean feiGongBingMove(ChessBoardEnum from, ChessBoardEnum to, String roomId) {
		if (from == null || to == null) {
			return false;
		}
		if (from.getType() != Constats.TIE_LU || to.getType() != Constats.TIE_LU) {
			return false;
		}
		if (from == to) return true;
		if (getChess(from.getX(), from.getY(), roomId) != null) return false;
		if (from.getX() == to.getX()) {
			if (from.getX() == 3) {
				return false;
			} else {
				return feiGongBingMove(ChessBoardEnum.getChessBoardEnumByXY(from.getX(), from.getY() + 1), to, roomId) ||
						feiGongBingMove(ChessBoardEnum.getChessBoardEnumByXY(from.getX(), from.getY() - 1), to, roomId);
			}
		} else {
			return feiGongBingMove(ChessBoardEnum.getChessBoardEnumByXY(from.getX() + 1, from.getY()), to, roomId) ||
					feiGongBingMove(ChessBoardEnum.getChessBoardEnumByXY(from.getX() - 1, from.getY()), to, roomId);
		}
	}
	
	public static void main(String[] args) {
		DB db = new DB("ensign", "spring-mybatis.xml");
		SqlSession session = db.getSession();
		User user = new User();
		UserMapper userMapper = session.getMapper(UserMapper.class);
		user.setUserName("123");
		user.setPassword("qaaa");
		try {
			int id = userMapper.insert(user);
			System.out.println(id);
			session.commit();
		} catch (Exception e) {
			e.printStackTrace();
		} finally {
			session.close();
		}
	}
	
}
