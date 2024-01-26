using NoheasApparel.DataAccess.Repository.Interfaces;
using NoheasApparel.DataAccessLibrary.DataAccess.Repository.Interfaces;
using NoheasApparel.Models;
using NoheasApparel.Models.Models;

namespace NoheasApparel.DataAccess.Repository
{
    public class OrderDetailRepository : Repository<OrderDetail>, IOrderDetailRepository
    {
        private readonly NoheasApparelContext _contextDB;
        public OrderDetailRepository(NoheasApparelContext ctx)
                : base(ctx) => _contextDB = ctx;

        public void Update(OrderDetail OrderDetail) 
        {
            _contextDB.OrderDetails.Update(OrderDetail);
        }

     

        

    }





}
