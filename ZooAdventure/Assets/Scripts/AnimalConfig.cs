using System;
public class AnimalConfig
{
    private string title;
    private string resourceName;

    public string Title
    {
        get { return title; }
    }
    public string ResourceName
    {
        get { return resourceName; }
    }

    public AnimalConfig(string inTitle, string inResourceName)
    {
        title = inTitle;
        resourceName = inResourceName;
    }
}
