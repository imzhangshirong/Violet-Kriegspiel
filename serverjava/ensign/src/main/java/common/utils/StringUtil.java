package common.utils;

import java.util.regex.Matcher;
import java.util.regex.Pattern;

public class StringUtil {

	/**
	 * 是否为null或者""
	 * @param str
	 * @return
	 */
	public static boolean isNullOrEmpty(String str)
	{
		if(str == null || str.equals(""))
		{
			return true;
		}
		return false;
	}
	

	/**
	 * @param text 需要查询的字符串
	 * @param keyWord 关键字
	 * @return 关键字在所需查询的字符串的索引值
	 */
	public static int HotIndex(String text, String keyWord) {
		int index = -1;
		if (!text.contains(keyWord)) {
			return -1;
		} else {
			index = text.indexOf(keyWord);
			return index;
		}
	}

	/**
	 * 首字母大写,得到方法名
	 * @param str
	 * @return
	 */
	public static String FirstLetterToUpper(String str){
		char[] array = str.toCharArray();
		array[0] -= 32;
		return String.valueOf(array);
	}

	/**
	 * 首字母小写
	 * @param str
	 * @return
	 */
	public static String FirstLetterToLower(String str){
		char[] array = str.toCharArray();
		array[0] += 32;
		return String.valueOf(array);
	}
	
	/**
	 * 所有字母小写
	 * A~Z 65~90
	 * a~z 97~122
	 * @param str
	 * @return
	 */
	public static String AllLetterToLower(String str){
		char[] array = str.toCharArray();
		{
			for(int i = 0, size = array.length; i < size; i ++)
			{
				if(array[i] >= 65 && array[i] <= 90)
				{
					array[i] += 32;
				}
			}
		}
		return String.valueOf(array);
	}

	/**
	 * 判断字符串是否是数字
	 * @param str
	 * @return
	 */
	public static boolean isNumeric(String str)
	{ 
		if("".equals(str))
		{
			return false ;
		}
		else
		{
			if(str.matches("\\d*"))
			{
				return true; 
			}
			else
			{
				try
				{
					Integer.parseInt(str);
				}
				catch (Exception e) {
					return false;
				}
				return true;
			}
		}
		
	}

	//判断邮箱格式
	public static boolean checkEmail(String strEmail) { 
		if(strEmail == null)
		{
			return false;
		}
		String strPattern = "^([a-zA-Z0-9_\\.-])+@(([a-zA-Z0-9-])+\\.)+([a-zA-Z0-9])+$"; 
		Pattern p = Pattern.compile(strPattern); 
		Matcher m = p.matcher(strEmail); 
		return m.matches(); 
	}

}
