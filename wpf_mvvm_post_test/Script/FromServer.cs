using System;
using GalaSoft.MvvmLight.Messaging;
using Newtonsoft.Json;
using System.Net;

namespace wpf_mvvm_post_test
{
    class FromServer
    {
        DbManager DM;
        public static string D_path;
        public FromServer()
        {
            Messenger.Default.Register<ServerConfig>(this, SET_CONFIG);
            Messenger.Default.Register<ServerGet>(this, GET);
            Messenger.Default.Register<CreateNote>(this, ADD);
            Messenger.Default.Register<ServerUpdate>(this, UPDATE);
            Messenger.Default.Register<DeleteNote>(this, DELETE);
            Messenger.Default.Register<ExceptWeb>(this, GET_LOCAL);
            DM = new DbManager();
        }

        private string API_HOST;
        private string API_TOKEN;
        private string DB_PATH;

        private void GET_LOCAL(ExceptWeb EW)
        {
            System.Windows.MessageBox.Show("YOU ARE OFFLINE", "ERROR");

            Messenger.Default.Send<NotificationMessage>(new NotificationMessage(EW.message));
            string[] id = new string[0];
            string[] title = new string[0];
            string[] content = new string[0];

            DM.Read((output, output_2, output_3) => { id = output; title = output_2; content = output_3; });

            CollectionDataResponse cdr = new CollectionDataResponse();
            cdr.ARR_ID = id;
            cdr.ARR_TITLE = title;
            cdr.ARR_CONTENT = content;
            Messenger.Default.Send<CollectionDataResponse>(cdr);
            Messenger.Default.Send<Busy>(new Busy() { busy = false });
        }

        [ExceptionHandler]
        private void GET(ServerGet SG)
        {
            Messenger.Default.Send<Busy>(new Busy() { busy = true });
          //  try
          //  {
            Messenger.Default.Send<NotificationMessage>(new NotificationMessage(API_HOST + " " + API_TOKEN));
            WebRequest request = WebRequest.Create(API_HOST);
            request.Method = WebRequestMethods.Http.Get;
            var response = request.GetResponse();

            using(System.IO.Stream stream = response.GetResponseStream())
            {
                using(System.IO.StreamReader SR = new System.IO.StreamReader(stream))
                {
                    string res = SR.ReadToEnd();
                    Console.WriteLine(res);
                    ServerAdd[] result = JsonConvert.DeserializeObject<ServerAdd[]>(res);
                    int length = result.Length;
                    string[] id = new string[length];
                    string[] title = new string[length];
                    string[] content = new string[length];

                    for(int i = 0; i<length; i++)
                    {
                        id[i] = result[i]._id;
                        title[i] = result[i].title;
                        content[i] = result[i].content;
                        Console.WriteLine(result[i]._id);
                    }

                    DM.SyncAll(id, title, content);

                    SG.callback(id, title, content);

                    System.Threading.Thread.Sleep(500);
                    Messenger.Default.Send<Busy>(new Busy() { busy = false });
                }
            }
          //  }
           /* catch (Exception e)
            {
                Messenger.Default.Send<NotificationMessage>(new NotificationMessage(e.GetBaseException()+" "+ e.Message));
                System.Windows.MessageBox.Show("YOU ARE OFFLINE", "ERROR");
            }*/
        }

        [ExceptionHandler]
        private void ADD(CreateNote CN)
        {
            string output = JsonConvert.SerializeObject(CN);

            Console.WriteLine(API_HOST);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(API_HOST);
            request.Method = WebRequestMethods.Http.Post;
            request.ContentType = "application/json";

            using (System.IO.StreamWriter SW = new System.IO.StreamWriter(request.GetRequestStream()))
            {
                SW.Write(output);
                SW.Flush();
                SW.Close();

                Messenger.Default.Send<NotificationMessage>(new NotificationMessage(output));

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (System.IO.Stream stream = response.GetResponseStream())
                {
                    using (System.IO.StreamReader SR = new System.IO.StreamReader(stream))
                    {
                        string res = SR.ReadToEnd();
                        Messenger.Default.Send<NotificationMessage>(new NotificationMessage(res));
                        ServerAdd result = JsonConvert.DeserializeObject<ServerAdd>(res);

                        DataResponse DR = new DataResponse();
                        DR.id = result._id;
                        DR.title = result.title;
                        DR.content = result.content;
                        Messenger.Default.Send<DataResponse>(DR);

                        DM.Insert(result._id, result.title, result.content);
                    }
                }
            }
        }
        [ExceptionHandler]
        private void UPDATE(ServerUpdate SU)
        {
            Messenger.Default.Send<Busy>(new Busy() { busy = true });
            CardSchema CS = new CardSchema();
            CS._id = SU.id;
            CS.title = SU.title;
            CS.content = SU.content;

            string output = JsonConvert.SerializeObject(CS);

            Console.WriteLine(API_HOST +"/"+ SU.id);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(API_HOST+"/"+SU.id);
            request.Method = WebRequestMethods.Http.Put;
            request.ContentType = "application/json";

            using (System.IO.StreamWriter SW = new System.IO.StreamWriter(request.GetRequestStream()))
            {
                SW.Write(output);
                SW.Flush();
                SW.Close();

                Messenger.Default.Send<NotificationMessage>(new NotificationMessage(output));

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (System.IO.Stream stream = response.GetResponseStream())
                {
                    using (System.IO.StreamReader SR = new System.IO.StreamReader(stream))
                    {
                        string res = SR.ReadToEnd();
                        Messenger.Default.Send<NotificationMessage>(new NotificationMessage(res));
                        Messenger.Default.Send<Command>(new Command() { _cmd = Constants.CMD_DATA_LOAD });
                    }
                }
            }
        }

        [ExceptionHandler]
        private void DELETE(DeleteNote DN)
        {
            Console.WriteLine(API_HOST);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(API_HOST+"/"+DN.id);
            request.Method = "DELETE";
            request.ContentType = "application/json";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (System.IO.Stream stream = response.GetResponseStream())
            {
                using (System.IO.StreamReader SR = new System.IO.StreamReader(stream))
                {
                    string res = SR.ReadToEnd();
                    DM.Delete(DN.id);

                    Messenger.Default.Send<NotificationMessage>(new NotificationMessage(res));
                    Messenger.Default.Send<Command>(new Command() { _cmd = Constants.CMD_DATA_LOAD });
                }
            }
        }

        private void SET_CONFIG(ServerConfig SC)
        {
            Messenger.Default.Send<NotificationMessage>(new NotificationMessage("SET " + SC.HOST + " " + SC.TOKEN));
            API_HOST = SC.HOST;
            API_TOKEN = SC.TOKEN;
            DB_PATH = SC.DB_PATH;
            DM.SetDBpath(DB_PATH);
            D_path = DB_PATH;
        }
    }

    public class ServerConfig
    {
        public string HOST;
        public string TOKEN;
        public string DB_PATH;
    }

    public class ServerGet
    {
        public Action<string[], string[], string[]> callback;
    }

    public class ServerAdd
    {
        public string _id;
        public string title;
        public string content;
    }

    public class CardSchema
    {
        public string _id;
        public string title;
        public string content;
    }

    public class ServerUpdate
    {
        public string id;
        public string title;
        public string content;
    }

    public class CreateNote
    {
        public string title;
        public string content;
    }

    public class DeleteNote
    {
        public string id;
    }

    public class ExceptWeb
    {
        public string message;
    }
}
