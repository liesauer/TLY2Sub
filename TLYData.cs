using Newtonsoft.Json;

namespace TLY2Sub
{
    class Node
    {
        public string id;
        public string alterId;
        public string cipher;
        public string node_name;
        public string node_method;
        public string node_server;
        public string node_status;
        public string pass;
        public int port;
        public string recommend;
        public string tls;
        [JsonProperty(PropertyName = "tls-hostname")]
        public string tlsHostname;
        [JsonProperty(PropertyName = "ws-path")]
        public string wsPath;
    }

    class TLYData
    {
        public string ret;
        public int time;
        public float transfer;
        public int node_number;
        public Node[] node;
    }
}
