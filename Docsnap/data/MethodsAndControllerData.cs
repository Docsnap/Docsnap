using System.Reflection;

namespace Docsnap.data;

public struct MethodsWithRoutes(MethodInfo method, string route)
{
    public MethodInfo Method = method;
    public string Route = route;

    public readonly void Deconstruct(out MethodInfo method, out string route)
    {
        method = Method;
        route = Route;
    }
}

public struct CheckAndUpdateMethods
{
    public List<string> FileLines;
    public string MethodName;
    public string Route;

    public CheckAndUpdateMethods(List<string> fileLines, string methodName, string fullRoute)
    {
        FileLines = fileLines;
        MethodName = methodName;
        Route = fullRoute;
    }

    public CheckAndUpdateMethods()
    {
        FileLines = [];
        MethodName = string.Empty;
        Route = string.Empty;
    }
}