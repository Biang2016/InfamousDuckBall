using System.Text;
using Bolt;
using UdpKit;

class ServerRefuseToken : IProtocolToken
{
    public string Message;

    public void Read(UdpPacket packet)
    {
        Message = packet.ReadString(Encoding.Unicode);
    }

    public void Write(UdpPacket packet)
    {
        packet.WriteString(Message, Encoding.Unicode, Message.Length);
    }
}