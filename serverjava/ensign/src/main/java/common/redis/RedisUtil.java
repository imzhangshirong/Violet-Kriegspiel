package common.redis;

import java.lang.reflect.Type;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.Iterator;
import java.util.List;
import java.util.Map;
import java.util.Map.Entry;

import common.ConfigurationUtil;
import common.utils.JsonUtil;
import common.utils.StringUtil;
import redis.clients.jedis.Jedis;
import redis.clients.jedis.JedisPool;
import redis.clients.jedis.JedisPoolConfig;

public class RedisUtil {
	
	private JedisPool pool = null;
	
	public RedisUtil()
	{
		init();
	}
	
	public void init()
	{
		// 连接池配置
		try
		{
			JedisPoolConfig config = new JedisPoolConfig();
			config.setMaxTotal(250);
			config.setMaxIdle(21600000);
			config.setMaxWaitMillis(1000);
			config.setTestOnBorrow(true);
			config.setTestWhileIdle(true);
			pool = new JedisPool(config, 
					ConfigurationUtil.APP_REDIS_HOST, 
					ConfigurationUtil.APP_REDIS_PROT, 
					10000, 
					null, 
					0);
		}
		catch(Exception e)
		{
			
		}
		
	}
	
	public String get(String key) {
		String result = null;
		Jedis jedis = null;
		try 
		{
			jedis = pool.getResource();
			result = jedis.get(key);
		} 
		catch (Exception e){
			
		} 
		finally {
			returnJedis(jedis);
		}
		return result;
	}
	
	public <T> T get(String key, Class<T> classType) {
		String v = this.get(key);
		if (StringUtil.isNullOrEmpty(v))
			return null;
		return JsonUtil.fromJson(v, classType);
	}

	public <T> T get(String key, Type typeOfT) {
		String v = get(key);
		if (StringUtil.isNullOrEmpty(v))
			return null;
		return JsonUtil.fromJson(v, typeOfT);
	}
	
	public void set(String key, String value) {
		Jedis jedis = null;
		try {
			jedis = pool.getResource();
			jedis.set(key, value);
		} catch (Exception e) {
//			logger.error("", e);
		} finally {
			returnJedis(jedis);
		}
	}
	
	public void set(String key, Object value) {
		set(key, JsonUtil.toJson(value));
	}
	
	public void setex(String key, String value, int expireTime) {
		Jedis jedis = null;
		try {
			jedis = pool.getResource();
			jedis.setex(key, expireTime, value);
		} catch (Exception e) {
//			logger.error("", e);
		} finally {
			returnJedis(jedis);
		}
	}
	
	public void setex(String key, Object value, int expireTime) {
		setex(key, JsonUtil.toJson(value), expireTime);
	}
	
	public void hincrByexAt(String key, String field, long value,long expireTime) {
		Jedis jedis = null;
		try {
			jedis = pool.getResource();
			jedis.hincrBy(key, field, value);
			jedis.expireAt(key, expireTime);
		} catch (Exception e) {
//			logger.error("", e);
		} finally {
			returnJedis(jedis);
		}
	}
	
	public boolean isExits(String key) {
		Jedis jedis = null;
		try {
			jedis = pool.getResource();
			return jedis.exists(key);
		} catch (Exception e) {
//			logger.error("", e);
		} finally {
			returnJedis(jedis);
		}
		return false;
	}
	
	public long delete(String... keys) {
		Jedis jedis = null;
		try {
			jedis = pool.getResource();
			return jedis.del(keys);
		} catch (Exception e) {
//			logger.error("", e);
		} finally {
			returnJedis(jedis);
		}
		return -1l;
	}
	
	public long hashSet(String key, String field, String value) {
		Jedis jedis = null;
		try {
			jedis = pool.getResource();
			return jedis.hset(key, field, value);
		} catch (Exception e) {
//			logger.error("", e);
		} finally {
			returnJedis(jedis);
		}

		return -1l;
	}
	
	public long hashSet(String key, String field, Object value) {
		Jedis jedis = null;
		try {
			jedis = pool.getResource();
			return jedis.hset(key, field, JsonUtil.toJson(value));
		} catch (Exception e) {
//			logger.error("", e);
		} finally {
			returnJedis(jedis);
		}

		return -1l;
	}
	
	public String hashGet(String key, String field) {
		String result = null;
		Jedis jedis = null;
		try {
			jedis = pool.getResource();
			result = jedis.hget(key, field);
		} catch (Exception e) {
//			logger.error("", e);
		} finally {
			returnJedis(jedis);
		}
		return result;
	}
	
	public Map<String, String> hashGetAll(String key) {
		Map<String, String> result = null;
		Jedis jedis = null;
		try {
			jedis = pool.getResource();
			result = jedis.hgetAll(key);
		} catch (Exception e) {
//			logger.error("", e);
		} finally {
			returnJedis(jedis);
		}
		return result;
	}
	
	public <T> Map<String, T> hashGetAll(String key, Class<T> classType) {
		Map<String, String> hash = hashGetAll(key);
		if(hash != null)
		{
			Map<String, T> new_hash = new HashMap<String, T>();
			Iterator<Entry<String, String>> it = hash.entrySet().iterator();
			while(it.hasNext())
			{
				Entry<String, String> entry = it.next();
				new_hash.put(entry.getKey(), JsonUtil.fromJson(entry.getValue(), classType));
			}
			return new_hash;
		}
		return null;
	}
	
	public <T> List<T > listGet(String key, Class<T> classType) {
		Jedis jedis = null;
		try {
			jedis = pool.getResource();
			List<String> result = jedis.lrange(key, 0, -1);
			if(result != null)
			{
				List<T > list = new ArrayList<>();
				for (int i = 0; i < result.size(); i++) {
					String json_str = result.get(i);
					T obj = JsonUtil.fromJson(json_str, classType);
					list.add(obj);
				}
				return list;
			}
			
		} catch (Exception e) {
//			logger.error("", e);
		} finally {
			returnJedis(jedis);
		}
		return null;
	}
	
	public void listAdd(String key, Object val) {
		Jedis jedis = null;
		try {
			jedis = pool.getResource();
			jedis.rpush(key, JsonUtil.toJson(val));
		} catch (Exception e) {
//			logger.error("", e);
		} finally {
			returnJedis(jedis);
		}
	}
	
	public void listAdd(String key, Object val, int max) {
		Jedis jedis = null;
		try {
			jedis = pool.getResource();
			jedis.rpush(key, JsonUtil.toJson(val));
			Long len = jedis.llen(key);
			if(len > max)
			{
				jedis.lpop(key);
			}
		} catch (Exception e) {
//			logger.error("", e);
		} finally {
			returnJedis(jedis);
		}
	}
	
	public int listSize(String key) {
		int len = 0;
		Jedis jedis = null;
		try {
			jedis = pool.getResource();
			len = jedis.llen(key).intValue();
		} catch (Exception e) {
//			logger.error("", e);
		} finally {
			returnJedis(jedis);
		}
		return len;
	}
	
	private void returnJedis(Jedis jedis) {
		if (jedis != null) {
			this.pool.returnResource(jedis);
		}
	}
	
	public void flushAll() {
		Jedis jedis = null;
		try {
			jedis = pool.getResource();
			jedis.flushAll();
		} catch (Exception e) {
//			logger.error("", e);
		} finally {
			returnJedis(jedis);
		}
	}

	public void flushDB() {
		Jedis jedis = null;
		try {
			jedis = pool.getResource();
			jedis.flushDB();
		} catch (Exception e) {
//			logger.error("", e);
		} finally {
			returnJedis(jedis);
		}
	}
	
	public void close()
	{
		if (pool != null) 
		{
			pool.destroy();
			pool = null;
		}
	}
	
	/**
	 * 批量设置HASH表的键和值 不序列化
	 */
	public int hashMultiSet(String key, Map<String, String> keyValues) {
		if (keyValues.size() > 0) {
			Jedis jedis = null;
			try {
				jedis = pool.getResource();
				String value = jedis.hmset(key, keyValues);
				if (value.equalsIgnoreCase("ok")) {
					return 1;
				}
			} catch (Exception e) {
				e.printStackTrace();
			} finally {
				returnJedis(jedis);
			}
		}
		return -1;
	}
	


}
