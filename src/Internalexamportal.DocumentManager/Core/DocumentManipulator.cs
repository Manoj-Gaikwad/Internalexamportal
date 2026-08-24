using Aspose.Words;
using Aspose.Words.MailMerging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml.Linq;

namespace Internalexamportal.DocumentManager.Core
{
    public class DocumentManipulator
    {
        public Document Document { get; }
        private List<FieldManipulator> FieldManipulators { get; }
        private List<BookmarkManipulator> BookmarkManipulators { get; }
        private List<XDocument> DataSources { get; }
        private DataSet DataSetForJArray { get; set; }


        public DocumentManipulator(Document document)
        {
            Document = document;
            FieldManipulators = new List<FieldManipulator>();
            BookmarkManipulators = new List<BookmarkManipulator>();
            DataSources = new List<XDocument>();
            DataSetForJArray = new DataSet();
        }

        public DocumentManipulator Inject(object obj)
        {
            var properties = obj.GetType().GetProperties().ToList();

            properties.ForEach(property =>
            {
                FieldManipulators.Add(
                    new FieldManipulator(this, property.Name, property.GetValue(obj, null)));
            });

            return this;
        }

        public DocumentManipulator InjectJObject(JObject jObj, string prefix = "")
        {
            foreach (var kvp in jObj)
            {
                var formattedKey = GetFormattedKey(prefix, kvp.Key);

                //If we have nested object
                if (kvp.Value.GetType() == typeof(JObject))
                {
                    InjectJObject((JObject)kvp.Value, formattedKey);
                }
                //if we have nested array
                else if (kvp.Value.GetType() == typeof(JArray))
                {
                    JArray jArray = (JArray)kvp.Value;

                    CreateDataSetFromJArray(jArray, formattedKey, DataSetForJArray);
                }
                //default condition
                else
                {
                    FieldManipulators.Add(
                     new FieldManipulator(this, formattedKey, kvp.Value));
                }
            }
            return this;
        }

        public static string GetFormattedKey(string prefix, string key)
        {
            return (string.IsNullOrEmpty(prefix)) ? key : prefix + "." + key;
        }

        public static void CreateDataSetFromJArray(JArray jArray, string tableName
            , DataSet DataSetForJArray, string foreignKey= "", string foreignTableName = "")
        {
            DataTable dataTable;

            if (DataSetForJArray.Tables.Contains(tableName))
            {
                dataTable = DataSetForJArray.Tables[tableName];
            }
            else
            {
                dataTable = new DataTable
                {
                    TableName = tableName
                };
                DataSetForJArray.Tables.Add(dataTable);

                string primaryKey = tableName+"Id";

                dataTable.Columns.Add(primaryKey, typeof(string));

                dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns[primaryKey] };
            }

            if (!string.IsNullOrEmpty(foreignTableName) && !string.IsNullOrEmpty(foreignKey))
            {
                //maintain relation
                if (!dataTable.Columns.Contains(foreignTableName + "Id"))
                {
                    dataTable.Columns.Add(foreignTableName + "Id", typeof(string));

                    DataSetForJArray.Relations.Add(
                        foreignTableName + "." + tableName, 
                        DataSetForJArray.Tables[foreignTableName].Columns[foreignTableName + "Id"], 
                        DataSetForJArray.Tables[tableName].Columns[foreignTableName + "Id"]);
                }
              
            }
            foreach (var item in jArray)
            {
                var row = dataTable.NewRow();

                var primaryKey = Guid.NewGuid().ToString();

                row[tableName + "Id"] = primaryKey;

                dataTable.Rows.Add(row);


                foreach (var property in item.ToObject<JObject>())
                {

                    if (!string.IsNullOrEmpty(foreignTableName) && !string.IsNullOrEmpty(foreignKey))
                    {
                        row[foreignTableName + "Id"] = foreignKey;
                    }

                    var value = property.Value;

                    if (value.GetType() == typeof(JArray) || value.GetType() == typeof(JObject))
                    {
                        var JarrayValue = new JArray();
                        if (value.GetType() == typeof(JObject))
                            JarrayValue.Add(value);
                        else
                            JarrayValue = (JArray)value;

                        CreateDataSetFromJArray(JarrayValue,
                            property.Key, DataSetForJArray, primaryKey, tableName);
                    }
                    else
                    {
                        if (!dataTable.Columns.Contains(property.Key))
                        {
                            dataTable.Columns.Add(property.Key, typeof(string));
                        }
                        row[property.Key] = value;
                    }
                }
            }
        }

        public FieldManipulator InjectField(string fieldKey)
        {
            var newFieldManipulator = new FieldManipulator(this);
            newFieldManipulator.SetKey(fieldKey);
            FieldManipulators.Add(newFieldManipulator);
            return newFieldManipulator;
        }

        public BookmarkManipulator InjectBookmark(string bookmarkdKey)
        {
            var newBookmarkManipulator = new BookmarkManipulator(this);
            newBookmarkManipulator.SetKey(bookmarkdKey);
            BookmarkManipulators.Add(newBookmarkManipulator);
            return newBookmarkManipulator;
        }

        public DataSourceManipulator<T> AssociateDataSource<T>(T dataSourceObject)
        {
            var newDataSourceManipulator = new DataSourceManipulator<T>(this);
            newDataSourceManipulator.SetDataSourceObject(dataSourceObject);
            newDataSourceManipulator.SetDataSource(DataSources);
            return newDataSourceManipulator;
        }

        public InputOutputHelper Render() 
        {
            Document.MailMerge.UseNonMergeFields = true;
            RenderFields();
            RenderFieldsWithDataSources();
            RenderFieldsForJArray();
            RenderBookmarks();
            return new InputOutputHelper(Document);
        }

        public InputOutputHelper Render(IFieldMergingCallback customMailMergeCallBack)
        {
            Document.MailMerge.FieldMergingCallback = customMailMergeCallBack;
            return Render();
        }

        public InputOutputHelper Render<T>() where T : IFieldMergingCallback, new()
        {
            var customMailMergeCallBack = new T(); // no reflection, no runtime errors
            Document.MailMerge.FieldMergingCallback = customMailMergeCallBack;
            return Render();
        }

        private void RenderFieldsWithDataSources()
        {
            if (DataSources == null || !DataSources.Any()) return;

            Document.MailMerge.UseNonMergeFields = true;
            foreach (var dataSource in DataSources)
            {
                var dataSet = new DataSet();
                dataSet.ReadXml(dataSource.CreateReader());
                Document.MailMerge.ExecuteWithRegions(dataSet);
            }
        }

        private void RenderFieldsForJArray()
        {
            if (DataSetForJArray == null) return;

            Document.MailMerge.ExecuteWithRegions(DataSetForJArray);
        }

        private void RenderBookmarks()
        {
            if (BookmarkManipulators == null || !BookmarkManipulators.Any()) return;

            var documentBuilder = new DocumentBuilder(Document);
            foreach (var bookmarkManipulator in BookmarkManipulators)
            {
                if (Document.Range.Bookmarks[bookmarkManipulator.Key] == null) continue;
                documentBuilder.MoveToBookmark(bookmarkManipulator.Key);
                switch (bookmarkManipulator.ContentType)
                {
                    case BookmarkContentType.Text:
                        documentBuilder.InsertHtml(bookmarkManipulator.Value);
                        break;
                    case BookmarkContentType.Image:
                        documentBuilder.InsertImage(bookmarkManipulator.Value);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void RenderFields()
        {
            if (FieldManipulators == null || !FieldManipulators.Any()) return;

            Document.MailMerge.Execute(
                FieldManipulators.Select(m => m.Key).ToArray(),
                FieldManipulators.Select(m => m.Value).ToArray());
        }

        public InputOutputHelper CleanUp()
        {
            Document.MailMerge.CleanupOptions = MailMergeCleanupOptions.RemoveEmptyParagraphs
                                                | MailMergeCleanupOptions.RemoveUnusedRegions
                                                | MailMergeCleanupOptions.RemoveUnusedFields
                                                | MailMergeCleanupOptions.RemoveContainingFields
                                                | MailMergeCleanupOptions.RemoveStaticFields;

            Document.MailMerge.Execute(new string[] { }, new object[] { });

            Document.MailMerge.ExecuteWithRegions(new DataSet());

            return new InputOutputHelper(Document);
        }
    }

    
}
