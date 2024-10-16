using System.Reflection;
using Docsnap.data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Docsnap.utils;

public class MethodsAndController
{
    public static void ScanAllControllers(string Path)
    {
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

        List<Type> controllers = assemblies
                                    .SelectMany(assembly => assembly.GetTypes())
                                    .Where(assembly => typeof(ControllerBase).IsAssignableFrom(assembly) && !assembly.IsAbstract)
                                    .ToList();

        foreach (Type controller in controllers)
        {
            string pathController = $"{Path}/{controller.Name}.md";

            if (!File.Exists(pathController))
            {
                MDFile.CreateMDFiles(pathController, controller);
            }
            else
            {
                MDFile.AjustRoutesMDFiles(pathController, controller);
            }
        }
    }

    public static string GetControllerRoute(Type controller)
    {
        RouteAttribute? routeAttribute = controller.GetCustomAttribute<RouteAttribute>();

        if (routeAttribute != null)
        {
            return routeAttribute.Template?.Replace("[controller]", controller.Name.Replace("Controller", "")) ?? string.Empty;
        }

        return string.Empty;
    }

    public static IEnumerable<MethodsWithRoutes> ScanAllMethods(Type controller)
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

                yield return new MethodsWithRoutes()
                {
                    Method = method,
                    Route = route
                };
            }
        }
    }

    public static bool CheckAndUpdateAllMethods(CheckAndUpdateMethods checkAndUpdate, out bool routeUpdated)
    {
        routeUpdated = false;
        bool methodExists = false;

        for (int i = 0; i < checkAndUpdate.FileLines.Count - 1; i++)
        {
            if (checkAndUpdate.FileLines[i].StartsWith($"## @@{checkAndUpdate.MethodName}"))
            {
                methodExists = true;

                if (checkAndUpdate.FileLines[i + 1].Trim() == checkAndUpdate.Route)
                {
                    continue;
                }
                else
                {
                    routeUpdated = true;
                }
            }
        }

        return methodExists;
    }
}