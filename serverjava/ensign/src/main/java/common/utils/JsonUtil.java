package common.utils;

import java.lang.reflect.Type;

import com.google.gson.Gson;
import com.google.gson.GsonBuilder;
import com.google.gson.JsonSyntaxException;

public class JsonUtil {

	/**
	 * 将java对象转换成json格式的字符串
	 * @param src
	 * @return
	 */
	public static String toJson(Object src) {
		return new Gson().toJson(src);
	}
	
	public static String toJson(Object src,Type typeOfSrc){
		return new Gson().toJson(src, typeOfSrc);
	}
	
	
	public static String toJsonWithExpose(Object src) {
		GsonBuilder bulider = new GsonBuilder();
		bulider.excludeFieldsWithoutExposeAnnotation();
		Gson gson = bulider.create();
		return gson.toJson(src);
	}
	
	
	public static String toJsonWithExpose(Object src,Type typeOfSrc) {
		GsonBuilder bulider = new GsonBuilder();
		bulider.excludeFieldsWithoutExposeAnnotation();
		Gson gson = bulider.create();
		return gson.toJson(src,typeOfSrc);
	}
	/**
	 * 将json格式的字符串转换成java对象
	 * @param json
	 * @param classOfT
	 * @return
	 */
	public static <T> T fromJson(String json, Class<T> classOfT) {
		try {
			return new Gson().fromJson(json, classOfT);
		} catch (JsonSyntaxException e) {
			//e.printStackTrace();
			////System.out.println(e.getMessage());
		}
		return null;
	}
	
	/**
	 * 将json格式的字符串转换成java对象
	 * @param json
	 * @param typeOfT
	 * @return
	 */
	
	public static <T> T fromJson(String json, Type typeOfT) {
		try {
			Gson gson = new Gson();
			return gson.fromJson(json, typeOfT);
		} catch (Exception e) {
			e.printStackTrace();
		}
		return null;
	}


}
