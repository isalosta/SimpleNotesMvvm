using System;

namespace wpf_mvvm_post_test
{
    public class XmlLoadFormat
    {
        public byte _mode { get; set; }
        public string _path { get; set; }
        public string _node { get; set; }
        public string _singleKey { get; set; }
        public string[] _multiKey { get; set; }
        public Action<string> _callback { get; set; }
        public string[] _storeVal { get; set; }
    }
}
