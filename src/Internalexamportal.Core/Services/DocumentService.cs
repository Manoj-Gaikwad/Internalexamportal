using Aspose.Words;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Internalexamportal.Core.FileSystem;
using Internalexamportal.Core.Models;
using Internalexamportal.Core.Configurations;

namespace Internalexamportal.Core.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly FileConfiguration _fileConfiguraiton;
        private readonly IFluentLocal _fluentLocal;

        public DocumentService(
            IOptions<FileConfiguration> fileConfiguration,
            IFluentLocal fluentLocal
        )
        {
            _fileConfiguraiton = fileConfiguration.Value;
            _fluentLocal = fluentLocal;
        }

        public JObject ToJObject<T>(T model) where T : class
        {
            //parse json string data to Jobject
            var jObject = JObject.Parse(
               model
               .GetType()
               .GetProperty("JsonFormData")
               .GetValue(model, null).ToString()
               );

            string[] accesibleProperty = new string[] { "JsonFormData"};

            //Add other model properties to JObject
            foreach (var property in model.GetType().GetProperties())
            {
                var propType = property.PropertyType.Name;
                var propertyName = property.Name;

                if (!accesibleProperty.Contains(propertyName))
                {
                    try
                    {
                        jObject.Add(
                            char.ToLowerInvariant(propertyName[0]) + propertyName.Substring(1), 
                            (property.GetValue(model, null) ?? string.Empty).ToString()
                        );
                    }
                    catch (Exception)
                    {

                    }
                }
            }
            return jObject;
        }

        public Stream GetTemplateStream(TemplateModel model, SaveFormat format, string filePath)
        {
            var data = model.JObject ?? ToJObject(model);

            Stream document = _fluentLocal
                .UseTemplate(filePath)
                .Fill(data, format)
                .Get();

            return document;
        }

        private string JsonFormDataToJObject(string jsonFormData) 
        {
            //Modify Json Form Data
            JObject jsonData 
                = (JObject)JsonConvert.DeserializeObject(jsonFormData);

            return jsonData.ToString(Formatting.None);
        }

        public string ToBase64String(Stream stream)
        {
            return ToBase64String((MemoryStream)stream);
        }

        public string ToBase64String(MemoryStream stream)
        {
            return Convert.ToBase64String(stream.ToArray());
        }

    }
}
