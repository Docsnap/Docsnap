using Docsnap.interfaces;

namespace Docsnap.Models;

public class MDJson : IMDJson
{
    public string TitleMD { get; set; }
    public List<string> BodyMD { get; set; }

    public MDJson(string title, List<string> body)
    {
        TitleMD = title;
        BodyMD = body;
    }

    public MDJson()
    {
        TitleMD = "";
        BodyMD = [];
    }
}

public class ListMDJson
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