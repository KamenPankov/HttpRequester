using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace HttpRequester
{
    class Program
    {
        static void Main(string[] args)
        {
            const string HttpNewLine = "\r\n";

            TcpListener tcpListener = new TcpListener(IPAddress.Loopback, 25456);
            tcpListener.Start();

            while (true)
            {
                TcpClient client = tcpListener.AcceptTcpClient();

                using (NetworkStream networkStream = client.GetStream())
                {
                    byte[] requestBytes = new byte[100000];
                    int bytesRead = networkStream.Read(requestBytes, 0, requestBytes.Length);

                    string request = Encoding.UTF8.GetString(requestBytes, 0, bytesRead);

                    string responseText = "<form action=\"/Account/Login\" method=\"POST\">" +
                                            "<input type=date name=\"date\" />" +
                                            "<input type=name name=\"name\" />" +
                                            "<input type=password name=\"password\" />" +
                                            "<input type=submit value=\"Login\" />" +
                                           "</ form>";

                    string response = "HTTP/1.0 307 OK" + HttpNewLine
                                    + "Server: KamServer/1.0" + HttpNewLine
                                    + "Content-Type: text/html" + HttpNewLine
                                    + "Content-Length: " + responseText.Length + HttpNewLine
                                    //+ "Content-Disposition: attachment; kamen.html" + HttpNewLine
                                    + "Location: https://softuni.bg" + HttpNewLine
                                    + HttpNewLine 
                                    + responseText;

                    byte[] responseBytes = Encoding.UTF8.GetBytes(response);
                    networkStream.Write(responseBytes, 0, responseBytes.Length);

                    Console.WriteLine(request);
                    Console.WriteLine(new string('=', 70));
                }
                
            }
            

        }
    }
}
