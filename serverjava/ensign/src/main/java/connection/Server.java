package connection;

import java.io.DataInputStream;
import java.io.DataOutputStream;
import java.net.ServerSocket;
import java.net.Socket;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.Random;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.context.ApplicationContext;
import org.springframework.context.support.ClassPathXmlApplicationContext;

import com.google.protobuf.ByteString;
import com.google.protobuf.TextFormat;
import com.violet.rpc.Push.EnterBattleFieldPush;
import com.violet.rpc.Struct.ChessData;
import com.violet.rpc.Struct.PlayerInfo;
import com.violet.rpc.Struct._Request;
import com.violet.rpc.Struct._Response;

import common.BaseServlet;
import common.ConfigurationUtil;
import common.Constats;
import common.RpcNetWork;
import common.db.DBUtil;
import ensign.pojo.RoomInfo;
import ensign.pojo.User;

public class Server{
	
	private static final Logger logger = LoggerFactory.getLogger(Server.class.getName());
	
	public static final int port = 8000;
	
	public static List<byte[]> byteArrayList = new ArrayList<byte[]>();
	
	public static Map<String, Socket> socketMap = new HashMap<>();
	
	public void init() {
		ApplicationContext context = new ClassPathXmlApplicationContext("classpath:applicationContext.xml");
		ConfigurationUtil.beanFactory = context;
		DBUtil.GetInstance().init();
		new PushThread();
		System.out.println("\n\n\n" +
				"	   _________ __                  __              ._.                         " + "\n" + 
				"	  /   _____/_| |_______ ________/  |_ __ ________| |                         " + "\n" + 
				"	  |_____  ||_  __||__  ||_  __ |   __|  |   | __/| |                         " + "\n" + 
				"	 /        | |  |  / __ ||  | |/ |  | |  |  /| |_> >|                         " + "\n" + 
				"	/_______  / |__| (____ /|__|    |__| |____/ |  __/_|                         " + "\n" + 
				"	        |/           |/                     |__| |/        "
				+ "\n\n\n");
		service();
	}

	public void service() {
		ServerSocket serverSocket = null;
		try {
			serverSocket = new ServerSocket(port);
			while (true) {
				Socket socket = null;
				socket = serverSocket.accept();
				new ReceiveThread(socket);
				new ReadThread(socket);
				System.out.println("连接完成");
			}
		} catch (Exception e) {
			System.out.println("断开");
		}
	}
	
	private class ReceiveThread implements Runnable {
		private Socket socket;
		public ReceiveThread(Socket client) {
			this.socket = client;
			new Thread(this).start();
		}
		
		public void run() {
			receiveData(socket);
		}
	}
	
	private class ReadThread implements Runnable {
		private Socket socket;
		public ReadThread(Socket client) {
			this.socket = client;
			new Thread(this).start();
		}
		
		public void run() {
			readData(socket);
		}
	}
	
	private void receiveData(Socket socket) {
		try {
			DataInputStream input = null;
			int blockSize = 50;
			while (true) {
				if (!socket.isConnected())
					break;
				byte[] buffer = new byte[blockSize];
				if (input == null) {
					input = new DataInputStream(socket.getInputStream());
				}
				while (input.read(buffer) > 0) {
					synchronized (byteArrayList) {
						byteArrayList.add(buffer);
					}
					buffer = new byte[blockSize];
				}
			} 
		} catch (Exception e) {
			e.printStackTrace();
		}
	}
	
	private void readData(Socket socket) {
		while (true) {
			if (!socket.isConnected())
				break;
			try {
				Thread.sleep(500);
			} catch (Exception e) {
				e.printStackTrace();
			}
			if (byteArrayList.size() > 0) {
				synchronized (byteArrayList) {
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
								byteArrayList = byteArrayList.subList(blockRange, byteArrayList.size());
								byteArrayList.set(0, remainBuffer);
							} else {
								if (byteArrayList.size() <= blockRange + 1) {
									byteArrayList.removeAll(byteArrayList);
								} else {
									byteArrayList = byteArrayList.subList(blockRange + 1, byteArrayList.size());
								}
							}
							dealSocketData(messageBuffer, socket);
						} else {
							byteArrayList = byteArrayList.subList(blockRange, byteArrayList.size());
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
	
	private void dealSocketData(byte[] data, Socket socket) {
		try {
//			RpcNetWork rpc = new RpcNetWork();
//			RpcNetWork.SocketData socketData = rpc.new SocketData();
//			socketData = rpc.parseSocketData(data);
//			_Request req = _Request.parser().parseFrom(socketData.data);
//			onRequestRpc(req.getToken(), req.getRpc(), req.getData(),req.getUnique(), rpc);
			RpcNetWork rpc = new RpcNetWork();
			RpcNetWork.SocketData socketData = rpc.new SocketData();
			socketData = rpc.parseSocketData(data);
			_Request req = _Request.parser().parseFrom(socketData.data);
			
			List<Object> result = null;
			BaseServlet servlet = (BaseServlet) ConfigurationUtil.beanFactory.getBean(req.getRpc());
			result = servlet.service(req.getData(), req.getToken());
			if (req.getRpc().equals("Login")) {
				socketMap.put((String) result.get(1), socket);
			}
			DataOutputStream out = new DataOutputStream(socket.getOutputStream());
			_Response.Builder response = _Response.newBuilder();
			response.setToken((String) result.get(1));
			response.setCode(0);
			response.setRpc(req.getRpc());
			response.setData((ByteString) result.get(0));
			response.setUnique(req.getUnique());
			byte[] byteRes = response.build().toByteArray();
			socketData.dataLength = byteRes.length;
			socketData.data = byteRes;
			byte[] sendData = rpc.buildSocketData(socketData);
			out.write(sendData);
			out.flush();
		} catch (Exception e) {
			e.printStackTrace();
		}
	}
	
	private class PushThread implements Runnable {
		public PushThread() {
			new Thread(this).start();
		}
		
		public void run() {
			while (true) {
				try {
					Thread.sleep(1000);
				} catch (Exception e) {
					e.printStackTrace();
				}
				if (ConfigurationUtil.tokenList.size() > 1) {
					int p1 = new Random().nextInt(ConfigurationUtil.tokenList.size());
					int p2 = new Random().nextInt(ConfigurationUtil.tokenList.size());
					while (p1 == p2) {
						p2 = new Random().nextInt(ConfigurationUtil.tokenList.size());
					}
					String t1 = ConfigurationUtil.tokenList.get(p1);
					String t2 = ConfigurationUtil.tokenList.get(p2);
					User u1 = ConfigurationUtil.tokenMap.get(t1);
					User u2 = ConfigurationUtil.tokenMap.get(t2);
					PlayerInfo.Builder playerInfo1 = PlayerInfo.newBuilder();
					PlayerInfo.Builder playerInfo2 = PlayerInfo.newBuilder();
					playerInfo1.setZoneId(1000);
					playerInfo1.setUserId(u1.getId());
					playerInfo1.setUserName(u1.getUserName());
					playerInfo1.setState(1);//未准备
					playerInfo2.setZoneId(1000);
					playerInfo2.setUserId(u2.getId());
					playerInfo2.setUserName(u2.getUserName());
					playerInfo2.setState(1);
					String roomId = t1 + t2;
					RoomInfo roomInfo = new RoomInfo();
					roomInfo.setRoomId(roomId);
					u1.setRoomId(roomId);
					u2.setRoomId(roomId);
					ConfigurationUtil.tokenMap.put(t1, u1);
					ConfigurationUtil.tokenMap.put(t2, u2);
					List<User> userList = new ArrayList<>();
					userList.add(u1);
					userList.add(u2);
					roomInfo.setUserList(userList);
					DBUtil.GetInstance().setRoomInfo(roomInfo);
					EnterBattleFieldPush.Builder enterRoomPush1 = EnterBattleFieldPush.newBuilder();
//					enterRoomPush1.addPlayerList(playerInfo1.build());
					enterRoomPush1.addPlayerList(playerInfo2.build());
					List<ChessData> chessList1 = Constats.initChessData(1000+"/" + u1.getId());
					enterRoomPush1.addAllChessSetting(chessList1);
					enterRoomPush1.setRoomId(roomId);
					pushToClient(t1, enterRoomPush1.build().toByteString(), "EnterBattleField");
					EnterBattleFieldPush.Builder enterRoomPush2 = EnterBattleFieldPush.newBuilder();
					enterRoomPush2.addPlayerList(playerInfo1.build());
//					enterRoomPush2.addPlayerList(playerInfo2.build());
					List<ChessData> chessList2 = Constats.initChessData(1000 + "/" + u2.getId());
					enterRoomPush2.addAllChessSetting(chessList2);
					enterRoomPush2.setRoomId(roomId);
					pushToClient(t2, enterRoomPush2.build().toByteString(), "EnterBattleField");
					ConfigurationUtil.tokenList.remove(t1);
					ConfigurationUtil.tokenList.remove(t2);
					System.out.println(TextFormat.printToUnicodeString(enterRoomPush1.build()));
					System.out.println(TextFormat.printToUnicodeString(enterRoomPush2.build()));
				} 
			}
		}
	}
	
	public static void pushToClient(String token, ByteString buffer, String name) {
		try {
			RpcNetWork rpc = new RpcNetWork();
			RpcNetWork.SocketData socketData = rpc.new SocketData();
			DataOutputStream out = new DataOutputStream(socketMap.get(token).getOutputStream());
			_Response.Builder response = _Response.newBuilder();
			response.setToken(token);
			response.setCode(0);
			response.setRpc(name);
			response.setData(buffer);
			response.setUnique(0);
			byte[] byteRes = response.build().toByteArray();
			socketData.version = 1;
			socketData.dataLength = byteRes.length;
			socketData.data = byteRes;
			byte[] sendData = rpc.buildSocketData(socketData);
			out.write(sendData);
		} catch (Exception e) {
			e.printStackTrace();
		}
	}
	
//	private void onRequestRpc(String token, String name, ByteString buffer, int unique, RpcNetWork rpc) {
//		try {
//			ByteString result = null;
//			BaseServlet servlet = (BaseServlet) ConfigurationUtil.beanFactory.getBean(name);
//			result = servlet.service(buffer, token);
//			onResponseRpc(token, 0, name,result, unique,rpc);
//		} catch (Exception e) {
//			e.printStackTrace();
//		}
//	}
//	
//	public void onResponseRpc(String token,int errorCode, String name,ByteString result, int unique, RpcNetWork rpc) throws IOException {
//		try {
//			DataOutputStream out = new DataOutputStream(socket.getOutputStream());
//			_Response.Builder response = _Response.newBuilder();
//			response.setToken(token);
//			response.setCode(errorCode);
//			response.setRpc(name);
//			response.setData(result);
//			response.setUnique(unique);
//			byte[] byteRes = response.build().toByteArray();
//			RpcNetWork.SocketData socketData = rpc.new SocketData();
//			socketData.dataLength = byteRes.length;
//			socketData.data = byteRes;
//			byte[] sendData = rpc.buildSocketData(socketData);
//			out.write(sendData);
//		} catch (Exception e) {
//			e.printStackTrace();
//		}
//	}
	
	public static void main(String[] args) {
		Server connection = new Server();
		connection.init();
	}

}
