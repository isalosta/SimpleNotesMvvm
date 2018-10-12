using System;
using PostSharp.Aspects;
using GalaSoft.MvvmLight.Messaging;

namespace wpf_mvvm_post_test
{
    [Serializable]
    class ExceptionHandlerAttribute : OnExceptionAspect
    {
        public override void OnException(MethodExecutionArgs args)
        {
            if (args.Exception.GetType().Equals(typeof(System.Net.WebException)))
            {
                args.FlowBehavior = FlowBehavior.ThrowException;
                args.Exception = new ExceptionWeb();

            } else if (args.Exception.GetType().Equals(typeof(System.Reflection.TargetInvocationException)))
            {
                Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>> INVOCATION " + args.Exception.Message);
                args.FlowBehavior = FlowBehavior.Return;

            } else if (args.Exception.GetType().Equals(typeof(System.Data.SQLite.SQLiteException)))
            {
                Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>> SQLITE " + args.Exception.Message);
                args.FlowBehavior = FlowBehavior.ThrowException;
                args.Exception = new SQLExcept();
            }
            else
            {
                Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>> " + args.Exception.Message);
                args.FlowBehavior = FlowBehavior.ThrowException;
                args.Exception = new DefaultException(args.Exception.Message, args.Exception);
            }
        }
    }

    public class ExceptionWeb : Exception
    {
        public ExceptionWeb()
        {
            Messenger.Default.Send<ExceptWeb>(new ExceptWeb() { message = ">>>> THROW" });
        }
    }

    public class SQLExcept : Exception
    {
        public SQLExcept()
        {
            System.Data.SQLite.SQLiteConnection.CreateFile(FromServer.D_path);
            System.Data.SQLite.SQLiteConnection dbConnection = new System.Data.SQLite.SQLiteConnection(string.Format("Data Source={0}", FromServer.D_path));
            string sql = "CREATE TABLE `ms_notes` (`id` TEXT, `title` TEXT, content `TEXT`)";
            dbConnection.Open();

            System.Data.SQLite.SQLiteCommand cmd = new System.Data.SQLite.SQLiteCommand(sql, dbConnection);

            
            cmd.ExecuteNonQuery();
            dbConnection.Close();
        }
    }

    public class DefaultException : Exception
    {
        public DefaultException(string message, Exception caption) : base(message, caption)
        {
            Messenger.Default.Send<Show_Error>(new Show_Error() { message = message, exception = caption.GetType().ToString() });
        }
    }




}
