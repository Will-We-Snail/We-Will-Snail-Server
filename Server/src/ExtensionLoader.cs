using System.Reflection;
using Extension;

class ExtensionLoader
{
    public ExtensionLoader(String ExtensionsPath){
        List<IExtension> extensions = new List<IExtension>();
        try {
            var loadingmods = Directory.EnumerateFiles(ExtensionsPath, "*.wwsext");
            foreach (string mod in loadingmods){
                Assembly modAssembly = Assembly.LoadFrom(mod);
                Type modClass = modAssembly.GetType("mod", false, true); 
                    IExtension extension = (IExtension) Activator.CreateInstance(modClass);
                    Console.WriteLine(String.Format("Loaded mod {0} with ID {1}", extension.getName(), extension.getID()));
                    Console.Write("");
            }
        }catch (Exception e){
            Console.WriteLine(e);
            System.Environment.Exit(1);
        }
    }
}