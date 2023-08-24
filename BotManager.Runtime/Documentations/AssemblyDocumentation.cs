using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace BotManager.Runtime.Documentations;

/// <summary>
/// This class can read the .NET documentation .xml files and access the <see cref="Members"/> in runtime.
/// </summary>
[XmlRoot("doc")]
public class AssemblyDocumentation : IXmlSerializable
{
    /// <summary>
    /// Gets and sets the assembly name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets the list of all members in this assembly.
    /// </summary>
    public List<AssemblyMember> Members { get; } = new();

    /// <summary>
    /// Returns the member with the given type and name.
    /// </summary>
    /// <param name="type">The required type.</param>
    /// <param name="name">The required name.</param>
    /// <param name="member">The found member.</param>
    /// <returns>Returns <c>true</c> if a member was found.</returns>
    public bool TryGetMember(AssemblyMemberType type, string name, [MaybeNullWhen(false)] out AssemblyMember member)
    {
        foreach (var m in Members)
        {
            if (m.Type != type || m.Name != name) continue;
            member = m;
            return true;
        }

        member = default;
        return false;
    }
    
    #region Xml
    
    /// <inheritdoc />
    public XmlSchema? GetSchema()
    {
        return null;
    }

    /// <inheritdoc />
    public void ReadXml(XmlReader reader)
    {
        
        while (reader.Read())
        {
            switch (reader.Name)
            {
                case "assembly" when reader.NodeType == XmlNodeType.Element:
                    if (reader.MoveToContent() == XmlNodeType.None)
                        break;
                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.EndElement)
                            break;
                        
                        switch (reader.Name)
                        {
                            case "name" when reader.NodeType == XmlNodeType.Element:
                                Name = reader.ReadString();
                                break;
                            default:
                                reader.Skip();
                                break;
                        }
                    }

                    break;
                
                case "members" when reader.NodeType == XmlNodeType.Element:
                    if (reader.MoveToContent() == XmlNodeType.None)
                        break;
                    
                    var skipNextRead = false;
                    while (skipNextRead || reader.Read())
                    {
                        skipNextRead = false;
                        if (reader.NodeType == XmlNodeType.EndElement)
                            break;
                        
                        switch (reader.Name)
                        {
                            case "member" when reader.NodeType == XmlNodeType.Element:
                                var member = new AssemblyMember();
                                member.ReadXml(reader);
                                Members.Add(member);
                                
                                // ReadInnerXml reads past the end of the element, so we need to ignore the next
                                skipNextRead = true;
                                break;
                            default:
                                reader.Skip();
                                break;
                        }
                    }
                    break;
                default:
                    reader.Skip();
                    break;
            }
        }
    }

    /// <inheritdoc />
    public void WriteXml(XmlWriter writer)
    {
        // Not needed right now.
        throw new NotSupportedException();
    }
    
    #endregion Xml
    
    #region Static

    /// <summary>
    /// Reads an assembly documentation xml from a file.
    /// </summary>
    /// <param name="path">The file path to the xml.</param>
    /// <returns>Returns the documentation instance.</returns>
    public static AssemblyDocumentation Read(string path)
    {
        using var stream = new FileStream(path, FileMode.Open, FileAccess.Read);
        return Read(stream);
    }
    
    /// <summary>
    /// Reads an assembly documentation xml from a stream.
    /// </summary>
    /// <param name="stream">The stream to the xml.</param>
    /// <returns>Returns the documentation instance.</returns>
    public static AssemblyDocumentation Read(Stream stream)
    {
        using var reader = new StreamReader(stream);
        var serializer = new XmlSerializer(typeof(AssemblyDocumentation));
        return (AssemblyDocumentation)serializer.Deserialize(reader)!;
    }
    
    /// <summary>
    /// Returns the expected documentation xml path for the given assembly.
    /// It is not checked if this file does actually exists.
    /// </summary>
    /// <param name="assembly">The assembly.</param>
    /// <returns>The expected path.</returns>
    public static string GetPathByAssembly(Assembly assembly)
    {
        var assemblyPath = Path.GetDirectoryName(assembly.Location);
        var assemblyName = Path.GetFileNameWithoutExtension(assembly.Location);
        return Path.Combine(assemblyPath!, $"{assemblyName}.xml");
    }
    
    #endregion Static
}