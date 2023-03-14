using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        UdpClient client;
        IPEndPoint connectEP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12345);
        IPEndPoint serverEP;
        

        public MainWindow()
        {
            InitializeComponent();
            client = new(connectEP);
            serverEP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 45678);
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {

            var len = 0;
            var size = ushort.MaxValue - 29;
            var buffer = new byte[size];
            client.Send(buffer, buffer.Length, connectEP);
            var list = new List<byte>();

            buffer = new byte[size];
            do
            {
                UdpReceiveResult result = await client.ReceiveAsync();
                len = result.Buffer.Length;
                list.AddRange(buffer.Take(len));
                var image = GetImage(list.ToArray());
                Image1.Source = image;

            } while (len == buffer.Length);
        }

        private static BitmapImage GetImage(byte[] imageInfo)
        {
            var image = new BitmapImage();

            using (var memoryStream = new MemoryStream(imageInfo))
            {
                memoryStream.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = memoryStream;
                image.EndInit();
            }

            image.Freeze();

            return image;
        }
    }
}
