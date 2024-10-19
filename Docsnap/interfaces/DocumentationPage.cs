namespace Docsnap.interfaces;

internal interface IDocumentationEndpoint
{
    public string Endpoint { get; set; }
    List<string> ContentEndpoint { get; set; }
}