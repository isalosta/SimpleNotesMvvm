using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Messaging;

namespace wpf_mvvm_post_test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TitleChange tc;
        ContentChange cc;
        public MainWindow()
        {
            InitializeComponent();
            tc = new TitleChange();
            cc = new ContentChange();
        }

        public void Click_O_Config(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Send<Command>(new Command() { _cmd = Constants.CMD_OPEN_CONFIG });
        }

        private void ComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (!Core.instance.isBusy)
            {
                Messenger.Default.Send<SelectData>(new SelectData() { content = (string)e.AddedItems[0] });
            } else
            {
                Messenger.Default.Send<NotificationMessage>(new NotificationMessage("ON BUSY"));
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Send<DataConfig>(new DataConfig());
            Messenger.Default.Send<Command>(new Command() { _cmd = Constants.CMD_XML_LOAD });
            Messenger.Default.Send<Command>(new Command() { _cmd = Constants.CMD_DATA_LOAD });
            Messenger.Default.Send<Command>(new Command() { _cmd = Constants.TIMER_INIT });
        }

        private void Load_all(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Send<Command>(new Command() { _cmd = Constants.CMD_DATA_LOAD });
        }

        [ExceptionHandler]
        private void Action_Update(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Send<Command>(new Command() { _cmd = Constants.CMD_RUN_UPDATE });
        }

        [ExceptionHandler]
        private void Action_Create(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Send<Command>(new Command() { _cmd = Constants.CMD_RUN_CREATE });
        }

        [ExceptionHandler]
        private void Action_Delete(object sender,  RoutedEventArgs e)
        {
            Messenger.Default.Send<Command>(new Command() { _cmd = Constants.CMD_RUN_DELETE });
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
