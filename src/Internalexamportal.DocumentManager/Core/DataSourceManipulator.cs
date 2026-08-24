using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Internalexamportal.DocumentManager.Core
{
    public class DataSourceManipulator<T>
    {
        private DocumentManipulator DocumentManipulator { get; }
        private T DataSource { get; set; }
        private List<XDocument> DataSourceList { get; set; }

        public void SetDataSourceObject(T dataSource)
        {
            DataSource = dataSource;
        }

        public void SetDataSource(List<XDocument> dataSourceList)
        {
            DataSourceList = dataSourceList;
        }

        public DataSourceManipulator(DocumentManipulator documentManipulator)
        {
            DocumentManipulator = documentManipulator;
        }

        public DocumentManipulator WithMapper(IXmlMapper<T> mapper)
        {
            DataSourceList.Add(mapper.MapToXDocument(DataSource));
            return DocumentManipulator;
        }

        public DocumentManipulator WithMapper<TMapper>()
     where TMapper : IXmlMapper<T>, new()
        {
            var mapper = new TMapper(); // no Activator needed
            DataSourceList.Add(mapper.MapToXDocument(DataSource));
            return DocumentManipulator;
        }
    }
}