using System.Runtime.InteropServices;

namespace DosAttack
{
    public static class PackageBuilder
    {
		public static byte[] ConvertToByteArray(IpHeader ipHdr, TcpHeader tcpHdr)
		{
			var ipHeaderSize = Marshal.SizeOf(ipHdr);
			var tcpHeaderSize = Marshal.SizeOf(tcpHdr);
			var size = ipHeaderSize + tcpHeaderSize;
			var result = new byte[size];

			var ptr = Marshal.AllocHGlobal(ipHeaderSize);
			Marshal.StructureToPtr(ipHdr, ptr, true);
			Marshal.Copy(ptr, result, 0, ipHeaderSize);
			Marshal.FreeHGlobal(ptr);

			ptr = Marshal.AllocHGlobal(tcpHeaderSize);
			Marshal.StructureToPtr(tcpHdr, ptr, true);
			Marshal.Copy(ptr, result, ipHeaderSize, tcpHeaderSize);
			Marshal.FreeHGlobal(ptr);

			return result;
		}
	}
}
