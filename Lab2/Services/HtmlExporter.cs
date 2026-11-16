using System.Globalization;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;
using Lab2.Models;

namespace Lab2.Services;

public class HtmlExporter
{
    public void Export(IEnumerable<Scientist> scientists, XmlReader xsl, Stream output)
    {
        XDocument doc = BuildXml(scientists);

        XslCompiledTransform xslt = new();
        xslt.Load(xsl);

        using XmlWriter writer = XmlWriter.Create(output, xslt.OutputSettings);
        xslt.Transform(doc.CreateReader(), writer);
    }

    private static XDocument BuildXml(IEnumerable<Scientist> scientists)
    {
        CultureInfo inv = CultureInfo.InvariantCulture;

        return new XDocument(
            new XElement("scientists",
                scientists.Select(s => new XElement("scientist",
                    new XAttribute("faculty", s.Faculty),
                    new XAttribute("department", s.Department),
                    new XAttribute("position", s.Position),
                    new XAttribute("salary", s.Salary.ToString(inv)),
                    new XAttribute("hireDate", s.HireDate.ToString("yyyy-MM-dd", inv)),
                    s.FireDate is { } fd
                        ? new XAttribute("fireDate", fd.ToString("yyyy-MM-dd", inv))
                        : null,
                    new XElement("fullName", s.FullName)))));
    }
}