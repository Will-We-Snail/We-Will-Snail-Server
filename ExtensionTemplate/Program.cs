using Extension;
class mod : IExtension{

    public void init(){}
    public int getID(){
        return 0;
    }
    public string getName(){
        return "Extension Template";
    }

    public int e(){
        return 3;
    }
    public (int period, Func<int> function) getPeriodic(){
        return (3, e);
    }
}