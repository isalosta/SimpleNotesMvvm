using System;
using System.Collections.Generic;

namespace wpf_mvvm_post_test
{
    class dataModelList
    {
        public List<datamodel> list;

        public void CreateList()
        {
            if (list == null)
            {
                list = new List<datamodel>();
            } else
            {
                list.Clear();
            }
        }

        public void ResetList()
        {
            if (list != null)
            {
                list = null;
                GC.Collect();
                CreateList();
            }
        }
    }
}
