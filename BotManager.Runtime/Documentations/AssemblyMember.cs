using System.Xml;

namespace BotManager.Runtime.Documentations;

/// <summary>
/// A single member of a <see cref="AssemblyDocumentation"/>.
/// </summary>
public sealed class AssemblyMember
{
    /// <summary>
    /// Gets the type of the member.
    /// </summary>
    public AssemblyMemberType Type { get; set; }

    /// <summary>
    /// Gets the full name of the member.
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Gets the xml content.
    /// </summary>
    public string XmlContent { get; set; }
    
    #region Xml
    
    /// <summary>
    /// Reads the member from the xml reader.
    /// </summary>
    /// <param name="reader"></param>
    public void ReadXml(XmlReader reader)
    {
        if (reader.MoveToFirstAttribute())
        {
            do
            {
                switch (reader.Name)
                {
                    case "name":
                    {
                        var name = reader.Value;
                    
                        // First char is the type:
                        Type = name[0] switch
                        {
                            'T' => AssemblyMemberType.Type,
                            'M' => AssemblyMemberType.Method,
                            'F' => AssemblyMemberType.Field,
                            'P' => AssemblyMemberType.Property,
                            _ => throw new Exception($"Unexpected member type '{name[0]}'.")
                        };
                        Name = name[2..];
                        break;
                    }
                }
            } while (reader.MoveToNextAttribute());
        }

        if (reader.MoveToContent() != XmlNodeType.None)
        {
            XmlContent = reader.ReadInnerXml();
        }
    }
    
    #endregion Xml
}