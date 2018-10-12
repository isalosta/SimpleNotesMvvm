using System;
using System.Collections.Generic;
using GalaSoft.MvvmLight.Messaging;
using PostSharp.Patterns.Model;

namespace wpf_mvvm_post_test
{
    class DataConfig
    {
        private string api;
        private string api_key;
        private string interval;
        private string db_name;

        private string XML_PATH = AppDomain.CurrentDomain.BaseDirectory + AppDomain.CurrentDomain.FriendlyName + ".config";

        public string API { get; set; }
        public string API_KEY { get; set; }
        public int INTERVAL { get; set; }
        public string DB_NAME { get; set; }

        private void Load()
        {
            XmlLoadFormat XLF = new XmlLoadFormat();
            int counter = -1;

            XLF._mode = 0;
            XLF._path = XML_PATH;
            XLF._node = Constants.XML_NODE_CHD;
            XLF._singleKey = Constants.XML_CONF_ATTR_API;
            XLF._callback = (callback) => api = callback;

            Messenger.Default.Send<XmlLoadFormat>(XLF);
            while (String.IsNullOrEmpty(api))
            {
                counter++;
                System.Threading.Thread.Sleep(1000);
                Console.WriteLine(counter);
                if(counter > 0)
                {
                    api = "null";
                    break;
                }
            }
            counter = -1;

            XLF._singleKey = Constants.XML_CONF_ATTR_TKN;
            XLF._callback = (callback) => api_key = callback;

            Messenger.Default.Send<XmlLoadFormat>(XLF);
            while (String.IsNullOrEmpty(api_key))
            {
                counter++;
                System.Threading.Thread.Sleep(1000);
                Console.WriteLine(counter);
                if (counter > 0)
                {
                    api_key = "null";
                    break;
                }
            }
            counter = -1;

            XLF._singleKey = Constants.XML_CONF_ATTR_INTR;
            XLF._callback = (callback) => interval = callback;

            Messenger.Default.Send<XmlLoadFormat>(XLF);
            while (String.IsNullOrEmpty(interval))
            {
                counter++;
                System.Threading.Thread.Sleep(1000);
                Console.WriteLine(counter);
                if (counter > 0)
                {
                    interval = "0";
                    break;
                }
            }
            counter = -1;
            XLF._singleKey = Constants.XML_CONF_ATTR_DB;
            XLF._callback = (callback) => db_name = callback;

            Messenger.Default.Send<XmlLoadFormat>(XLF);
            while (String.IsNullOrEmpty(db_name))
            {
                counter++;
                System.Threading.Thread.Sleep(1000);
                Console.WriteLine(counter);
                if (counter > 0)
                {
                    db_name = "null";
                    break;
                }
            }

            XLF = null;
            API = api;
            API_KEY = api_key;
            INTERVAL = int.Parse(interval);
            DB_NAME = db_name;

            ServerConfig SC = new ServerConfig();
            SC.HOST = api;
            SC.TOKEN = api_key;
            SC.DB_PATH = db_name;
            Messenger.Default.Send<ServerConfig>(SC);
        }

        private string[] ConfigsDataPacket()
        {
            string[] Arr = new string[4];
            Arr[0] = API;
            Arr[1] = API_KEY;
            Arr[2] = interval;
            Arr[3] = DB_NAME;
            return Arr;
        }

        private void SendPacket()
        {
            DataConfigResponse DCR = new DataConfigResponse();
            DCR._configArray = ConfigsDataPacket();

            Messenger.Default.Send<DataConfigResponse>(DCR);

            DCR = null;
        }

        private void PacketFormat(string A, string AK, string I, string D)
        {
            api = A;
            api_key = AK;
            interval = I;
            db_name = D;
        }

        private void GetPacket()
        {
            Messenger.Default.Send<Busy>(new Busy() { busy = true });
            GetDataConfig GDC = new GetDataConfig();
            GDC.Callback = PacketFormat;
            Messenger.Default.Send<GetDataConfig>(GDC);
            GDC = null;

            System.Threading.Thread.Sleep(500);
            SaveAll();
        }

        private void SaveAll()
        {
            int length = 4;
            string[] strArr = new string[length];
            string[] strVal = new string[length];

            strArr[0] = Constants.XML_CONF_ATTR_API;
            strArr[1] = Constants.XML_CONF_ATTR_TKN;
            strArr[2] = Constants.XML_CONF_ATTR_INTR;
            strArr[3] = Constants.XML_CONF_ATTR_DB;

            strVal[0] = api;
            strVal[1] = api_key;
            strVal[2] = interval;
            strVal[3] = db_name;

            API = api;
            API_KEY = api_key;
            INTERVAL = int.Parse(interval);
            DB_NAME = db_name;

            XmlLoadFormat XLF = new XmlLoadFormat();
            XLF._mode = 1;
            XLF._path = XML_PATH;
            XLF._node = Constants.XML_NODE_CHD;
            XLF._multiKey = strArr;
            XLF._storeVal = strVal;

            Messenger.Default.Send<XmlLoadFormat>(XLF);

            strArr = null;
            strVal = null;
            XLF = null;

            ServerConfig SC = new ServerConfig();
            SC.HOST = api;
            SC.TOKEN = api_key;
            SC.DB_PATH = db_name;
            Messenger.Default.Send<ServerConfig>(SC);
            Messenger.Default.Send<Command>(new Command() { _cmd = Constants.CMD_CLOSE_CONFIG });
            Messenger.Default.Send<Busy>(new Busy() { busy = false });
        }

        public void Reciever(Responder response)
        {
            Messenger.Default.Send<NotificationMessage>(new NotificationMessage("DATACONFIG RECIEVE CMD " + response._response));
            switch (response._response)
            {
                case Constants.REC_XML_LOAD:
                    Load();
                    break;

                case Constants.SENT_CONF:
                    SendPacket();
                    break;

                case Constants.GET_CONF:
                    GetPacket();
                    break;
            }
        }
    }
}
