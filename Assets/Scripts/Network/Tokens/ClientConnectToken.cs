using System.Text;
using Bolt;
using UdpKit;

internal class ClientConnectToken : IProtocolToken
{
    public string UserName;

    public void Read(UdpPacket packet)
    {
        UserName = packet.ReadString(Encoding.UTF8);
    }

    public void Write(UdpPacket packet)
    {
        packet.WriteString(UserName, Encoding.UTF8, UserName.Length);
    }
}