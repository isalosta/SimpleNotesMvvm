using System;
using System.Collections.Generic;
using GalaSoft.MvvmLight.Messaging;

namespace wpf_mvvm_post_test
{
    [ExceptionHandler]
    class DataMain
    {
        private string[] arr_id;
        private string[] arr_title;
        private string[] arr_content;

        private void ApplyPacket(string[] ids, string[] tit, string[] contents)
        {
            arr_id = ids;
            arr_title = tit;
            arr_content = contents;

            CollectionDataResponse CDR = new CollectionDataResponse();
            CDR.ARR_ID = arr_id;
            CDR.ARR_TITLE = arr_title;
            CDR.ARR_CONTENT = arr_content;

            Messenger.Default.Send<CollectionDataResponse>(CDR);
        }

        private void LoadData()
        {
            ServerGet s = new ServerGet();
            s.callback = ApplyPacket;

            Messenger.Default.Send<ServerGet>(s);
        }

        public void Receiver(Responder res)
        {
            switch (res._response)
            {
                case Constants.REC_DATA_LOAD:
                    LoadData();
                    break;
            }
        }
    }
}
