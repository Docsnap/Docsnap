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
            string fullRoute = !string.IsNullOrEmpty(route) ? route[0] == '/' ? route : $"{classRoute}/{route}" : string.Empty;

            content.AppendLine($"## @@{method.Name}");
            content.AppendLine($"       {fullRoute}");
        }

        File.WriteAllText(pathController, content.ToString());
    }

    public static void AjustRoutesMDFiles(string pathController, Type controller)
    {
        List<string> existingFileLines = [.. File.ReadAllLines(pathController)];
        //  StringBuilder content = existingFileLines.Aggregate(new StringBuilder(), (stringBuilder, lines) => stringBuilder.AppendLine(lines));

        IEnumerable<MethodsWithRoutes> methods = MethodsAndController.ScanAllMethods(controller);
        foreach ((MethodInfo method, string route) in methods)
        {
            string classRoute = MethodsAndController.GetControllerRoute(controller);
            string fullRoute = !string.IsNullOrEmpty(route) ? route[0] == '/' ? route : $"{classRoute}/{route}" : string.Empty;

            bool methodExists = MethodsAndController.CheckAndUpdateAllMethods(new CheckAndUpdateMethods(
                method.Name,
                fullRoute
            ), out bool needToUpdate, existingFileLines);

            if (!methodExists)
            {
                UpdateMethodInMDFile(new CheckAndUpdateMethods(
                    method.Name,
                    fullRoute
                ), ref existingFileLines);
            }
            else if (needToUpdate)
            {
                UpdateRouteInMDFile(new CheckAndUpdateMethods(
                    method.Name,
                    fullRoute
                ), ref existingFileLines);
            }
        }

        StringBuilder content = existingFileLines.Aggregate(new StringBuilder(), (stringBuilder, lines) => stringBuilder.AppendLine(lines));
        File.WriteAllText(pathController, content.ToString());
    }

    private static void UpdateMethodInMDFile(CheckAndUpdateMethods checkAndUpdate, ref List<string> fileLines)
    {
        //     StringBuilder tempContent = new();
        //     StringBuilder updateContent = new();
        bool methodUpdated = false;

        for (int i = 0; i < fileLines.Count; i++)
        {
            // updateContent.AppendLine(checkAndUpdate.FileLines[i]);

            if (fileLines[i].Trim() == checkAndUpdate.Route)
            {
                if (fileLines[i - 1].StartsWith("## @@"))
                {
                    // for (int j = 0; j < i - 1; j++)
                    // {
                    //     tempContent.AppendLine(checkAndUpdate.FileLines[j]);
                    // }

                    //Adicionar a linha acima em vez de sobrescrever ela ???
                    fileLines[i - 1] = $"## @@{checkAndUpdate.MethodName}";
                    fileLines[i] = $"       {checkAndUpdate.Route}";

                    // tempContent.AppendLine($"## @@{checkAndUpdate.MethodName}");
                    // tempContent.AppendLine($"       {checkAndUpdate.Route}");

                    // updateContent.Clear();
                    // updateContent = tempContent;

                    methodUpdated = true;
                }
            }
        }

        // updateContent = tempContent;

        if (!methodUpdated)
        {
            fileLines.Add($"## @@{checkAndUpdate.MethodName}");
            fileLines.Add($"       {checkAndUpdate.Route}");
            // updateContent.Append(updateContent);
            // updateContent.AppendLine($"## @@{checkAndUpdate.MethodName}");
            // updateContent.AppendLine($"       {checkAndUpdate.Route}");
        }

        // Console.WriteLine(updateContent);
        // return updateContent;
    }

    private static void UpdateRouteInMDFile(CheckAndUpdateMethods checkAndUpdate, ref List<string> fileLines)
    {
       // StringBuilder updatedContent = new();
       List<string> updatedContent = [];

        for (int i = 0; i < fileLines.Count; i++)
        {
            updatedContent.Add(fileLines[i]);
            if (fileLines[i].StartsWith($"## @@{checkAndUpdate.MethodName}"))
            {
           //     updatedContent.AppendLine($"       {checkAndUpdate.Route}");
                updatedContent.Add($"       {checkAndUpdate.Route}");
                i++;
            }
        }

        fileLines = updatedContent;
       // return updatedContent;
    }
}