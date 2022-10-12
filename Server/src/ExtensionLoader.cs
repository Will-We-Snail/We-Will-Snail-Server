using System.Reflection;
using Lidgren.Network;
namespace WillWeSnail;

class ExtensionLoader
{
    public Dictionary<Int16, IExtension> extensions = new Dictionary<Int16, IExtension>();
    public List<Func<Int32, NetServer, Int32>> PeriodicFunctions = new List<Func<int, NetServer, int>>();
    public ExtensionLoader(String ExtensionsPath)
    {
        try
        {
            var loadingmods = Directory.EnumerateFiles(ExtensionsPath, "*.wwsext");
            foreach (string mod in loadingmods)
            {
                Assembly modAssembly = Assembly.LoadFrom(mod);
                Type modClass = modAssembly.GetTypes()
                    .FirstOrDefault(modType => modType.GetInterfaces().Contains(typeof(IExtension)));
                
                if(modClass is null) {
                    Console.WriteLine($"File {mod} has a wwsext file extension, but does not appear to be an extension. Skipping.");
                    continue;
                }
                
                IExtension extension = (IExtension)Activator.CreateInstance(modClass);
                if (extension != null)
                {
                    extension.Init();
                    extensions.Add(extension.GetID(), extension);
                    if (extension.GetPeriodic() != null)
                        PeriodicFunctions.Add(extension.GetPeriodic());
                    Console.WriteLine($"Loaded mod {extension.GetName()} with ID {extension.GetID()}");
                }
                else
                {
                    Console.WriteLine($"File {mod} has a wwsext file extension, but does not appear to be an extension. Skipping.");
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            System.Environment.Exit(1);
        }
    }
}
