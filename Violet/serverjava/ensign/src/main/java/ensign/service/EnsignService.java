package ensign.service;

import common.Constats;

public class EnsignService {

	//吃子规则
	public int compare(int type1, int type2) {
		
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
	
	
}
