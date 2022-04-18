using System.Reflection;
using Extension;

class ExtensionLoader
{
    List<IExtension> extensions = new List<IExtension>();
    public ExtensionLoader(String ExtensionsPath)
    {
        try
        {
            var loadingmods = Directory.EnumerateFiles(ExtensionsPath, "*.wwsext");
            foreach (string mod in loadingmods)
            {
                Assembly modAssembly = Assembly.LoadFrom(mod);
                Type modClass = modAssembly.GetType("mod", false, true);
                IExtension extension = (IExtension)Activator.CreateInstance(modClass);
                if (extension != null)
                {
                    extension.init();
                    extensions.Add(extension);
                    Console.WriteLine($"Loaded mod {extension.getName()} with ID {extension.getID()}");
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