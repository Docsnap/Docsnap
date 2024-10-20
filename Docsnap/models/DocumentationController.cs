using Docsnap.interfaces;

namespace Docsnap.Models;

internal class DocumentationEndpoint : IDocumentationEndpoint
{
    public string EndpointName { get; set; }
    public string EndpointMethod { get; set; }
    public string EndpointRoute { get; set; }
    public List<string> ContentEndpoint { get; set; }


    public DocumentationEndpoint(string endpoint, string method, string route, List<string> contentEndpoint)
    {
        EndpointName = endpoint;
        EndpointMethod = method;
        EndpointRoute = route;
        ContentEndpoint = contentEndpoint;
    }

    public DocumentationEndpoint()
    {
        EndpointName = string.Empty;
        EndpointMethod = string.Empty;
        EndpointRoute = string.Empty;
        ContentEndpoint = [];
    }
}

internal class DocumentationController
{
    public string ControllerName { get; set; }
    public string ControllerRoute { get; set; }
    public List<DocumentationEndpoint> EndpointsCollection { get; set; }

    public DocumentationController(string controller, string controllerRoute, List<DocumentationEndpoint> collection)
    {
        ControllerName = controller;
        ControllerRoute = controllerRoute;
        EndpointsCollection = collection;
    }

    public DocumentationController()
    {
        ControllerName = string.Empty;
        ControllerRoute = string.Empty;
        EndpointsCollection = [];
    }
}