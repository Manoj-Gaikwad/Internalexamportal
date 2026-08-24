using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace InternalExamportal.DataAccessLayer
{
    public class EntityConfigurator : IEntityConfigurator
    {
        private readonly IEnumerable<object> _collection;

        public EntityConfigurator(IEnumerable<object> collection)
        {
            _collection = collection;
        }
        public void Apply(ModelBuilder modelBuilder)
        {
            foreach (var configuration in _collection)
            {
                // Get Configuration interface in the assembly. Eg:  IEntityConfiguration<ApplicationError>
                var configInterface = configuration
                    .GetType()
                    .GetInterfaces()
                    .First(i => i.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>));

                //Get the argument from the interface. Eg: ApplicationError
                Type genericType = configInterface.GetGenericArguments()[0];

                //Find  ApplyConfiguration Method. 
                var method = typeof(ModelBuilder)
                    .GetMethods()
                    .Where(m => m.Name == "ApplyConfiguration")
                    .Select(m => new
                    {
                        Method = m,
                        Params = m.GetParameters(),
                        Args = m.GetGenericArguments()
                    })
                    .Where(x => x.Params.Length == 1
                                && x.Args.Length == 1
                                && x.Params[0].ParameterType == typeof(IEntityTypeConfiguration<>).MakeGenericType(x.Args[0]))
                    .Select(x => x.Method)
                    .First().MakeGenericMethod(genericType);

                //Call apply configuration method. 
                method.Invoke(modelBuilder, new[] { configuration });
            }
        }
    }
}
