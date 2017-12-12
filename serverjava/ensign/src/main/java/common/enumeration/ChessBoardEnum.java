package common.enumeration;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

import common.Constats;

public enum ChessBoardEnum {

	//type:0-公路，1-铁路，2-行营，3-大本营                x,y表示坐标
	p1(0,11,0),p2(1,11,3),p3(2,11,0),p4(3,11,3),p5(4,11,0),
	p6(0,10,1),p7(1,10,1),p8(2,10,1),p9(3,10,1),p10(4,10,1),
	p11(0,9,1),p12(1,9,2),p13(2,9,0),p14(3,9,2),p15(4,9,1),
	p16(0,8,1),p17(1,8,0),p18(2,8,2),p19(3,8,0),p20(4,8,1),
	p21(0,7,1),p22(1,7,2),p23(2,7,0),p24(3,7,2),p25(4,7,1),	
	p26(0,6,1),p27(1,6,1),p28(2,6,1),p29(3,6,1),p30(4,6,1),

	p31(0,5,1),p32(1,5,1),p33(2,5,1),p34(3,5,1),p35(4,5,1),
	p36(0,4,1),p37(1,4,2),p38(2,4,0),p39(3,4,2),p40(4,4,1),
	p41(0,3,1),p42(1,3,0),p43(2,3,2),p44(3,3,0),p45(4,3,1),
	p46(0,2,1),p47(1,2,2),p48(2,2,0),p49(3,2,2),p50(4,2,1),
	p51(0,1,1),p52(1,1,1),p53(2,1,1),p54(3,1,1),p55(4,1,1),
	p56(0,0,0),p57(1,0,3),p58(2,0,0),p59(3,0,3),p60(4,0,0);
	
	private int x;
	private int y;
	private int type;
	
	private ChessBoardEnum(int x, int y, int type) {
		this.x = x;
		this.y = y;
		this.type = type;
	}

	public int getX() {
		return x;
	}

	public void setX(int x) {
		this.x = x;
	}

	public int getY() {
		return y;
	}

	public void setY(int y) {
		this.y = y;
	}

	public int getType() {
		return type;
	}

	public void setType(int type) {
		this.type = type;
	}
	
	public static ChessBoardEnum getChessBoardEnumByXY(int x, int y) {
		ChessBoardEnum[] array = ChessBoardEnum.values();
		for (ChessBoardEnum tempEnum : array) {
			if (tempEnum.getX() == x && tempEnum.getY() == y) {
				return tempEnum;
			}
		}
		return null;
	}
	
	private static Map<ChessBoardEnum, List<ChessBoardEnum>> enumMap;
	
	//初始化棋盘
	public static void init() {
		for (ChessBoardEnum chessBoard : ChessBoardEnum.values()) {
			List<ChessBoardEnum> enumList = new ArrayList<>();
			int x = chessBoard.getX();
			int y = chessBoard.getY();
			int type = chessBoard.getType();
			if (x - 1 >= 0 && y + 1 <= 11 && (type == Constats.XING_YING || getChessBoardEnumByXY(x - 1, y + 1).getType() == Constats.XING_YING)) 
				enumList.add(getChessBoardEnumByXY(x - 1, y + 1));
			
			
			enumMap.put(chessBoard, enumList);
		}
	}
}
