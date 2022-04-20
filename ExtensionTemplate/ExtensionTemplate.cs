using WillWeSnail;
using Lidgren.Network;
class mod : IExtension{

    public void Init(){}
    public Int16 GetID(){
        return 0;
    }
    public string GetName(){
        return "Extension Template";
    }

    public Int32 e(Int32 tickNumber, NetServer server){
        return 0;
    }
    public Func<Int32, NetServer, Int32> GetPeriodic(){
        return e;
    }
}
