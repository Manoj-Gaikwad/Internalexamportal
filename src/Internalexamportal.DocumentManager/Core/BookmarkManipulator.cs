namespace Internalexamportal.DocumentManager.Core
{
    public class BookmarkManipulator
    {
        private DocumentManipulator DocumentManipulator { get; }
        public string Key { get; private set; }
        public string Value { get; private set; }
        public BookmarkContentType ContentType { get; private set; }
        public void SetKey(string key) 
        {
            Key = key;
        }

        public BookmarkManipulator(DocumentManipulator documentManipulator)
        {
            DocumentManipulator = documentManipulator;
        }

        public DocumentManipulator WithText(string bookmarkText)
        {
            ContentType = BookmarkContentType.Text;
            Value = bookmarkText; 
            return DocumentManipulator;
        }

        public DocumentManipulator WithImage(string bookmarkImagePath)
        {
            ContentType = BookmarkContentType.Image;
            Value = bookmarkImagePath;
            return DocumentManipulator;
        }
    }

    public enum BookmarkContentType
    {
        Text = 0,
        Image = 1
    }
}