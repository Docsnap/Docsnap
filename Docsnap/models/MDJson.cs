using Docsnap.interfaces;

namespace Docsnap.Models;

internal class MDJson : IMDJson
{
    public string Controller { get; set; }
    public List<string> ContentController { get; set; }

    public MDJson(string controller, List<string> contentController)
    {
        Controller = controller;
        ContentController = contentController;
    }

    public MDJson()
    {
        Controller = "";
        ContentController = [];
    }
}

internal class ListMDJson
{
    public List<MDJson> MDJsonList { get; set; }

    public ListMDJson(List<MDJson> list)
    {
        MDJsonList = list;
    }

    public ListMDJson()
    {
        MDJsonList = [];
    }
}