package connection;

import java.io.BufferedReader;
import java.io.DataInputStream;
import java.io.DataOutputStream;
import java.io.InputStreamReader;
import java.net.Socket;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

public class Client {

	private static final Logger logger = LoggerFactory.getLogger(Client.class.getName());
	
	public static final String ip = "127.0.0.1";
	
	public static final int port = 8000;
	
	public static void main(String[] args) {
		System.out.println("客户端启动");
		System.out.println("当接受到服务器端字符为OK的时候，客户端将终止");
		while (true) {
			Socket socket = null;
			try {
				socket = new Socket(ip, port);
				//读取服务器消息
				DataInputStream input = new DataInputStream(socket.getInputStream());
				//向服务器发送数据
				DataOutputStream out = new DataOutputStream(socket.getOutputStream());
				System.out.println("请输入:");
				String str = new BufferedReader(new InputStreamReader(System.in)).readLine();
				out.writeUTF(str);
				
				String ret = input.readUTF();
				System.out.println("服务器返回过来的是:" + ret);
				if ("OK".equals(ret)) {
					System.out.println("客户端将关闭连接");
					Thread.sleep(10000);
					break;
				}
				
				out.close();
				input.close();
			} catch (Exception e) {
				logger.info("客户端异常");
			}finally {
				if (socket != null) {
					try {
						System.out.println("连接关闭");
						socket.close();
					} catch (Exception e2) {
						logger.info("客户端finally异常");
					}
				}
			}
		}
	}

}
