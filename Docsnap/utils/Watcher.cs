using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Docsnap.utils;

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

                string classRoute = GetControllerRoute(controller);
                content.AppendLine($"# {controller.Name}");
                content.AppendLine($"Controller route: {classRoute}");

                IEnumerable<(MethodInfo, string)> methods = ScanAllMethods(controller);
                foreach ((MethodInfo method, string route) in methods)
                {
                    content.AppendLine($"## {method.Name}");
                    Console.WriteLine("Rotas: " + route);
                    content.AppendLine($"## Route: {route}");
                }

                File.WriteAllText(pathController, content.ToString());
            }
        }
    }

    private static string GetControllerRoute(Type controller)
    {
        RouteAttribute? routeAttribute = controller.GetCustomAttribute<RouteAttribute>();
        
        if (routeAttribute != null)
        {
            // Substitui o placeholder [controller] com o nome da classe do controller
            return routeAttribute.Template?.Replace("[controller]", controller.Name.Replace("Controller", "")) ?? string.Empty;
        }

        return string.Empty;
    }

    private static IEnumerable<(MethodInfo method, string route)> ScanAllMethods(Type controller)
    {
        IEnumerable<MethodInfo> methods = controller.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                                   .Where(m => m.IsPublic && m.DeclaringType == controller);

        foreach (MethodInfo method in methods)
        {
            List<Attribute> routesAttributes = method.GetCustomAttributes()
                                .Where(attribute => attribute is RouteAttribute || attribute.GetType().Name.StartsWith("Http"))
                                .ToList();

            foreach (Attribute attribute in routesAttributes)
            {
                string route = string.Empty;

                if (attribute is RouteAttribute routeAttribute)
                {
                    route = routeAttribute.Template ?? string.Empty;
                }
                else if (attribute is HttpMethodAttribute httpAttribute)
                {
                    route = httpAttribute.Template ?? string.Empty;
                }

                yield return (method, route);
            }
        }
    }
}