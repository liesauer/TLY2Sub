using Newtonsoft.Json;

using System;
using System.Text;

namespace TLY2Sub
{
    public class VMessUtil
	{
		public static string GetShareLink(VMess vmess)
		{
			string text = JsonConvert.SerializeObject(new
			{
				v = "2",
				ps = vmess.Remark,
				add = vmess.Hostname,
				port = vmess.Port,
				id = vmess.UserID,
				aid = vmess.AlterID,
				net = vmess.TransferProtocol,
				type = vmess.FakeType,
				host = vmess.Host,
				path = vmess.Path,
				tls = (vmess.TLSSecure ? "tls" : "")
			});
			return "vmess://" + URLSafeBase64Encode(text);
		}

		public static string URLSafeBase64Decode(string text)
		{
			return Encoding.UTF8.GetString(Convert.FromBase64String(text.Replace("-", "+").Replace("_", "/").PadRight(text.Length + (4 - text.Length % 4) % 4, '=')));
		}

		public static string URLSafeBase64Encode(string text)
		{
			return Convert.ToBase64String(Encoding.UTF8.GetBytes(text)).Replace("+", "-").Replace("/", "_")
				.Replace("=", "");
		}
	}
}
