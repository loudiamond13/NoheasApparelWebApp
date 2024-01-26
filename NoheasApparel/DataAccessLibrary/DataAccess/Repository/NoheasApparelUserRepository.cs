using NoheasApparel.DataAccess;
using NoheasApparel.DataAccess.Repository;
using NoheasApparel.DataAccessLibrary.DataAccess.Repository.Interfaces;
using NoheasApparel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoheasApparel.DataAccessLibrary.DataAccess.Repository
{
    public class NoheasApparelUserRepository : Repository<NoheasApparelUser>, INoheasApparelUserRepository
    {
        private readonly NoheasApparelContext _contextDB;
        public NoheasApparelUserRepository(NoheasApparelContext x) 
            : base(x) => _contextDB = x;

        public void Update(NoheasApparelUser user)
        { 
            _contextDB.NoheasApparelUsers.Update(user);
        }
    }
}
