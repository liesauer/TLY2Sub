using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace TLY2Sub
{
	public class AESHelper
	{
		public static string AESDecrypt(string content, string key, string iv)
		{
			string result = null;

			try
			{
				byte[] contentBytes = Convert.FromBase64String(content);
				byte[] resultBytes = new RijndaelManaged
				{
					Key = Encoding.UTF8.GetBytes(key),
					IV = Encoding.UTF8.GetBytes(iv),
					Mode = CipherMode.CBC,
					Padding = PaddingMode.Zeros
				}.CreateDecryptor().TransformFinalBlock(contentBytes, 0, contentBytes.Length);

				result = Encoding.Default.GetString(resultBytes);
			}
			catch { }

			return result;
		}

		public static string AesEncrypt(string content, string key, string iv)
		{
			string result = null;

			using (RijndaelManaged rijndaelManaged = new RijndaelManaged())
			{
				rijndaelManaged.Mode = CipherMode.CBC;
				rijndaelManaged.Padding = PaddingMode.Zeros;
				rijndaelManaged.KeySize = 128;
				rijndaelManaged.BlockSize = 128;
				rijndaelManaged.Key = Encoding.UTF8.GetBytes(key);
				rijndaelManaged.IV = Encoding.UTF8.GetBytes(iv);

				byte[] contentBytes = Encoding.UTF8.GetBytes(content);

				using (ICryptoTransform cryptoTransform = rijndaelManaged.CreateEncryptor())
				{
					using (MemoryStream memoryStream = new MemoryStream())
					{
						using (CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write))
						{
							cryptoStream.Write(contentBytes, 0, contentBytes.Length);
							cryptoStream.FlushFinalBlock();
							byte[] resultBytes = memoryStream.ToArray();
							StringBuilder stringBuilder = new StringBuilder();
							for (int i = 0; i < resultBytes.Length; i++)
							{
								byte b = resultBytes[i];
								Convert.ToString(resultBytes[i], 16).PadLeft(2, '0');
								stringBuilder.Append(Convert.ToString(resultBytes[i], 16).PadLeft(2, '0'));
							}
							result = HexToBase64(stringBuilder.ToString());
						}
					}
				}
			}

			return result;
		}

		public static string HexToBase64(string content)
		{
			string result = null;

			try
			{
				byte[] contentBytes = new byte[content.Length / 2];
				for (int i = 0; i < contentBytes.Length; i++)
				{
					contentBytes[i] = Convert.ToByte(content.Substring(i * 2, 2), 16);
				}
				result = Convert.ToBase64String(contentBytes);
			}
			catch { }

			return result;
		}

		public static string Encrypt(string content, string key)
		{
			byte[] contentBytes = Encoding.UTF8.GetBytes(content);
			byte[] resultBytes = new RijndaelManaged
			{
				Key = Encoding.UTF8.GetBytes(key),
				IV = Encoding.UTF8.GetBytes(key),
				Mode = CipherMode.CBC,
				Padding = PaddingMode.PKCS7
			}.CreateEncryptor().TransformFinalBlock(contentBytes, 0, contentBytes.Length);

			return Convert.ToBase64String(resultBytes, 0, resultBytes.Length);
		}

		public static string Decrypt(string content, string key)
		{
			byte[] contentBytes = Convert.FromBase64String(content);
			byte[] resultBytes = new RijndaelManaged
			{
				Key = Encoding.UTF8.GetBytes(key),
				IV = Encoding.UTF8.GetBytes(key),
				Mode = CipherMode.CBC,
				Padding = PaddingMode.PKCS7
			}.CreateDecryptor().TransformFinalBlock(contentBytes, 0, contentBytes.Length);

			return Encoding.UTF8.GetString(resultBytes);
		}
	}
}
