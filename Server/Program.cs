using System;
using System.Drawing;
using System.Net;
using System.Net.Sockets;




var remoteEP = new IPEndPoint(IPAddress.Any, 0);

var ip = IPAddress.Parse("127.0.0.1");
var server = new UdpClient(45678);
var port = 12345;
var clientEp = new IPEndPoint(ip, port);



while (true)
{
    server.Receive(ref remoteEP);
    var image = TakeScreenshot();
    var imageBytes = ImageToByte(image);
    var ImageBytes = imageBytes.Chunk(ushort.MaxValue - 29);
    var buffer = ImageBytes.ToArray();

    for (int i = 0; i < buffer.Length; i++)
    {
        await server.SendAsync(buffer[i], buffer[i].Length, clientEp);
    }
}

    
Image TakeScreenshot()
{
    Bitmap bitmap = new Bitmap(1920, 1080);

    Graphics graphics = Graphics.FromImage(bitmap);
    graphics.CopyFromScreen(0, 0, 0, 0, bitmap.Size);

    return bitmap;
}

byte[] ImageToByte(Image image)
{
    using (var stream = new MemoryStream())
    {
        image.Save(stream, System.Drawing.Imaging.ImageFormat.Png);

        return stream.ToArray();
    }
}