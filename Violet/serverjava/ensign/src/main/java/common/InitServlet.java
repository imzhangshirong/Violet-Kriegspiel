package common;

import javax.servlet.ServletConfig;
import javax.servlet.ServletException;
import javax.servlet.http.HttpServlet;

import org.springframework.beans.factory.BeanFactory;
import org.springframework.web.context.support.WebApplicationContextUtils;

public class InitServlet extends HttpServlet{

	/**
	 * 
	 */
	private static final long serialVersionUID = 317355317856436373L;

	@Override
	public void init(ServletConfig config) throws ServletException{
		super.init(config);
		
		try {
			BeanFactory context = WebApplicationContextUtils.getWebApplicationContext(config.getServletContext());
			
			ConfigurationUtil.beanFactory = context;
			
			System.out.println("\n\n\n" +
					"	   _________ __                  __              ._.                         " + "\n" + 
					"	  /   _____/_| |_______ ________/  |_ __ ________| |                         " + "\n" + 
					"	  |_____  ||_  __||__  ||_  __ |   __|  |   | __/| |                         " + "\n" + 
					"	 /        | |  |  / __ ||  | |/ |  | |  |  /| |_> >|                         " + "\n" + 
					"	/_______  / |__| (____ /|__|    |__| |____/ |  __/_|                         " + "\n" + 
					"	        |/           |/                     |__| |/        "
					+ "\n\n\n");
		}catch (Exception e) {
			e.printStackTrace();
		}
	}
	
	@Override
	public void destroy() {
		try {
			super.destroy();
			System.exit(0);
		} catch (Exception e) {
			e.printStackTrace();
		}
	}
}
