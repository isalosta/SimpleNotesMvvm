using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using wpf_mvvm_post_test;
using GalaSoft.MvvmLight.Messaging;

namespace wpf_mvvm_post_test
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        [STAThread]
        public static void Main()
        {
            wpf_mvvm_post_test.App app = new wpf_mvvm_post_test.App();
            bool isReady = false;
            App.Start((Callback) => isReady = Callback);
            while (!isReady)
            {

            }
            app.InitializeComponent();
            app.Run();
        }

        public static void Start(Action<bool> Callback)
        {
            Core core = new Core();

            Messenger.Default.Send<XmlProcessor>(new XmlProcessor());
            Messenger.Default.Send<dataModelList>(new dataModelList());
            Messenger.Default.Send<DataMain>(new DataMain());
            Messenger.Default.Send<FromServer>(new FromServer());

            Callback(true);
        }
    }
}
