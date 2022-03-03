using System;

namespace TLY2Sub
{
	public class VMess
	{
		public string Remark;

		public string Group = "None";

		public string Type;

		public double Rate = 1.0;

		public string Hostname;

		public int Port;

		public int Delay = -1;

		public string UserID { get; set; }

		public int AlterID { get; set; }

		public string EncryptMethod { get; set; } = VMessGlobal.EncryptMethods[0];

		public string TransferProtocol { get; set; } = VMessGlobal.TransferProtocols[0];

		public string FakeType { get; set; } = VMessGlobal.FakeTypes[0];

		public string QUIC { get; set; } = VMessGlobal.QUIC[0];

		public string Host { get; set; }

		public string Path { get; set; }

		public string QUICSecure { get; set; } = VMessGlobal.QUIC[0];

		public string QUICSecret { get; set; } = string.Empty;

		public bool TLSSecure { get; set; }

		public bool UseMux { get; set; } = true;

		public string TLSSecureType { get; set; } = VMessGlobal.TLSSecure[0];
	}
}
