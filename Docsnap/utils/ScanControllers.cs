using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace docsnap.utils;

public class ScanControllers
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
            string fileController = $"{Path}/{controller.Name}.md";

            if (!File.Exists(fileController))
            {
                StringBuilder content = new();

                IEnumerable<MethodInfo> methods = controller.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                                           .Where(m => m.IsPublic && m.DeclaringType == controller);

                foreach (MethodInfo method in methods)
                {
                    content.AppendLine($"## {method.Name}");
                }

                File.WriteAllText(fileController, content.ToString());
            }
        }
    }
}