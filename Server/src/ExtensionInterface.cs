namespace WillWeSnail;
using Lidgren.Network;
public interface IExtension
{
    public void Init();
    public Int16 GetID();
    public String GetName();

    public Func<Int32, NetServer, Int32> GetPeriodic();
}
