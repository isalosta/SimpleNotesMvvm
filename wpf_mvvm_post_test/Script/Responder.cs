using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpf_mvvm_post_test
{
    public class Responder
    {
        public string _response { get; set; }
        public Action<Type> _callback { get; set; }
    }
}
