package common.db;

import java.io.Reader;

import org.apache.ibatis.io.Resources;
import org.apache.ibatis.session.SqlSession;
import org.apache.ibatis.session.SqlSessionFactory;
import org.apache.ibatis.session.SqlSessionFactoryBuilder;

public class DB {

	private SqlSessionFactory sqlSessionFactory = null;
	
	private SqlSession session = null;
	
	private String name;

	public DB(String name, String config_path) 
	{
		this.name = name;
		
		try
		{
//			Reader reader = new FileReader(config_path);
			Reader reader = Resources.getResourceAsReader(config_path);
			sqlSessionFactory = new SqlSessionFactoryBuilder().build(reader);
		}
		catch(Exception e)
		{
			e.printStackTrace();
		}
	}

	public String getName() {
		return name;
	}

	public synchronized SqlSession getSession()
	{
		session = sqlSessionFactory.openSession();
		return session;
	}

	public void close()
	{
		if(session != null)
		{
			session.close();
			session = null;
		}
	}
}
