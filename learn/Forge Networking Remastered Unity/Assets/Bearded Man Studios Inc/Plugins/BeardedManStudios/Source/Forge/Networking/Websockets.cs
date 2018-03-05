/*-----------------------------+-------------------------------\
|                                                              |
|                         !!!NOTICE!!!                         |
|                                                              |
|  These libraries are under heavy development so they are     |
|  subject to make many changes as development continues.      |
|  For this reason, the libraries may not be well commented.   |
|  THANK YOU for supporting forge with all your feedback       |
|  suggestions, bug reports and comments!                      |
|                                                              |
|                              - The Forge Team                |
|                                Bearded Man Studios, Inc.     |
|                                                              |
|  This source code, project files, and associated files are   |
|  copyrighted by Bearded Man Studios, Inc. (2012-2017) and    |
|  may not be redistributed without written permission.        |
|                                                              |
\------------------------------+------------------------------*/

using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace BeardedManStudios.Forge.Networking
{
	/// <summary>
	/// A static class of helper methods to setup and handle Websocket features
	/// </summary>
	public static class Websockets
	{
		public static byte[] ConnectionHeader(string headerHash, ushort port)
		{
			// This is a typical Websockets accept header to be validated
			byte[] connectHeader = Encoding.UTF8.GetBytes("GET / HTTP/1.1\r\n" +
				"Host: http://developers.forgepowered.com:" + port.ToString() + "\r\n" +
				"Upgrade: websocket\r\n" +
				"Connection: Upgrade\r\n" +
				"Sec-WebSocket-Key: " + headerHash + "\r\n" +
				"Sec-WebSocket-Version: 13\r\n");

			return connectHeader;
		}

		public static bool ValidateResponseHeader(string headerHash, byte[] bytes)
		{
			// The first packet response from the server is going to be a string
			string tmp = Encoding.UTF8.GetString(bytes);
			string[] headers = tmp.Replace("\r", "").Split('\n');

			// Improper header, so a disconnect is required
			if (headers.Length < 4)
				return false;

			// Validate that the header sent by the server correctly lines
			// up with what the expected response
			if (headers[0] == "HTTP/1.1 101 Switching Protocols" &&
				headers[1] == "Connection: Upgrade" &&
				headers[2] == "Upgrade: websocket" &&
				headers[3].StartsWith("Sec-WebSocket-Accept: "))
			{
				string hash = headers[3].Substring(headers[3].IndexOf(' ') + 1);
				if (hash == HeaderHashKeyCheck(headerHash))
					return true;
			}

			return false;
		}

        /// <summary>
        /// Check to see if a header is the correct websocket header, if so
        /// then generate a response to send back, otherwise return null
        /// </summary>
        /// <param name="headers">The sent headers that need to verified</param>
        /// <returns>The response to be sent back, or null if failed validation</returns>
        /// <summary>
        ///检查头部是否是正确的websocket头部，如果是的话
        ///然后生成一个响应来发回，否则返回null
        /// </ summary>
        /// <param name =“headers”>需要验证的发送的头文件</ param>
        /// <returns>要发回的响应，如果验证失败，则返回null </ returns>
        public static byte[] ValidateConnectionHeader(byte[] headers)
		{
            //验证头文件总是一个字符串
            // The validation headers are always a string
            string data = Encoding.UTF8.GetString(headers);

            //确保它以GET请求开始
            // Make sure that it starts as a GET request
            if (new Regex("^GET").IsMatch(data))
			{
                //通过提供随机字符串键生成响应
                // Generate a response by hasing the provided random string key
                byte[] response = Encoding.UTF8.GetBytes("HTTP/1.1 101 Switching Protocols" + Environment.NewLine
					+ "Connection: Upgrade" + Environment.NewLine
					+ "Upgrade: websocket" + Environment.NewLine
					+ "Sec-WebSocket-Accept: " + Convert.ToBase64String((new SHA1CryptoServiceProvider()).ComputeHash(Encoding.UTF8.GetBytes(
						new Regex("Sec-WebSocket-Key: (.*)").Match(data).Groups[1].Value.Trim() + "258EAFA5-E914-47DA-95CA-C5AB0DC85B11"
					))) + Environment.NewLine + Environment.NewLine);

				return response;
			}

            //不是一个GET请求失败
            // Was not a GET request so failed
            return null;
		}

        /// <summary>
        /// Generate a random header hash key that is to be hashed by the server
        /// </summary>
        /// <returns>The hash of a random string</returns>
        /// <summary>
        ///生成一个将被服务器哈希的随机头散列键
        /// </ summary>
        /// <returns>随机字符串的散列</ returns>
        public static string HeaderHashKey()
		{
			string headerHash = "";
			Random rand = new Random();
			char[] availableChars = "1234567890qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM".ToCharArray();

			// Generate a random string
			for (int i = 0; i < 9; i++)
				headerHash += availableChars[rand.Next(0, availableChars.Length)];

			// Hash the string
			headerHash = Convert.ToBase64String((new SHA1CryptoServiceProvider()).ComputeHash(Encoding.UTF8.GetBytes(headerHash)));

			return headerHash;
		}

        /// <summary>
        /// Compare the hash with the provided Websockets code
        /// </summary>
        /// <param name="data">The hash that was generated by Websockets.HeaderHashKey</param>
        /// <returns>Hashed string that is to be compared with the server's hash response</returns>
        /// <summary>
        ///将哈希与提供的Websockets代码进行比较
        /// </ summary>
        /// <param name =“data”>由Websockets.HeaderHashKey生成的散列</ param>
        /// <returns>要与服务器的散列响应进行比较的散列字符串</ returns>
        public static string HeaderHashKeyCheck(string data)
		{
			return Convert.ToBase64String((new SHA1CryptoServiceProvider()).ComputeHash(Encoding.UTF8.GetBytes(data + "258EAFA5-E914-47DA-95CA-C5AB0DC85B11")));
		}
	}
}