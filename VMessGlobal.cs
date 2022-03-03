using System;
using System.Collections.Generic;

namespace TLY2Sub
{
	public class VMessGlobal
	{
		public static readonly List<string> EncryptMethods = new List<string> { "auto", "none", "aes-128-gcm", "chacha20-poly1305" };

		public static readonly List<string> QUIC = new List<string> { "none", "aes-128-gcm", "chacha20-poly1305" };

		public static readonly List<string> TransferProtocols = new List<string> { "tcp", "kcp", "ws", "h2", "quic" };

		public static readonly List<string> FakeTypes = new List<string> { "none", "http", "srtp", "utp", "wechat-video", "dtls", "wireguard" };

		public static readonly List<string> TLSSecure = new List<string> { "tls", "" };
	}
}
