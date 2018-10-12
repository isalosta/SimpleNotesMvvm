using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using PostSharp.Patterns.Model;
using System;

namespace wpf_mvvm_post_test
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    [NotifyPropertyChanged]
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        
        public MainViewModel()
        {
            Messenger.Default.Register<DataResponse>(this, UPDATE_COLLECTION);
            Messenger.Default.Register<GetDataMain>(this, GET_DISPLAY);
            Messenger.Default.Register<CollectionDataResponse>(this, SET_COLLECTION);
            Messenger.Default.Register<SelectData>(this, SELECT);
            Messenger.Default.Register<SetTimer>(this, SET_TIME);
            Messenger.Default.Register<TitleChange>(this, TITLE_CHANGED);
            Messenger.Default.Register<ContentChange>(this, CONTENT_CHANGED);
            Messenger.Default.Send<NotificationMessage>(new NotificationMessage("CREATE MAINVIEW"));
        }

        private string id;
        private string title;
        private string content;
        private int idx;

        private string timer;

        private string[] ARR_ID;
        private string[] ARR_TITLE; 
        private string[] ARR_CONTENT;
        private string[] titles;

        public string ID {
            get
            {
                return id;
            } set
            {
                id = value;
            }
        }

        public string TITLE
        {
            get
            {
                return title;
            } set
            {
                title = value;
            }
        }

        public string CONTENT
        {
            get
            {
                return content;
            } set
            {
                content = value;
            }
        }

        public string[] ITEM_ARR
        {
            get
            {
                return titles;
            }
            set
            {
                titles = value;
            }
        }

        public System.Collections.Generic.List<string> ITEM
        {
            get
            {
                if (ARR_TITLE.Length <= 0 || ARR_TITLE == null)
                {

                    System.Collections.Generic.List<string> def = new System.Collections.Generic.List<string>();
                    def.Add("NEW ID");
                    return def;
                }
                else
                {
                    return new System.Collections.Generic.List<string>(ARR_TITLE);
                }
            } set
            {
                ARR_TITLE = value.ToArray();
            }
        }

        public int SELECTED
        {
            get
            {
                return idx;
            }
            set
            {
                idx = value;
            }
        }

        public string TIMER {
            get
            {
                return timer;
            }
            set
            {
                timer = value;
            }
        }

        private void SELECT(SelectData SD)
        {
            int i = Array.IndexOf(ARR_TITLE, SD.content);
            ID = ARR_ID[i];
            TITLE = ARR_TITLE[i];
            CONTENT = ARR_CONTENT[i];
        }

        private void GET_DISPLAY(GetDataMain GDM)
        {
            GDM.Callback(ID, TITLE, CONTENT);
        }

        private void SET_COLLECTION(CollectionDataResponse CDR)
        {
            RESET_COLLECTION();
            int length = CDR.ARR_CONTENT.Length;

            Array.Resize(ref ARR_CONTENT, length);
            Array.Resize(ref ARR_ID, length);
            Array.Resize(ref ARR_TITLE, length);

            for (int i = 0; i < length; i++)
            {
                ARR_CONTENT[i] = CDR.ARR_CONTENT[i];
                ARR_ID[i] = CDR.ARR_ID[i];
                ARR_TITLE[i] = CDR.ARR_TITLE[i];
            }

            ITEM_ARR = ARR_TITLE;
            Messenger.Default.Send<SelectData>(new SelectData() { content = ARR_TITLE[0]});
        }

        private void UPDATE_COLLECTION(DataResponse DR)
        {
            Array.Resize(ref ARR_ID, ARR_ID.Length + 1);
            ARR_ID[ARR_ID.Length - 1] = DR.id;

            Array.Resize(ref ARR_TITLE, ARR_TITLE.Length + 1);
            ARR_TITLE[ARR_TITLE.Length - 1] = DR.title;

            Array.Resize(ref ARR_CONTENT, ARR_CONTENT.Length + 1);
            ARR_CONTENT[ARR_CONTENT.Length - 1] = DR.content;

            ITEM_ARR = ARR_TITLE;
        }

        private void TITLE_CHANGED(TitleChange tc)
        {
            TITLE = tc.title;
        }

        private void CONTENT_CHANGED(ContentChange cc)
        {
            CONTENT = cc.content;
        }

        private void RESET_COLLECTION()
        {
            idx = 0;
            ID = null;
            TITLE = null;
            CONTENT = null;

            Array.Resize(ref ARR_ID, 0);
            Array.Resize(ref ARR_TITLE, 0);
            Array.Resize(ref ARR_CONTENT, 0);
        }

        private void SET_TIME(SetTimer ST)
        {
            TIMER = ST.t;
        }
    }

    public class SelectData
    {
        public string content;
    }

    public class GetDataMain
    {
        public Action<string, string, string> Callback;
    }

    public class CollectionDataResponse
    {
        public string[] ARR_ID;
        public string[] ARR_TITLE;
        public string[] ARR_CONTENT;
    }

    public class DataResponse
    {
        public string id;
        public string title;
        public string content;
    }

    public class SetTimer {
        public string t;
    }

    public class TitleChange
    {
        public string title;
    }

    public class ContentChange
    {
        public string content;
    }
}