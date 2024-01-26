using NoheasApparel.DataAccess.Repository.Interfaces;
using NoheasApparel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoheasApparel.DataAccessLibrary.DataAccess.Repository.Interfaces
{
    public interface INoheasApparelUserRepository : IRepository<NoheasApparelUser>
    {
        void Update(NoheasApparelUser user);
    }
}
