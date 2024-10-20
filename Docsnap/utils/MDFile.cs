using System.Text;
using Docsnap.data;

namespace Docsnap.utils;

internal class MDFile
{
    internal static void CreateMDFiles(string pathController, Type controller)
    {
        StringBuilder content = new();

        string classRoute = MethodsAndController.GetControllerRoute(controller);
        content.AppendLine($"# &{controller.Name}");
        content.AppendLine($"       Controller route: {classRoute}");

        IEnumerable<MethodsWithRoutesAndEndpoint> methods = MethodsAndController.ScanAllMethods(controller);
        foreach ((string endpoint, string method, string route) in methods)
        {
            string fullRoute = !string.IsNullOrEmpty(route) ? route[0] == '/' ? route : $"{classRoute}/{route}" : string.Empty;

            content.AppendLine($"## @@{endpoint}");
            content.AppendLine($"### &{method}");
            content.AppendLine($"       {fullRoute}");
        }

        File.WriteAllText(pathController, content.ToString());
    }

    internal static void AjustRoutesMDFiles(string pathController, Type controller)
    {
        List<string> existingFileLines = [.. File.ReadAllLines(pathController)];
        IEnumerable<MethodsWithRoutesAndEndpoint> methods = MethodsAndController.ScanAllMethods(controller);

        foreach ((string endpoint, string method, string route) in methods)
        {
            string classRoute = MethodsAndController.GetControllerRoute(controller);
            string fullRoute = !string.IsNullOrEmpty(route) ? route[0] == '/' ? route : $"{classRoute}/{route}" : classRoute;

            bool endpointExists = MethodsAndController.CheckAndUpdateAllEndpoints(new CheckAndUpdateEndpoints(
                endpoint,
                method,
                fullRoute
            ), out bool needToUpdate, existingFileLines);

            if (!endpointExists)
            {
                UpdateEndpointInMDFile(new CheckAndUpdateEndpoints(
                    endpoint,
                    method,
                    fullRoute
                ), ref existingFileLines);
            }
            else if (needToUpdate)
            {
                UpdateTypeMethodInMDFile(new CheckAndUpdateEndpoints(
                    endpoint,
                    method,
                    fullRoute
                ), ref existingFileLines);

                UpdateRouteInMDFile(new CheckAndUpdateEndpoints(
                    endpoint,
                    method,
                    fullRoute
                ), ref existingFileLines);
            }
        }

        StringBuilder content = existingFileLines.Aggregate(new StringBuilder(), (stringBuilder, lines) => stringBuilder.AppendLine(lines));
        File.WriteAllText(pathController, content.ToString());
    }

    private static void UpdateEndpointInMDFile(CheckAndUpdateEndpoints checkAndUpdate, ref List<string> fileLines)
    {
        bool methodUpdated = false;

        for (int i = 0; i < fileLines.Count; i++)
        {
            if (fileLines[i].Trim() == checkAndUpdate.Route)
            {
                if (fileLines[i - 2].StartsWith($"## @@{checkAndUpdate.Endpoint}"))
                {
                    if (fileLines[i - 1].StartsWith($"### &{checkAndUpdate.MethodHttp}"))
                    {

                        fileLines[i - 2] = $"## @@{checkAndUpdate.Endpoint}";
                        fileLines[i - 1] = $"### &{checkAndUpdate.MethodHttp}";
                        fileLines[i] = $"       {checkAndUpdate.Route}";

                        methodUpdated = true;
                    }
                }
            }
        }

        if (!methodUpdated)
        {
            fileLines.Add($"## @@{checkAndUpdate.Endpoint}");
            fileLines.Add($"### &{checkAndUpdate.MethodHttp}");
            fileLines.Add($"       {checkAndUpdate.Route}");
        }
    }

    private static void UpdateRouteInMDFile(CheckAndUpdateEndpoints checkAndUpdate, ref List<string> fileLines)
    {
        List<string> updatedContent = [];

        for (int i = 0; i < fileLines.Count; i++)
        {
            updatedContent.Add(fileLines[i]);
            if (fileLines[i].StartsWith($"## @@{checkAndUpdate.Endpoint}"))
            {
                if (fileLines[i + 1].StartsWith($"### &{checkAndUpdate.MethodHttp}"))
                {
                    updatedContent.Add(fileLines[i + 1]);
                    updatedContent.Add($"       {checkAndUpdate.Route}");
                    i += 2;
                }
            }
        }

        fileLines = updatedContent;
    }

    private static void UpdateTypeMethodInMDFile(CheckAndUpdateEndpoints checkAndUpdate, ref List<string> fileLines)
    {
        List<string> updatedContent = [];

        for (int i = 0; i < fileLines.Count; i++)
        {
            updatedContent.Add(fileLines[i]);
            if (fileLines[i].StartsWith($"## @@{checkAndUpdate.Endpoint}"))
            {
                if (!fileLines[i + 1].Trim().Contains(checkAndUpdate.Route))
                {
                    i++;
                }

                updatedContent.Add($"### &{checkAndUpdate.MethodHttp}");
            }
        }

        fileLines = updatedContent;
    }
}