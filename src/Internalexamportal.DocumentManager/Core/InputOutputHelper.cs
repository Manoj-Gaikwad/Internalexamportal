using System.Data;
using System.IO;
using Aspose.Words;
using Aspose.Words.MailMerging;
using Aspose.Words.Saving;

namespace Internalexamportal.DocumentManager.Core
{
    public class InputOutputHelper
    {
        private Document Document { get; }

        public InputOutputHelper(Document document)
        {
            Document = document;
        }

        public void Save(string outputPath)
        {
            Document.Save(outputPath);
        }

        public MemoryStream SaveToStream(MemoryStream stream, SaveFormat saveFormat)
        {
            Document.Save(stream, saveFormat);
            return stream;
        }

        public MemoryStream SaveToStream(SaveFormat saveFormat)
        {
            var stream = new MemoryStream();
            Document.Save(stream, saveFormat);
            return stream;
        }

        public string GetDocumentAsHtmlString()
        {
            {
                using (var memoryStream = new MemoryStream())
                {
                    Document.Save(memoryStream, new HtmlFixedSaveOptions()
                    {
                        ExportFormFields = true,
                        ExportEmbeddedSvg = true,
                        ExportEmbeddedCss = true,
                        ExportEmbeddedFonts = true,
                        ExportEmbeddedImages = true
                    });

                    using (var streamReader = new StreamReader(memoryStream))
                    {
                        memoryStream.Position = 0;
                        return streamReader.ReadToEnd();
                    }
                }
            }
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

            return this;
        }
    }
}