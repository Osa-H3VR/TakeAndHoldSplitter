using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace Osa.TakeAndHoldSplitter
{
 public class LiveSplitClient
    {
        private readonly string _host;
        private readonly int _port;
        private readonly Socket _socket;

        public LiveSplitClient(string host, int port)
        {
            _host = host;
            _port = port;
            _socket = new Socket(AddressFamily.InterNetwork, 
                SocketType.Stream, ProtocolType.Tcp);
        }

        private void SendCommand(string command)
        {
            try
            {
                if (!_socket.Connected)
                {
                    var add = Dns.GetHostEntry(_host).AddressList.First(x => x.AddressFamily== AddressFamily.InterNetwork);
                    IPEndPoint ipe = new IPEndPoint(add, _port);
                    _socket.Connect(ipe);
                }

                byte[] msg = System.Text.Encoding.ASCII.GetBytes(command);
                _socket.Send(msg);
            }
            catch (ArgumentNullException ae)
            {
                Console.WriteLine("ArgumentNullException : {0}", ae.ToString());
            }
            catch (SocketException se)
            {
                Console.WriteLine("SocketException : {0}", se.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unexpected exception : {0}", ex.ToString());
            }
        }

        public void InitGameTime() => SendCommand("initgametime\r\n");
        public void StartTimer() => SendCommand("starttimer\r\n");
        public void Reset() => SendCommand("reset\r\n");
        public void Split() => SendCommand("split\r\n");
        public void Unsplit() => SendCommand("unsplit\r\n");
        public void Pause() => SendCommand("pause\r\n");
        public void Resume() => SendCommand("resume\r\n");
        
        public void Dispose()
        {
            if (_socket.Connected)
            {
                _socket.Shutdown(SocketShutdown.Both);
            }
            _socket.Close();
        }
    }
}