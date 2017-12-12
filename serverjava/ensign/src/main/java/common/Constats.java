package common;

import java.util.ArrayList;
import java.util.List;

import com.violet.rpc.Struct.ChessData;
import com.violet.rpc.Struct.ChessPoint;

public class Constats {

	public static final int JUN_QI = 11;
	
	public static final int SI_LING = 10;
	
	public static final int JUN_ZHANG = 9;
	
	public static final int SHI_ZHANG = 8;
	
	public static final int LV_ZHANG = 7;
	
	public static final int TUAN_ZHANG = 6;
	
	public static final int YING_ZHANG = 5;
	
	public static final int LIAN_ZHANG = 4;
	
	public static final int PAI_ZHANG = 3;
	
	public static final int GONG_BING = 2;
	
	public static final int ZHA_DAN = 1;
	
	public static final int DI_LEI = 0;
	
	public static final int WIN = 3;
	
	public static final int LOSE = 1;
	
	public static final int EQUAL = 2;
	
	public static final int CAN_MOVE = 4;
	
	public static final int CANNOT_MOVE = 5;
	
	public static final int GONG_LU = 0;
	
	public static final int TIE_LU = 1;
	
	public static final int XING_YING = 2;
	
	public static final int DA_BEN_YING = 3;
	
	public static final String SPLIT = "_";
	
	public static final int[][] twoPersonQipan = {
													{0,3,0,3,0},
													{1,1,1,1,1},
													{1,2,0,2,1},
													{1,0,2,0,1},
													{1,2,0,2,1},
													{1,1,1,1,1},
			
													{1,1,1,1,1},
													{1,2,0,2,1},
													{1,0,2,0,1},
													{1,2,0,2,1},
													{1,1,1,1,1},
													{0,3,0,3,0}
																};
	
	public static List<ChessData> initChessData (String belong) {
		List<ChessData> initChessList = new ArrayList<>();
		ChessData.Builder chess1 = ChessData.newBuilder();
		chess1.setChessType(LIAN_ZHANG);
		ChessPoint.Builder point = ChessPoint.newBuilder();
		point.setX(0);
		point.setY(5);
		chess1.setPoint(point.build());
		chess1.setBelong(belong);
		initChessList.add(chess1.build());
		
		ChessData.Builder chess2 = ChessData.newBuilder();
		chess2.setChessType(TUAN_ZHANG);
		point.setX(1);
		point.setY(5);
		chess2.setPoint(point.build());
		chess2.setBelong(belong);
		initChessList.add(chess2.build());
		
		ChessData.Builder chess3 = ChessData.newBuilder();
		chess3.setChessType(PAI_ZHANG);
		point.setX(2);
		point.setY(5);
		chess3.setPoint(point.build());
		chess3.setBelong(belong);
		initChessList.add(chess3.build());
		
		ChessData.Builder chess4 = ChessData.newBuilder();
		chess4.setChessType(YING_ZHANG);
		point.setX(3);
		point.setY(5);
		chess4.setPoint(point.build());
		chess4.setBelong(belong);
		initChessList.add(chess4.build());
		
		ChessData.Builder chess5 = ChessData.newBuilder();
		chess5.setChessType(SHI_ZHANG);
		point.setX(4);
		point.setY(5);
		chess5.setPoint(point.build());
		chess5.setBelong(belong);
		initChessList.add(chess5.build());
		
		ChessData.Builder chess6 = ChessData.newBuilder();
		chess6.setChessType(SI_LING);
		point.setX(0);
		point.setY(4);
		chess6.setPoint(point.build());
		chess6.setBelong(belong);
		initChessList.add(chess6.build());
		
		ChessData.Builder chess7 = ChessData.newBuilder();
		chess7.setChessType(GONG_BING);
		point.setX(2);
		point.setY(4);
		chess7.setPoint(point.build());
		chess7.setBelong(belong);
		initChessList.add(chess7.build());
		
		ChessData.Builder chess8 = ChessData.newBuilder();
		chess8.setChessType(ZHA_DAN);
		point.setX(4);
		point.setY(4);
		chess8.setPoint(point.build());
		chess8.setBelong(belong);
		initChessList.add(chess8.build());
		
		ChessData.Builder chess9 = ChessData.newBuilder();
		chess9.setChessType(TUAN_ZHANG);
		point.setX(0);
		point.setY(3);
		chess9.setPoint(point.build());
		chess9.setBelong(belong);
		initChessList.add(chess9.build());
		
		ChessData.Builder chess10 = ChessData.newBuilder();
		chess10.setChessType(GONG_BING);
		point.setX(1);
		point.setY(3);
		chess10.setPoint(point.build());
		chess10.setBelong(belong);
		initChessList.add(chess10.build());
		
		ChessData.Builder chess11 = ChessData.newBuilder();
		chess11.setChessType(ZHA_DAN);
		point.setX(3);
		point.setY(3);
		chess11.setPoint(point.build());
		chess11.setBelong(belong);
		initChessList.add(chess11.build());
		
		ChessData.Builder chess12 = ChessData.newBuilder();
		chess12.setChessType(JUN_ZHANG);
		point.setX(4);
		point.setY(3);
		chess12.setPoint(point.build());
		chess12.setBelong(belong);
		initChessList.add(chess12.build());
		
		ChessData.Builder chess13 = ChessData.newBuilder();
		chess13.setChessType(LIAN_ZHANG);
		point.setX(0);
		point.setY(2);
		chess13.setPoint(point.build());
		chess13.setBelong(belong);
		initChessList.add(chess13.build());
		
		ChessData.Builder chess14 = ChessData.newBuilder();
		chess14.setChessType(YING_ZHANG);
		point.setX(2);
		point.setY(2);
		chess14.setPoint(point.build());
		chess14.setBelong(belong);
		initChessList.add(chess14.build());
		
		ChessData.Builder chess15 = ChessData.newBuilder();
		chess15.setChessType(LV_ZHANG);
		point.setX(4);
		point.setY(2);
		chess15.setPoint(point.build());
		chess15.setBelong(belong);
		initChessList.add(chess15.build());
		
		ChessData.Builder chess16= ChessData.newBuilder();
		chess16.setChessType(DI_LEI);
		point.setX(0);
		point.setY(1);
		chess16.setPoint(point.build());
		chess16.setBelong(belong);
		initChessList.add(chess16.build());
		
		ChessData.Builder chess17 = ChessData.newBuilder();
		chess17.setChessType(GONG_BING);
		point.setX(1);
		point.setY(1);
		chess17.setPoint(point.build());
		chess17.setBelong(belong);
		initChessList.add(chess17.build());
		
		ChessData.Builder chess18 = ChessData.newBuilder();
		chess18.setChessType(LV_ZHANG);
		point.setX(2);
		point.setY(1);
		chess18.setPoint(point.build());
		chess18.setBelong(belong);
		initChessList.add(chess18.build());
		
		ChessData.Builder chess19 = ChessData.newBuilder();
		chess19.setChessType(SHI_ZHANG);
		point.setX(3);
		point.setY(1);
		chess19.setPoint(point.build());
		chess19.setBelong(belong);
		initChessList.add(chess19.build());
		
		ChessData.Builder chess20 = ChessData.newBuilder();
		chess20.setChessType(PAI_ZHANG);
		point.setX(4);
		point.setY(1);
		chess20.setPoint(point.build());
		chess20.setBelong(belong);
		initChessList.add(chess20.build());
		
		ChessData.Builder chess21 = ChessData.newBuilder();
		chess21.setChessType(LIAN_ZHANG);
		point.setX(0);
		point.setY(0);
		chess21.setPoint(point.build());
		chess21.setBelong(belong);
		initChessList.add(chess21.build());
		
		ChessData.Builder chess22 = ChessData.newBuilder();
		chess22.setChessType(PAI_ZHANG);
		point.setX(1);
		point.setY(0);
		chess22.setPoint(point.build());
		chess22.setBelong(belong);
		initChessList.add(chess22.build());
		
		ChessData.Builder chess23 = ChessData.newBuilder();
		chess23.setChessType(DI_LEI);
		point.setX(2);
		point.setY(0);
		chess23.setPoint(point.build());
		chess23.setBelong(belong);
		initChessList.add(chess23.build());
		
		ChessData.Builder chess24 = ChessData.newBuilder();
		chess24.setChessType(JUN_QI);
		point.setX(3);
		point.setY(0);
		chess24.setPoint(point.build());
		chess24.setBelong(belong);
		initChessList.add(chess24.build());
		
		ChessData.Builder chess25 = ChessData.newBuilder();
		chess25.setChessType(DI_LEI);
		point.setX(4);
		point.setY(0);
		chess25.setPoint(point.build());
		chess25.setBelong(belong);
		initChessList.add(chess25.build());
		
		return initChessList;
	}
}
