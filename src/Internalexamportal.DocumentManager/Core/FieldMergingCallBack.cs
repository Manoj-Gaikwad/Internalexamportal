using System;
using System.IO;
using Aspose.Words;
using Aspose.Words.Drawing;
using Aspose.Words.MailMerging;

namespace Internalexamportal.DocumentManager.Core
{
    public class FieldMergingCallBack : IFieldMergingCallback
    {
        public void FieldMerging(FieldMergingArgs args)
        {
            DocumentBuilder builder = new DocumentBuilder(args.Document);

            if (args.FieldName.EndsWith("_html"))
            {
                builder.MoveToMergeField(args.DocumentFieldName);

                if (args.FieldValue != null)
                {
                    string fieldVlaueString = args.FieldValue.ToString();

                    builder.InsertHtml(fieldVlaueString);
                }
            }

            if (args.FieldName.EndsWith("_chckbx"))
            {
                builder.MoveToMergeField(args.DocumentFieldName);
                if (args.FieldValue != null)
                {
                    builder.Write("");
                    if (Convert.ToBoolean(args.FieldValue) == true)
                        builder.Write("X");
                }
            }

            if (args.FieldName.EndsWith("_radio"))
            {
                builder.MoveToMergeField(args.DocumentFieldName);
                if (args.FieldValue != null)
                {
                    //if (args.FieldValue.ToString().Contains("label"))
                    //{
                    //	builder.Write("");
                    //}
                    builder.Write("");
                    if (!string.IsNullOrEmpty(args.FieldValue.ToString()) && Convert.ToBoolean(args.FieldValue) == false)
                        builder.Write("X");
                }
            }

            if (args.FieldName.EndsWith("_watermark") 
                || args.FieldName.Contains("userSignature"))
            {
                builder.MoveToMergeField(args.DocumentFieldName);
                if (args.FieldValue != null)
                {
                    var fieldVlaueByte = (byte[])Convert.FromBase64String(args.FieldValue.ToString());
                    var shape = builder.InsertImage(fieldVlaueByte);
                    shape.WrapType = WrapType.None;
                    shape.BehindText = true;
                }
            }

            if (args.FieldName.Contains("reviewer_title"))
            {
                builder.MoveToMergeField(args.DocumentFieldName);
                if (args.FieldValue != null)
                {
                    string field = args.FieldValue.ToString();
                    builder.Write(field);
                }
            }

            if (args.FieldName.Contains("oday"))
            {
                builder.MoveToMergeField(args.DocumentFieldName);
                if (args.FieldValue != null)
                {
                    string field = args.FieldValue.ToString();
                    builder.Write(field);
                }
            }

            if (args.FieldName.Contains("omonth"))
            {
                builder.MoveToMergeField(args.DocumentFieldName);
                if (args.FieldValue != null)
                {
                    string field = args.FieldValue.ToString();
                    builder.Write(field);
                }
            }

            if (args.FieldName.Contains("oyear"))
            {
                builder.MoveToMergeField(args.DocumentFieldName);
                if (args.FieldValue != null)
                {
                    string field = args.FieldValue.ToString();
                    builder.Write(field);
                }
            }

            if (args.FieldName.Contains("otime"))
            {
                builder.MoveToMergeField(args.DocumentFieldName);
                if (args.FieldValue != null)
                {
                    string field = args.FieldValue.ToString();
                    builder.Write(field);
                }
            }
        }

        public void ImageFieldMerging(ImageFieldMergingArgs args)
        {
            if (args.FieldValue != null)
            {
                MemoryStream imageStream = new MemoryStream(
                    (byte[])Convert.FromBase64String(args.FieldValue.ToString()));

                // Now the mail merge engine will retrieve the image from the stream.
                args.ImageStream = imageStream;
            }
        }
    }
}
