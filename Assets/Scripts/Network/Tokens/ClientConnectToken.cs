using System.Text;
using Bolt;
using UdpKit;

class ClientConnectToken : IProtocolToken
{
    public string UserName;

    public void Read(UdpPacket packet)
    {
        UserName = packet.ReadString(Encoding.Unicode);
    }

    public void Write(UdpPacket packet)
    {
        packet.WriteString(UserName, Encoding.Unicode, UserName.Length);
    }
}