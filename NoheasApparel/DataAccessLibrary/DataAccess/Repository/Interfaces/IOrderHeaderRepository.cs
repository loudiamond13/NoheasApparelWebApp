﻿using NoheasApparel.DataAccess.Repository.Interfaces;
using NoheasApparel.Models.Models;

namespace NoheasApparel.DataAccessLibrary.DataAccess.Repository.Interfaces
{
    public interface IOrderHeaderRepository : IRepository<OrderHeader>
    {
        void Update(OrderHeader orderHeader);
    }
}