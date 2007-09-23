using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using RCube.Networking;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using RCube.Networking.Messages;

namespace RCube.Networking.Server.ClientTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Socket clientSocket;
            EndPoint epServer;

            try
            {
                //Using UDP sockets
                clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                //IP address of the server machine
                IPAddress ipAddress = IPAddress.Parse("127.0.0.1");

                //Server is listening on port 1000
                IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, 8081);

                epServer = (EndPoint)ipEndPoint;

                DataMessageHeader dmh = new DataMessageHeader();
                dmh.MessageId = Guid.NewGuid();
                dmh.Timestamp = DateTime.Now;
 
                RegisterClientMessage rcm = new RegisterClientMessage(dmh);
                rcm.NeedsAcknowledgement = true;
                rcm.Type = RegisterClientMessage.RegistrationType.Register;
                
                MemoryStream ms = new MemoryStream();
                BinaryFormatter bf = new BinaryFormatter();

                bf.Serialize(ms, rcm);
                byte[] byteData = ms.GetBuffer();

                //Login to the server
                clientSocket.BeginSendTo(byteData, 0, byteData.Length,
                    SocketFlags.None, epServer, new AsyncCallback(OnSend), clientSocket);
            }
            catch
            {
            }

            Console.In.ReadLine();
        }

        private static void OnSend(IAsyncResult ar)
        {
            Socket s = ar.AsyncState as Socket;
            s.EndSendTo(ar);

            byte[] buffer = new byte[1024];
            EndPoint ep = new IPEndPoint(IPAddress.Any, 8080);

            s.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None,
                ref ep, new AsyncCallback(OnRecieve), new object[] { s, buffer });
        }

        private static void OnRecieve(IAsyncResult ar)
        {
            object[] args = (object[])ar.AsyncState;
            Socket s = (Socket)args[0];
            byte[] buffer = (byte[])args[1];

            EndPoint ipEndPt = new IPEndPoint(IPAddress.Any, 8080);
            int recv = s.EndReceiveFrom(ar, ref ipEndPt);

            // Convert the binary message into a IDataMessage so we can read it
            // an figure out what is needed to handle the message.
            MemoryStream ms = new MemoryStream(buffer);
            BinaryFormatter bf = new BinaryFormatter();
            DataMessage msg = bf.Deserialize(ms) as DataMessage;

            //msg.HandleClientRequirements(ipEndPt, null);

            DataMessageHeader dmh = new DataMessageHeader();
            dmh.SessionId = msg.Header.SessionId;
            dmh.MessageId = Guid.Empty;
            dmh.Timestamp = DateTime.Now;

            RegisterClientMessage rcm = new RegisterClientMessage(dmh);
            rcm.NeedsAcknowledgement = false;
            rcm.Type = RegisterClientMessage.RegistrationType.Unregister;

            ms = new MemoryStream();
            bf = new BinaryFormatter();

            bf.Serialize(ms, rcm);
            byte[] byteData = ms.GetBuffer();

            //Login to the server
            s.BeginSendTo(byteData, 0, byteData.Length,
                SocketFlags.None, ipEndPt, new AsyncCallback(OnSend), s);
        }
    }
}