namespace Extension
{
    public interface IExtension
    {
        public void init();
        public int getID();
        public string getName();

        public (int period, Func<int> function) getPeriodic();
    }
}