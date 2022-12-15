using System.Drawing;

namespace DocumentManager.Model;

public class DocumentTag
{
    public String Name { get; set; }
    public String Description { get; set; }
    public Color Color { get; set; }

    public DocumentTag(string name, string description, Color color)
    {
        Name = name;
        Description = description;
        Color = color;
    }
#pragma warning disable CS8618
    protected  DocumentTag(){}
#pragma warning restore CS8618

    public DocumentTag CreateTag(string name, string desc, Color color)
    {
        return new DocumentTag(name, desc, color);
    }
    
    //update
    //delete
}