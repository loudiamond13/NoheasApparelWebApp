using NoheasApparel.DataAccess.Repository.Interfaces;
using NoheasApparel.DataAccessLibrary.DataAccess.Repository.Interfaces;
using NoheasApparel.Models;
using NoheasApparel.Models.Models;

namespace NoheasApparel.DataAccess.Repository
{
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        private readonly NoheasApparelContext _contextDB;
        public OrderHeaderRepository(NoheasApparelContext ctx)
                : base(ctx) => _contextDB = ctx;

        public void Update(OrderHeader OrderHeader) 
        {
            _contextDB.OrderHeaders.Update(OrderHeader);
        }

     

        

    }





}
