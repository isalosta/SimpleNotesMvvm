using System;
using System.Windows;
using GalaSoft.MvvmLight.Messaging;

namespace wpf_mvvm_post_test
{
    /// <summary>
    /// Interaction logic for Config.xaml
    /// </summary>
    public partial class Config : Window
    {
        public Config()
        {
            InitializeComponent();
        }

        public void Click_C_Config(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Send<Command>(new Command() { _cmd = Constants.CMD_CLOSE_CONFIG });
        }

        public void Click_S_Config(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Send<Command>(new Command() { _cmd = Constants.CMD_GET_CONFIG });
        }
    }
}
