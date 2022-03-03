using CommandLine;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using WatsonWebserver;

namespace TLY2Sub
{
    class Program
    {
        private static readonly string AesIV = "9987654321fedcsu";
	    private static readonly string AesKey = "tlynet923456789k";

		public class Options
		{
			[Option("host", Required = true)]
			public string Host { get; set; }

			[Option("port", Required = true)]
			public int Port { get; set; }

			[Option("email", Required = true)]
			public string Email { get; set; }

			[Option("passwd", Required = true)]
			public string Passwd { get; set; }
		}

		private static Options options;

		static void Main(string[] args)
        {
			CommandLine.Parser.Default.ParseArguments<Options>(args).WithParsed(opts => options = opts);

			Server server = new Server(options.Host, options.Port, false);

			server.Start();

            while (true)
            {
				Thread.Sleep(50);
            }
		}

		public static string FetchData(string email, string passwd)
		{
			string url = "https://win.tly08.com/api/E-win2.php";

            var postData = new Dictionary<string, string>
            {
                {
					"a",
					AESHelper.AesEncrypt(email, AesKey, AesIV)
				},
                {
					"b",
					passwd
				}
            };

            string result = "";

			HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
			httpWebRequest.Method = "POST";
			httpWebRequest.ContentType = "application/x-www-form-urlencoded";

			StringBuilder stringBuilder = new StringBuilder();

			int num = 0;
			foreach (KeyValuePair<string, string> keyValuePair in postData)
			{
				if (num > 0)
				{
					stringBuilder.Append("&");
				}
				stringBuilder.AppendFormat("{0}={1}", WebUtility.UrlEncode(keyValuePair.Key), WebUtility.UrlEncode(keyValuePair.Value));
				num++;
			}
			byte[] contentBytes = Encoding.UTF8.GetBytes(stringBuilder.ToString());

			httpWebRequest.ContentLength = (long)contentBytes.Length;

			try
			{
				using (Stream requestStream = httpWebRequest.GetRequestStream())
				{
					requestStream.Write(contentBytes, 0, contentBytes.Length);
					requestStream.Close();
				}
			}
			catch
			{
				return result;
			}

			using (StreamReader streamReader = new StreamReader(((HttpWebResponse)httpWebRequest.GetResponse()).GetResponseStream(), Encoding.UTF8))
			{
				result = streamReader.ReadToEnd();
			}

			return result;
		}

		public static string GenerateSubscribe(string email, string passwd)
		{
			string data = AESHelper.AESDecrypt(FetchData(email, passwd), AesKey, AesIV);

			var json = JsonConvert.DeserializeObject<TLYData>(data);

			if (json == null || json.ret != "ok")
			{
				Console.WriteLine(json.ret);

				return "";
			}

			var VMessList = new List<VMess>();

            foreach (var node in json.node)
            {
				if (node.node_method != "ws") continue;

				VMessList.Add(new VMess()
				{
					Group = "tly",
					Type = "VMess",
					Remark = node.node_name,
					Hostname = node.node_server,
					Port = node.port,
					UserID = node.pass,
					AlterID = int.Parse(node.alterId),
					TransferProtocol = "ws",
					FakeType = "none",
					Host = "",
					Path = node.wsPath,
					TLSSecure = true,
					EncryptMethod = "auto"
				});
			}

			VMessList.Add(new VMess()
			{
				Group = "tly",
				Type = "VMess",
				Remark = "剩余流量：" + json.transfer.ToString() + "G",
				Hostname = "127.0.0.1",
				Port = 8080,
				UserID = "",
				AlterID = 0,
				TransferProtocol = "ws",
				FakeType = "none",
				Host = "",
				Path = "",
				TLSSecure = true,
				EncryptMethod = "auto"
			});

			return VMessUtil.URLSafeBase64Encode(string.Join("\n", VMessList.Select(vmess => {
				return VMessUtil.GetShareLink(vmess);
			})));
		}

		[StaticRoute(HttpMethod.GET, "/subscribe")]
		public static async Task GetSubscribe(HttpContext ctx)
		{
			await ctx.Response.Send(GenerateSubscribe(options.Email, options.Passwd));
		}
	}
}
