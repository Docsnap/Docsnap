using Docsnap.interfaces;

namespace Docsnap.Models;

internal class DocumentationEndpoint : IDocumentationEndpoint
{
    public string Endpoint { get; set; }
    public List<string> ContentEndpoint { get; set; }

    public DocumentationEndpoint(string endpoint, List<string> contentEndpoint)
    {
        Endpoint = endpoint;
        ContentEndpoint = contentEndpoint;
    }

    public DocumentationEndpoint()
    {
        Endpoint = string.Empty;
        ContentEndpoint = [];
    }
}

internal class DocumentationAPI
{
    public string Controller { get; set; }
    public List<DocumentationEndpoint> MDJsonList { get; set; }

    public DocumentationAPI(string controller, List<DocumentationEndpoint> list)
    {
        Controller = controller;
        MDJsonList = list;
    }

    public DocumentationAPI()
    {
        Controller = string.Empty;
        MDJsonList = [];
    }
}