using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;

namespace wpf_mvvm_post_test
{
    class Core
    {
        public static Core instance;

        private XmlProcessor xmlProc;
        private System.Windows.Window configWindows;

        private dataModelList dataModel;
        private DataConfig dataConfig;

        private Responder respond;
        private Command command;

        private DataMain dataMain;
        private FromServer fromServer;

        public bool isBusy;

        public Core()
        {
            if (instance == null)
            {
                Messenger.Default.Register<NotificationMessage>(this, NotifyMe);
                Messenger.Default.Register<XmlProcessor>(this, SetXmlProc);
                Messenger.Default.Register<System.Windows.Window>(this, SetSecWindows);
                Messenger.Default.Register<dataModelList>(this, SetDataModelList);
                Messenger.Default.Register<FromServer>(this, SetFromServer);
                Messenger.Default.Register<DataMain>(this, SetDataMain);
                Messenger.Default.Register<DataConfig>(this, SetDataConfig);
                Messenger.Default.Register<Command>(this, CommandCenter);
                Messenger.Default.Register<Busy>(this, SET_BUSY);
                Messenger.Default.Register<Show_Error>(this, SHOW_ERROR);
                respond = new Responder();
                command = new Command();
                instance = this;
            }
        }

        private void NotifyMe(NotificationMessage notif)
        {
            string s = notif.Notification;
            string log = "<<LOGGER>>    ";
            Console.WriteLine(log + s);
        }

        private void SetXmlProc(XmlProcessor xml)
        {
            Messenger.Default.Send<NotificationMessage>(new NotificationMessage("INIT XMLPROCESSOR"));
            xmlProc = xml;
            command._cmd = Constants.REC_XML_XMLPROC;
            CommandCenter(command);
        }

        private void SetSecWindows(System.Windows.Window win)
        {
            Messenger.Default.Send<NotificationMessage>(new NotificationMessage("INIT CONFIG WINDOW"));
            configWindows = win;
        }

        private void SetDataModelList(dataModelList modelList)
        {
            Messenger.Default.Send<NotificationMessage>(new NotificationMessage("INIT DATALIST"));
            dataModel = modelList;
        }

        private void SetDataConfig(DataConfig config)
        {
            Messenger.Default.Send<NotificationMessage>(new NotificationMessage("INIT DATACONFIG"));
            dataConfig = config;
        }

        private void SetDataMain(DataMain DM)
        {
            dataMain = DM;
        }

        private void SetFromServer(FromServer FS)
        {
            fromServer = FS;
        }

        private void Timer()
        {
            int time = dataConfig.INTERVAL * 60;
            SetTimer st = new SetTimer();

            while (time > 0 && !isBusy)
            {
                time--;
                TimeSpan minute = TimeSpan.FromSeconds(time);
                string min = minute.ToString();
                st.t = min;
                Messenger.Default.Send<SetTimer>(st);
                System.Threading.Thread.Sleep(1000);
            }

            while (isBusy)
            {
                System.Threading.Thread.Sleep(1000);
            }

            UpdateNote();
            System.Threading.Thread.Sleep(500);
            Timer();
        }

        private void CreateNote()
        {
            CreateNote CN = new CreateNote();
            CN.title = "Untitled";
            CN.content = "[EMPTY]";

            Messenger.Default.Send<CreateNote>(CN);
            CN = null;
        }

        private void UpdateNote()
        {
            string id = "";
            string title = "";
            string content = "";

            GetDataMain GDM = new GetDataMain();
            GDM.Callback = (output, outputP, outputC) => { id = output; title = outputP; content = outputC; };

            Messenger.Default.Send<GetDataMain>(GDM);

            while(string.IsNullOrEmpty(id) && string.IsNullOrEmpty(title) && string.IsNullOrEmpty(content))
            {
                System.Threading.Thread.Sleep(200);
            }

            ServerUpdate SA = new ServerUpdate();
            SA.id = id;
            SA.title = title;
            SA.content = content;
            Messenger.Default.Send<ServerUpdate>(SA);

            GDM = null;
            SA = null;
        }

        public void DeleteNote()
        {
            string id = "";
            string title = "";
            string content = "";

            GetDataMain GDM = new GetDataMain();
            GDM.Callback = (output, outputP, outputC) => { id = output; title = outputP; content = outputC; };

            Messenger.Default.Send<GetDataMain>(GDM);

            while (string.IsNullOrEmpty(id) && string.IsNullOrEmpty(title) && string.IsNullOrEmpty(content))
            {
                System.Threading.Thread.Sleep(200);
            }

            DeleteNote DN = new DeleteNote();
            DN.id = id;

            Messenger.Default.Send<DeleteNote>(DN);
            GDM = null;
            DN = null;
        }

        public void SET_BUSY(Busy busy)
        {
            isBusy = busy.busy;
        }

        public void SHOW_ERROR(Show_Error SE)
        {
            System.Windows.MessageBox.Show(SE.message, SE.exception);
        }

        private void CommandCenter(Command cmd)
        {
            Messenger.Default.Send<NotificationMessage>(new NotificationMessage("RUN COMMAND " + cmd._cmd));
            switch (cmd._cmd)
            {
                case Constants.REC_XML_XMLPROC:
                    respond._response = cmd._cmd;
                    Messenger.Default.Register<Responder>(xmlProc, xmlProc.Reciever);
                    Messenger.Default.Send<Responder>(respond);
                    Messenger.Default.Unregister<Responder>(xmlProc, xmlProc.Reciever);
                    break;

                case Constants.CMD_XML_LOAD:
                    respond._response = Constants.REC_XML_LOAD;
                    Messenger.Default.Register<Responder>(dataConfig, dataConfig.Reciever);
                    Messenger.Default.Send<Responder>(respond);
                    Messenger.Default.Unregister<Responder>(dataConfig, dataConfig.Reciever);
                    break;

                case Constants.CMD_SND_CONFIG:
                    respond._response = Constants.SENT_CONF;
                    Messenger.Default.Register<Responder>(dataConfig, dataConfig.Reciever);
                    Messenger.Default.Send<Responder>(respond);
                    Messenger.Default.Unregister<Responder>(dataConfig, dataConfig.Reciever);
                    break;

                case Constants.CMD_GET_CONFIG:
                    respond._response = Constants.GET_CONF;
                    Messenger.Default.Register<Responder>(dataConfig, dataConfig.Reciever);
                    Messenger.Default.Send<Responder>(respond);
                    Messenger.Default.Unregister<Responder>(dataConfig, dataConfig.Reciever);
                    break;

                case Constants.CMD_DATA_LOAD:
                    respond._response = Constants.REC_DATA_LOAD;
                    Messenger.Default.Register<Responder>(dataMain, dataMain.Receiver);
                    Messenger.Default.Send<Responder>(respond);
                    Messenger.Default.Unregister<Responder>(dataMain, dataMain.Receiver);
                    break;

                case Constants.CMD_RUN_CREATE:
                    CreateNote();
                    break;

                case Constants.CMD_RUN_UPDATE:
                    UpdateNote();
                    break;

                case Constants.CMD_RUN_DELETE:
                    DeleteNote();
                    break;

                case Constants.TIMER_INIT:
                    Task task = new Task(Timer);
                    task.Start();
                    break;

                case Constants.CMD_OPEN_CONFIG:
                    Messenger.Default.Send<System.Windows.Window>(new Config());
                    configWindows.Show();
                    break;

                case Constants.CMD_CLOSE_CONFIG:
                    configWindows.Close();
                    break;

                case Constants.CMD_SHOW_ERROR:
                    break;
            }
        }
    }

    public class Busy
    {
        public bool busy;
    }

    public class Show_Error
    {
        public string exception;
        public string message;
    }
}


