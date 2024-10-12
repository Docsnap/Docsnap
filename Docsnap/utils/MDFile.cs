using System.Reflection;
using System.Text;
using Docsnap.data;

namespace Docsnap.utils;

public class MDFile
{
    public static void CreateMDFiles(string pathController, Type controller)
    {
        StringBuilder content = new();

        string classRoute = MethodsAndController.GetControllerRoute(controller);
        content.AppendLine($"# &{controller.Name}");
        content.AppendLine($"       Controller route: {classRoute}");

        IEnumerable<MethodsWithRoutes> methods = MethodsAndController.ScanAllMethods(controller);
        foreach ((MethodInfo method, string route) in methods)
        {
            string fullRoute = route[0] == '/' ? route : $"{classRoute}/{route}";

            content.AppendLine($"## @@{method.Name}");
            content.AppendLine($"       {fullRoute}");
        }

        File.WriteAllText(pathController, content.ToString());
    }

    public static void AjustRoutesMDFiles(string pathController, Type controller)
    {
        List<string> existingFileLines = [.. File.ReadAllLines(pathController)];
        StringBuilder content = existingFileLines.Aggregate(new StringBuilder(), (sb, s) => sb.AppendLine(s));

        IEnumerable<MethodsWithRoutes> methods = MethodsAndController.ScanAllMethods(controller);
        foreach ((MethodInfo method, string route) in methods)
        {
            string classRoute = MethodsAndController.GetControllerRoute(controller);
            string fullRoute = !string.IsNullOrEmpty(route) ? route[0] == '/' ? route : $"{classRoute}/{route}" : string.Empty;

            bool methodExists = MethodsAndController.CheckAndUpdateAllMethods(new CheckAndUpdateMethods(
                existingFileLines,
                method.Name,
                fullRoute
            ), out bool routeUpdated);

            if (!methodExists)
            {
                content.AppendLine($"## @@{method.Name}");
                content.AppendLine($"       {fullRoute}");
            }
            else if (routeUpdated)
            {
                content = UpdateRouteInMDFile(new CheckAndUpdateMethods(
                    existingFileLines,
                    method.Name,
                    fullRoute
                ));
            }
        }

        File.WriteAllText(pathController, content.ToString());
    }


    private static StringBuilder UpdateRouteInMDFile(CheckAndUpdateMethods checkAndUpdate)
    {
        StringBuilder updatedContent = new();

        for (int i = 0; i < checkAndUpdate.FileLines.Count; i++)
        {
            updatedContent.AppendLine(checkAndUpdate.FileLines[i]);
            if (checkAndUpdate.FileLines[i].StartsWith($"## @@{checkAndUpdate.MethodName}"))
            {
                updatedContent.AppendLine($"       {checkAndUpdate.Route}");
                i++;
            }
        }

        return updatedContent;
    }
}