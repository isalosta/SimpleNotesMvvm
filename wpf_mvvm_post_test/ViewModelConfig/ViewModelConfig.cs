using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using PostSharp.Patterns.Model;

namespace wpf_mvvm_post_test
{
    [NotifyPropertyChanged]
    public class ViewModelConfig : ViewModelBase
    {
        public ViewModelConfig()
        {
            Messenger.Default.Send<NotificationMessage>(new NotificationMessage("CREATE CONFIG"));
            Messenger.Default.Register<DataConfigResponse>(this, SET_VIEWCONFIG);
            Messenger.Default.Register<GetDataConfig>(this, GET_DATA);
            Messenger.Default.Send<Command>(new Command() { _cmd = Constants.CMD_SND_CONFIG });

        }

        private string api;
        private string api_key;
        private string interval;
        private string db_name;

        public string API
        {
            get
            {
                return api;
            } set
            {
                api = value;
            }
        }

        public string API_KEY
        {
            get
            {
                return api_key;
            }
            set
            {
                api_key = value;
            }
        }

        public string INTERVAL
        {
            get
            {
                return interval;
            } set
            {
                interval = value;
            }
        }

        public string DB
        {
            get
            {
                return db_name;
            }
            set
            {
                db_name = value;
            }
        }

        public void SET_VIEWCONFIG(DataConfigResponse DCR)
        {
            Messenger.Default.Send<NotificationMessage>(new NotificationMessage("CONFIGVIEWMODEL RECIEVE DCR"));
            API = DCR._configArray[0];
            API_KEY = DCR._configArray[1];
            INTERVAL = DCR._configArray[2];
            DB = DCR._configArray[3];
        }


        public void GET_DATA(GetDataConfig GDC)
        {
            GDC.Callback(API, API_KEY, INTERVAL, DB);
        }

    }

    public class DataConfigResponse
    {
        public string[] _configArray;
    }

    public class GetDataConfig
    {
        public Action<string, string, string, string> Callback;
    }
}
