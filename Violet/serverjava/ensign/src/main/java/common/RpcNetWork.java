package common;

public class RpcNetWork {

	public int headerLength = 10;
	
	public class SocketData {
		public int version;
		public int dataLength;
		public byte[] data;
	}
	
	//打包
	public byte[] buildSocketData(SocketData socketData) {
		
		byte[] bufferArray = new byte[headerLength + socketData.dataLength + 2];
		//头标识
		bufferArray[0] = (byte) 255; 
		//结束标识
		bufferArray[bufferArray.length - 2] = 0;
		bufferArray[bufferArray.length - 1] = (byte) 254;
		bufferArray[1] = (byte) socketData.version;
		switch (socketData.version) {
		case 1:
			writeDataLength(bufferArray, socketData.dataLength, 2, 3);
			break;
		}
		for (int i = 0; i < socketData.data.length; i ++) {
			bufferArray[headerLength + i] = socketData.data[i];
		}
		return bufferArray;
	}
	
	//解包
	public SocketData parseSocketData(byte[] bufferArray) {

		SocketData sd = new SocketData();
		if (bufferArray[0] == -1) {
			sd.version = bufferArray[1];

			switch (sd.version) {
			case 1:
				sd.dataLength = getDataLength(bufferArray, 2, 3);
				break;
			}
			byte[] data = new byte[sd.dataLength];
			for (int i = 0; i < data.length; i++) {
				data[i] = bufferArray[headerLength + i];
			}
			sd.data = data;
			return sd;
		}
		return null;
	}
	
	private int getDataLength(byte[] bufferArray, int offset, int length) {
		int dataLength = 0;
		int baseSize = 1;
		for (int i = length - 1; i >=0; i--) {
			dataLength += bufferArray[offset + i]* baseSize;
			baseSize *= 256;
		}
		return dataLength;
	}
	
	private void writeDataLength(byte[] bufferArray, int dataLength, int offset, int length) {
		int l = dataLength;
		int d = 0;
		for (int i = length - 1; i >= 0; i--) {
			d = l % 256;
			l = l / 256;
			bufferArray[offset + i] = (byte) d;
		}
	}
}
