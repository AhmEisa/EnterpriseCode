using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WCF.WindowsHost
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            MainUI = this;

            this.Title = "UI Running on Thread " + Thread.CurrentThread.ManagedThreadId + " | Process " + Process.GetCurrentProcess().Id.ToString();
            _SyncContext = SynchronizationContext.Current;
        }
        ServiceHost _hostMessageManager = null;
        SynchronizationContext _SyncContext = null;
        public static MainWindow MainUI { get; set; }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _hostMessageManager = new ServiceHost(typeof(MessageManager));
            _hostMessageManager.Open();

            btnStart.IsEnabled = false;
            btnStop.IsEnabled = true;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            _hostMessageManager.Close();

            btnStart.IsEnabled = true;
            btnStop.IsEnabled = false;
        }

        public void ShowMessage(string message)
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            SendOrPostCallback callback = new SendOrPostCallback(arg =>
            {
                lblMessage.Content = message + Environment.NewLine + " marshaled from  thread " + threadId.ToString() + " to thread " +
                Thread.CurrentThread.ManagedThreadId.ToString() + " | Process " + Process.GetCurrentProcess().Id.ToString();
            });

            _SyncContext.Send(callback, null);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Thread thread = new Thread(() =>
            {
                ChannelFactory<IMessageService> factory = new ChannelFactory<IMessageService>("");
                IMessageService proxy = factory.CreateChannel();

                proxy.ShowMessage(DateTime.Now.ToLongTimeString() + " from in-process call");

                factory.Close();
            });

            thread.IsBackground = true;
            thread.Start();

        }
    }
}
