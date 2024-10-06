using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace docsnap.utils;

/// <summary>
/// This class is responsible to see all of things about the project linked to this library 
/// </summary>
public class Watcher
{
    public static void ScanAllControllers(string Path)
    {
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

        List<Type> controllers = assemblies
            .SelectMany(a => a.GetTypes())
            .Where(t => typeof(ControllerBase).IsAssignableFrom(t) && !t.IsAbstract)
            .ToList();

        foreach (Type controller in controllers)
        {
            string pathController = $"{Path}/{controller.Name}.md";

            if (!File.Exists(pathController))
            {
                StringBuilder content = new();

                IEnumerable<MethodInfo> methods = ScanAllMethods(controller);
                foreach (MethodInfo method in methods)
                {
                    content.AppendLine($"## {method.Name}");
                }

                File.WriteAllText(pathController, content.ToString());
            }
        }
    }

    private static IEnumerable<MethodInfo> ScanAllMethods(Type controller)
    {
        return controller.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                                   .Where(m => m.IsPublic && m.DeclaringType == controller);
    }
}