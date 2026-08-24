using Microsoft.EntityFrameworkCore;

namespace InternalExamportal.DataAccessLayer
{
    public interface IEntityConfigurator
    {
        void Apply(ModelBuilder modelBuilder);
    }
}