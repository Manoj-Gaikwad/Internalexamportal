using System.Xml.Linq;

namespace Internalexamportal.DocumentManager.Core
{
    public interface IXmlMapper<in T>
    {
        XDocument MapToXDocument(T source);
    }
}