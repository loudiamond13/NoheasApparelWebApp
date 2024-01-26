using NoheasApparel.Models;

namespace NoheasApparel.DataAccess.Repository.Interfaces
{
    public interface IGenderRepository : IRepository<Gender>
    {

        void Update(Gender gender);
        
    }
}
