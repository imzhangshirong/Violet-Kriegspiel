package common;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

import org.apache.commons.configuration.Configuration;
import org.apache.commons.configuration.ConfigurationException;
import org.apache.commons.configuration.ConfigurationFactory;
import org.springframework.beans.factory.BeanFactory;

import ensign.pojo.User;

public class ConfigurationUtil {

	public static BeanFactory beanFactory;
	
	private static final ConfigurationFactory factory = new ConfigurationFactory("propertyConfig.xml");
	private static Configuration config ;
	
	public static Map<String, User> tokenMap = new HashMap<>();
	public static List<String> tokenList = new ArrayList<>();
	
	static{
		try {
			config = factory.getConfiguration();
		} catch (ConfigurationException e) {
			e.printStackTrace();
		}
	}
	//redis
	public static final String APP_REDIS_HOST = config.getString("app.redis.addr");
	public static final int APP_REDIS_PROT = config.getInt("app.redis.port");
	
}
