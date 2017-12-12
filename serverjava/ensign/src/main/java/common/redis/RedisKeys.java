package common.redis;

public class RedisKeys {

	
	public static final String getUserQiPan(int userId) {
		return "QIPAN" + "_" + String.valueOf(userId);
	}
	
}
