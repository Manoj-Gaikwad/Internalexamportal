namespace Internalexamportal.DocumentManager.Core
{
    public class FieldManipulator
    {
        private DocumentManipulator DocumentManipulator { get; }
        public string Key { get; private set; }
        public object Value { get; private set; }
        public void SetKey(string key)
        {
            Key = key;
        }

        public FieldManipulator(DocumentManipulator documentManipulator)
        {
            DocumentManipulator = documentManipulator;
        }

        internal FieldManipulator(
            DocumentManipulator documentManipulator, 
            string key, 
            object value)
        {
            DocumentManipulator = documentManipulator;
            Key = key;
            Value = value;
        }

        public DocumentManipulator WithValue(object fieldValue)
        {
            Value = fieldValue;
            return DocumentManipulator;
        }
    }
}