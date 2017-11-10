package connection;

import java.io.DataInputStream;
import java.io.DataOutputStream;
import java.io.IOException;
import java.net.ServerSocket;
import java.net.Socket;
import java.util.ArrayList;
import java.util.List;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.context.ApplicationContext;
import org.springframework.context.support.ClassPathXmlApplicationContext;

import com.google.protobuf.ByteString;
import com.violet.rpc.Client.HelloRequest;
import com.violet.rpc.Client.HelloResponse;
import com.violet.rpc.Client._Request;
import com.violet.rpc.Client._Response;

import common.BaseServlet;
import common.ConfigurationUtil;
import common.RpcNetWork;

public class Server{
	
	private static final Logger logger = LoggerFactory.getLogger(Server.class.getName());
	
	public static final int port = 8000;
	
	public static Socket socket;
	
	public static List<byte[]> byteArrayList = new ArrayList<byte[]>();
	
	public void init() {
		ApplicationContext context = new ClassPathXmlApplicationContext("classpath:applicationContext.xml");
		ConfigurationUtil.beanFactory = context;
		System.out.println("\n\n\n" +
				"	   _________ __                  __              ._.                         " + "\n" + 
				"	  /   _____/_| |_______ ________/  |_ __ ________| |                         " + "\n" + 
				"	  |_____  ||_  __||__  ||_  __ |   __|  |   | __/| |                         " + "\n" + 
				"	 /        | |  |  / __ ||  | |/ |  | |  |  /| |_> >|                         " + "\n" + 
				"	/_______  / |__| (____ /|__|    |__| |____/ |  __/_|                         " + "\n" + 
				"	        |/           |/                     |__| |/        "
				+ "\n\n\n");
	}

	public void run() {
		try {
			ServerSocket serverSocket = new ServerSocket(port);
			while (true) {
				socket = serverSocket.accept();
				System.out.println("连接完成");
				new HandlerThread(socket);
			}
		} catch(Exception e) {
			logger.info("服务器异常" + e.getMessage());
		}
	}
	
	private class HandlerThread implements Runnable {
		private Socket socket;
		public HandlerThread (Socket client) {
			socket = client;
			new Thread(this).start();
		}
		
		public void run() {
			try {
//				new receiveThread();
//				new readThread();
//				receiveData();
//				readData();
//				receiveData();
//				readData();
//				 读取客户端数据
//				DataInputStream input = new DataInputStream(socket.getInputStream());
//				byte[] b = new byte[1024];
//				int size = input.read(b);
//				System.out.println("byte长度:" + size);
//				RpcNetWork rpc = new RpcNetWork();
//				RpcNetWork.SocketData socketData = rpc.new SocketData();
//				socketData = rpc.parseSocketData(b);
//				_Request req = _Request.parser().parseFrom(socketData.data);
//				ByteString bs = req.getData();
//				byte[] data = bs.toByteArray();
//				HelloRequest hq = HelloRequest.parser().parseFrom(data);
//				System.out.println("客户端发过来的内容:" + hq.getContent());
//
////				 向客户端回复信息
//				DataOutputStream out = new DataOutputStream(socket.getOutputStream());
//				HelloResponse.Builder res = HelloResponse.newBuilder();
//				res.setGreet("longwei");
//				ByteString resbs = res.build().toByteString();
//				_Response.Builder response = _Response.newBuilder();
//				response.setToken("2");
//				response.setCode(0);
//				response.setRpc("rpc");
//				response.setData(resbs);
//				byte[] byteRes = response.build().toByteArray();
//				socketData.dataLength = byteRes.length;
//				socketData.data = byteRes;
//				byte[] sendData = rpc.buildSocketData(socketData);
//				out.write(sendData);
				
//				out.close();
//				input.close();
			} catch (Exception e) {
				e.printStackTrace();
			} 
//				finally {
//				if (socket != null) {
//					try {
//						socket.close();
//					} catch (Exception e2) {
//						logger.info("服务器finally异常");
//					}
//				}
//			}
		}
	}
	
	private class receiveThread implements Runnable {
		public receiveThread() {
			new Thread(this).start();
		}
		
		public void run() {
			receiveData();
		}
	}
	
	private class readThread implements Runnable {
		public readThread() {
			new Thread(this).start();
		}
		
		public void run() {
			readData();
		}
	}
	
	private void receiveData() {
		int blockSize = 10;
		while(true) {
			if(!socket.isConnected()) break;
			byte[] buffer = new byte[blockSize];
			try {
				DataInputStream input = new DataInputStream(socket.getInputStream());
				input.read(buffer);
//				System.out.println("收到数据");
				synchronized (byteArrayList) {
					byteArrayList.add(buffer);
				}
//				System.out.println(byteArrayList.size());
			} catch (Exception e) {
				e.printStackTrace();;
				break;
			}
		}
	}
	
	private void readData() {
//		System.out.println("开始读数据");
		while (true) {
			if (!socket.isConnected())
				break;
//			System.out.println(byteArrayList.size());
			try {
				Thread.sleep(5000);
			} catch (Exception e) {
				// TODO: handle exception
			}
			if (byteArrayList.size() > 0) {
				synchronized (byteArrayList) {
//					System.out.println("开始读数据");
					byte[] dataBuffer = byteArrayList.get(0);// 获取第一个包
					int endPoint = -1;
					if (dataBuffer[0] == -1) {// 检测是否为包头
						endPoint = getEndPoint(dataBuffer);
						int blockRange = 0;
						for (int i = 1; i < byteArrayList.size(); i++) {
							if (endPoint > -1)
								break;
							byte[] tempBuffer = new byte[dataBuffer.length + byteArrayList.get(i).length];
							for (int j = 0; j < dataBuffer.length; j++) {
								tempBuffer[j] = dataBuffer[j];
							}
							for (int j = 0; j < byteArrayList.get(i).length; j++) {
								tempBuffer[j + dataBuffer.length] = byteArrayList.get(i)[j];
							}
							dataBuffer = tempBuffer;
							blockRange++;
							endPoint = getEndPoint(dataBuffer);
						}

						if (endPoint > 1) {
							byte[] messageBuffer = new byte[endPoint + 1];
							for (int j = 0; j < messageBuffer.length; j++) {
								messageBuffer[j] = dataBuffer[j];
							}
							byte[] remainBuffer = new byte[dataBuffer.length - messageBuffer.length];
							int empty = 0;
							for (int j = 0; j < remainBuffer.length; j++) {
								remainBuffer[j] = dataBuffer[messageBuffer.length + j];
								if (remainBuffer[j] == 0)
									empty++;
							}
							if (empty != remainBuffer.length) {
								byteArrayList = byteArrayList.subList(blockRange, byteArrayList.size() + 1);
								byteArrayList.set(0, remainBuffer);
							} else {
								if (byteArrayList.size() <= blockRange + 1) {
									byteArrayList.removeAll(byteArrayList);
								} else {
									byteArrayList = byteArrayList.subList(blockRange + 1, byteArrayList.size() + 1);
								}
							}
							dealSocketData(messageBuffer);
						} else {
							byteArrayList = byteArrayList.subList(blockRange, byteArrayList.size() + 1);
							byteArrayList.set(0, dataBuffer);
						}
					} else {
						byteArrayList.remove(0);// 丢弃当前包
					}
				}
			}
		}
	}
	
	private int getEndPoint(byte[] data) {
		for(int i = 0; i < data.length - 1; i ++) {
			if(data[i] == 0 && data[i+1] == -2) return i + 1;
		}
		return -1;
	}
	
	private void dealSocketData(byte[] data) {
		try {
			RpcNetWork rpc = new RpcNetWork();
			RpcNetWork.SocketData socketData = rpc.new SocketData();
			socketData = rpc.parseSocketData(data);
			_Request req = _Request.parser().parseFrom(socketData.data);
			onRequestRpc(req.getToken(), req.getRpc(), req.getData(), rpc);
//			byte[] bsData = req.getData().toByteArray();
//			HelloRequest hq = HelloRequest.parser().parseFrom(bsData);
//			System.out.println("客户端发过来的内容:" + hq.getContent());
		} catch (Exception e) {
			System.out.println("dealSocketData错误");
		}
	}
	
	private void onRequestRpc(final String token,final String name,final ByteString buffer, RpcNetWork rpc) {
		try {
			ByteString result = null;
			BaseServlet servlet = (BaseServlet) ConfigurationUtil.beanFactory.getBean(name);
			result = servlet.service(rpc, buffer);
			onResponseRpc(token, 0, name,result,rpc);
		} catch (Exception e) {
			e.printStackTrace();
		}
	}
	
	public void onResponseRpc(String token,int errorCode,String name,ByteString result, RpcNetWork rpc) throws IOException {
		try {
			DataOutputStream out = new DataOutputStream(socket.getOutputStream());
			_Response.Builder response = _Response.newBuilder();
			response.setToken(token);
			response.setCode(0);
			response.setRpc(name);
			response.setData(result);
			byte[] byteRes = response.build().toByteArray();
			RpcNetWork.SocketData socketData = rpc.new SocketData();
			socketData.dataLength = byteRes.length;
			socketData.data = byteRes;
			byte[] sendData = rpc.buildSocketData(socketData);
			out.write(sendData);
			out.flush();
		} catch (Exception e) {
			e.printStackTrace();
		}
	}
	
	private void Connected() {
		
	}
	
	private void disConnect() {
		
	}
	
	public static void main(String[] args) {
		Server connection = new Server();
		connection.init();
		connection.run();
	}

}
