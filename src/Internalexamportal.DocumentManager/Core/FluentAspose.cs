using System.IO;
using Aspose.Words;

namespace Internalexamportal.DocumentManager.Core
{
	public static class FluentAspose
	{
		public static DocumentManipulator TakeDocument(string documentPath)
		{
			return new DocumentManipulator(new Document(documentPath));
		}

		public static DocumentManipulator TakeDocument(Stream documentStream)
		{
			return new DocumentManipulator(new Document(documentStream));
		}

		public static void InitializeLicense(string licensePath)
		{
			if (!string.IsNullOrEmpty(licensePath))
			{
                new License().SetLicense(licensePath);
            }
		}
	}
}