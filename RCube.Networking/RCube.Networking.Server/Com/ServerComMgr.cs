using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using NSpring.Logging;
using NSpring.Logging.Loggers;
using RCube.Networking;
using RCube.Networking.Messages;
using RCube.Networking.Server.MessageManagement;

namespace RCube.Networking.Server.Communications
{
    internal sealed class ServerComMgr : IServerComMgr
    {
        private Socket _socket;
        private IServiceProvider _serviceProvider;

        public ServerComMgr(IServiceProvider svcProvider)
        {
            _serviceProvider = svcProvider;
        }

        private Logger Logger
        {
            get { return _serviceProvider.GetService(typeof(Logger)) as Logger; }
        }

        private IMessageMgr MessageMgr
        {
            get { return _serviceProvider.GetService(typeof(IMessageMgr)) as IMessageMgr; }
        }

        public bool IsRunning
        {
            get { return (_socket != null && _socket.IsBound); }
        }

        public void Start()
        {
            try
            {
                Logger.Log(Level.Info, "Server is attempting to start.");

                if (IsRunning)
                    throw new Exception("The server is already running.");

                // Create the new socket and bind it to a local IP address/port.
                _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                _socket.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8081));

                ListenForMessage();
            }
            catch (Exception ex)
            {
                Logger.Log(ex, "Start Error");
                Logger.Log(Level.Info, "The server was unable to start.");

                _socket = null;
                throw ex;
            }
        }

        public void Stop()
        {
            if (!IsRunning)
                throw new Exception("The server is already not running.");

            if (_socket != null)
            {
                _socket.Close();
                _socket = null;
            }
        }

        public void SendToClient(EndPoint ep, DataMessage msg)
        {
            try
            {
                Logger.Log("Sending message to client " + ep.ToString());

                MemoryStream ms = new MemoryStream();
                BinaryFormatter bf = new BinaryFormatter();
                byte[] sendBuffer = null;

                // Convert the message to its binary representation.
                bf.Serialize(ms, msg);
                sendBuffer = ms.GetBuffer();

                _socket.BeginSendTo(sendBuffer, 0, sendBuffer.Length, SocketFlags.None,
                    ep, new AsyncCallback(OnSend), ep);
            }
            catch (Exception ex)
            {
                Logger.Log(ex, "Send Error");
                Logger.Log(Level.Info, "Error encountered while sending a message.");
            }
        }

        private void OnSend(IAsyncResult ar)
        {
            try
            {
                Logger.Log("Message send is complete.");
                _socket.EndSendTo(ar);
            }
            catch (Exception ex)
            {
                Logger.Log(ex, "Send error");
                Logger.Log(Level.Info, "Error encountered while sending a message.");
            }
        }

        private void OnRecieve(IAsyncResult ar)
        {
            try
            {
                byte[] buffer = (byte[])ar.AsyncState;
                EndPoint ipEndPt = new IPEndPoint(IPAddress.Any, 8080);
                int recv = _socket.EndReceiveFrom(ar, ref ipEndPt);

                Logger.Log("Recieved a message from " + ipEndPt.ToString());

                // Convert the binary message into a IDataMessage so we can read it
                // an figure out what is needed to handle the message.
                MemoryStream ms = new MemoryStream(buffer);
                BinaryFormatter bf = new BinaryFormatter();
                DataMessage msg = bf.Deserialize(ms) as DataMessage;

                MessageMgr.RecieveMessage(ipEndPt, msg);
            }
            catch (Exception ex)
            {
                Logger.Log(ex, "Recive Error");
                Logger.Log(Level.Info, "Error encountered while recieving a message.");
            }
            finally
            {
                ListenForMessage();
            }
        }

        private void ListenForMessage()
        {
            Logger.Log(Level.Info, "Listening for client messages.");

            EndPoint ep = new IPEndPoint(IPAddress.Any, 8080);
            byte[] buffer = new byte[1024];
            
            _socket.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None,
                ref ep, new AsyncCallback(OnRecieve), buffer);
        }
    }
}
