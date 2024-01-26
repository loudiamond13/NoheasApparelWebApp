using NoheasApparel.DataAccess.Repository.Interfaces;
using NoheasApparel.Models;

namespace NoheasApparel.DataAccess.Repository
{
    public class GenderRepository : Repository<Gender>, IGenderRepository
    {
        private readonly NoheasApparelContext _contextDB;
        public GenderRepository(NoheasApparelContext ctx) 
                : base(ctx) => _contextDB = ctx;

        public void Update(Gender gender)
        {
            _contextDB.Genders.Update(gender);
        }

   
    }
}
