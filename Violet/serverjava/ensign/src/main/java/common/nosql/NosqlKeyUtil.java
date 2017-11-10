package common.nosql;

public class NosqlKeyUtil {

	public static final String getUserQiPan(int userId) {
		return "QIPAN" + "_" + String.valueOf(userId);
	}
	
}
