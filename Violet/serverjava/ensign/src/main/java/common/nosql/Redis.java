package common.nosql;

import org.apache.commons.pool.impl.GenericObjectPool;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import redis.clients.jedis.Jedis;
import redis.clients.jedis.JedisPool;

public class Redis implements NoSql{

	private static final Logger logger = LoggerFactory.getLogger("nosqlLogger");
	
	static GenericObjectPool.Config cfg = new GenericObjectPool.Config();
	
	static {
		cfg.maxActive = 3000;
		cfg.maxWait = 2000;// time out
		cfg.minIdle = 20;
		cfg.timeBetweenEvictionRunsMillis = 4 * 60 * 1000;// connection will be
															// checked per
															// 2minutes
		cfg.minEvictableIdleTimeMillis =  10 * 60 * 1000;// an object will be
														// dropped if it's idle
														// last 10mins
		cfg.testWhileIdle = true;
		cfg.testOnBorrow = false;
		// cfg.setMaxIdle(20);
		// cfg.setMinIdle(10);
		// cfg.setMaxTotal(100);
		// cfg.setMaxWaitMillis(2000);
		// cfg.setTimeBetweenEvictionRunsMillis(2 * 60 * 1000);
		// cfg.setTestWhileIdle(true);
		// cfg.setMinEvictableIdleTimeMillis(10 * 60 * 1000);
	}
	
	JedisPool pool;

	// private static final int QUEUE_SIZE = 50;

	private String host;

	private int port;
	private String password;

	public String getPassword() {
		return password;
	}

	public void setPassword(String password) {
		this.password = password;
	}

	long logStartTime = System.currentTimeMillis();
	
	public void init() {// this method should be called in spring

		logger.info("launch jedis " + host + ":" + port);
		pool = new JedisPool(cfg, host, port);
		logger.info("jedis init successfully");
	}
	
	@Override
	public void hashSet(String key, String field, String value) throws Exception {
		logStartTime = System.currentTimeMillis();
		Jedis jedis = null;
		try {
			jedis = pool.getResource();
			jedis.hset(key, field, value);
			checkValue(key, field, value);
		} catch (Exception e) {
			logger.error("redis error: " , e);
			pool.returnBrokenResource(jedis);
			throw e;
		} finally {
			if (null != jedis) {
				pool.returnResource(jedis);
			}
			logger.debug("{}\t hashSet use:\t{}\tms", key, System.currentTimeMillis() - logStartTime);
		}
	}
	
	@Override
	public String hashGet(String key, String field) throws Exception {
		logStartTime = System.currentTimeMillis();
		Jedis jedis = null;
		try {
			jedis = pool.getResource();
			return jedis.hget(key, field);
		} catch (Exception e) {
			logger.error("redis error: " , e);
			pool.returnBrokenResource(jedis);
			throw e;
		} finally {
			if (null != jedis) {
				pool.returnResource(jedis);
			}
			logger.debug("{}\t hashGet use:\t{}\tms", key, System.currentTimeMillis() - logStartTime);
		}
	}
	
	@Override
	public void set(String key, String value) throws Exception {
		logStartTime = System.currentTimeMillis();
		Jedis jedis = null;
		try {
			jedis = pool.getResource();
			jedis.set(key, value);
			checkValue(key, value);
		}catch(Exception e) {
			logger.error("redis error:", e);
			pool.returnBrokenResource(jedis);
			throw e;
		}finally {
			if (null != jedis) {
				pool.returnResource(jedis);
			}
			logger.debug("{}\t set use:\t{}\tms", key, System.currentTimeMillis() - logStartTime);
		}
	}
	
	@Override
	public String get(String key) throws Exception {
		logStartTime = System.currentTimeMillis();
		Jedis jedis = pool.getResource();
		try {
			String result = jedis.get(key);
			return result;
		} catch (Exception e) {
			logger.error("redis error: " , e);
			pool.returnBrokenResource(jedis);
			throw e;
		} finally {
			if (null != jedis) {
				pool.returnResource(jedis);
			}
			logger.debug("{}\t get use:\t{}\tms", key, System.currentTimeMillis() - logStartTime);
		}
	}
	
	private void checkValue(String key1,String key2, String value){
		int size = value.getBytes().length;
		if(size>2*1024){
			logger.warn("checkValue key:{} key:{} more than 2kb,size:{}",key1,key2,size);
		}
		if(key1.indexOf("G_B")!=-1){
			logger.warn("checkValue key:{} size:{}", key1,size);
		}
	}
	
	private void checkValue(String key, String value){
		int size = value.getBytes().length;
		if(size>2*1024){
			logger.warn("checkValue key:{} more than 2kb,size:{}",key,size);
		}
		if(key.indexOf("G_B")!=-1){
			logger.warn("checkValue key:{} size:{}", key,size);
		}
	}
}
