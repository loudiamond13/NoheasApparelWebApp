using NoheasApparel.Models;

namespace NoheasApparel.DataAccess.Repository.Interfaces
{
    public interface IBrandRepository : IRepository<Brand>
    {

        void Update(Brand brand);
        
    }
}
