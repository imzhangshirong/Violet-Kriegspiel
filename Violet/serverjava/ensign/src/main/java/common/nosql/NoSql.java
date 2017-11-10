package common.nosql;

public interface NoSql {

	public void hashSet(String key,String field,String value) throws Exception;
	
	public String hashGet(String key, String field) throws Exception;
	
	public void set(String key, String value) throws Exception;
	
	public String get(String key) throws Exception;
}
