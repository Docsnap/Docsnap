namespace Docsnap.interfaces;

internal interface IDocumentationEndpoint
{
    public string EndpointName { get; set; }
    public string EndpointMethod { get; set; }
    public string EndpointRoute { get; set; }
    List<string> ContentEndpoint { get; set; }
}