namespace Docsnap.data;

public struct MethodsWithRoutesAndEndpoint(string endpoint, string method, string route)
{
    public string Endpoint = endpoint;
    public string MethodHttp = method;
    public string Route = route;

    public readonly void Deconstruct(out string endpoint, out string method, out string route)
    {
        endpoint = Endpoint;
        method = MethodHttp;
        route = Route;
    }
}

public struct CheckAndUpdateEndpoints
{
    public string Endpoint;
    public string MethodHttp;
    public string Route;

    public CheckAndUpdateEndpoints(string endpoint, string method, string fullRoute)
    {
        Endpoint = endpoint;
        MethodHttp = method;
        Route = fullRoute;
    }

    public CheckAndUpdateEndpoints()
    {
        Endpoint = string.Empty;
        MethodHttp = string.Empty;
        Route = string.Empty;
    }
}