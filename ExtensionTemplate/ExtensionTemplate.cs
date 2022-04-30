using WillWeSnail;
using Lidgren.Network;
class mod : IExtension
{

    public void ManagePacket(NetIncomingMessage message, NetServer server)
    {
        Guid guid = new Guid(message.ReadBytes(16));
        Int16 PacketId = message.ReadInt16();
        switch (PacketId)
        {
            case 1:
                Int32 LevelId = message.ReadInt32();
                Double PosX = message.ReadDouble();
                Double PosY = message.ReadDouble();
                Boolean lookDir = message.ReadBoolean();
                Console.WriteLine($"Player Guid: {guid}\nLevel: {LevelId}\nX Position: {PosX}\nY Position: {PosY}\nLooking: {lookDir}");
                break;
            default:
                Console.WriteLine("Invalid");
                break;
        }
    }

    public void Init() { }
    public Int16 GetID()
    {
        return 0;
    }
    public string GetName()
    {
        return "Extension Template";
    }

    public Int32 e(Int32 tickNumber, NetServer server)
    {
        return 0;
    }
    public Func<Int32, NetServer, Int32> GetPeriodic()
    {
        return e;
    }
}
