using System;

namespace wpf_mvvm_post_test
{
    class Command
    {
        public string _cmd { get; set; }
        public Action<Type> _callback { get; set; }
    }
}
