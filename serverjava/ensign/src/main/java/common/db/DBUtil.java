package common.db;

import org.apache.ibatis.session.SqlSession;

import common.redis.RedisUtil;
import ensign.dao.UserMapper;
import ensign.pojo.RoomInfo;
import ensign.pojo.User;

public class DBUtil {

	public static enum DBEnvironment {

		DB_JUNQI(new DB("ensign", "spring-mybatis.xml"));

		private DB db;

		private DBEnvironment(DB db) {
			this.db = db;
		}

		public DB getDB() {
			return db;
		}
	}

	private static Object obj = new Object();

	private static DBUtil dbUtil = null;

	public static DBUtil GetInstance() {
		synchronized (obj) {
			if (dbUtil == null) {
				dbUtil = new DBUtil();
			}
		}
		return dbUtil;
	}

	private RedisUtil redisUtil = null;

	// private FastConsumeTask dbwork;

	DBUtil() {

	}

	public void init() {
		getRedis();
		// dbwork = new FastConsumeTask();
		// dbwork.start(2);
	}

	public RedisUtil getRedis() {
		if (redisUtil == null) {
			redisUtil = new RedisUtil();
		}
		return redisUtil;
	}

	public User getUser(int userId) {
		User user = redisUtil.get("USER_" + userId, User.class);
		if (user == null) {
			user = getUserByDB(userId);
			if (user != null) {
				redisUtil.set("USER_" + user.getId(), user);
			}
		}
		return user;
	}

	public User getUserByDB(int userId) {
		User user = null;
		SqlSession session = DBEnvironment.DB_JUNQI.getDB().getSession();
		try {
			UserMapper userMapper = session.getMapper(UserMapper.class);
			user = userMapper.selectByPrimaryKey(userId);
			session.commit();
		} catch (Exception e) {
			// logger.error("DBUtil", e);
		} finally {
			session.close();
		}
		return user;
	}
	
	public User getUserByName(String userName) {
		User user = null;
		SqlSession session = DBEnvironment.DB_JUNQI.getDB().getSession();
		try {
			UserMapper userMapper = session.getMapper(UserMapper.class);
			user = userMapper.selectByUserName(userName);
			session.commit();
		} catch (Exception e) {
			// logger.error("DBUtil", e);
		} finally {
			session.close();
		}
		return user;
	}

	public void saveUser(final User user) {
		redisUtil.set("USER_" + user.getId(), user);
		saveUserByDB(user);
	}

	public int saveUserByDB(User user) {
		int id = 0;
		if (user == null) {
//			logger.error("saveUserByDB user is null!");
			return -1;
		}
		SqlSession session = DBEnvironment.DB_JUNQI.getDB().getSession();
		try {
			UserMapper userMapper = session.getMapper(UserMapper.class);
			id = userMapper.insertSelective(user);
			session.commit();
		} catch (Exception e) {
//			logger.error("DBUtil", e);
		} finally {
			session.close();
		}
		return id;
	}

	public void updateUser(final User user) {
		redisUtil.set("USER_" + user.getId(), user);
		updateUserByDB(user);
	}

	public void updateUserByDB(User user) {
		if (user == null) {
//			logger.error("updateUserToDB user is null!");
			return;
		}
		SqlSession session = DBEnvironment.DB_JUNQI.getDB().getSession();
		try {
			UserMapper userMapper = session.getMapper(UserMapper.class);
			userMapper.updateByPrimaryKeySelective(user);
			session.commit();
		} catch (Exception e) {
			// logger.error("DBUtil", e);
		} finally {
			session.close();
		}
	}

	
	public void setRoomInfo(RoomInfo roomInfo) {
		redisUtil.set(String.valueOf(roomInfo.getRoomId()), roomInfo);
	}
	
	public RoomInfo getRoomInfo(String roomId) {
		RoomInfo roomInfo = redisUtil.get(roomId, RoomInfo.class);
		return roomInfo;
	}
}
