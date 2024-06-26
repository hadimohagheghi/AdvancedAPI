﻿using System.Threading;
using System.Threading.Tasks;
using Entities;

namespace Data.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByUserAndPass(string username, string password, CancellationToken cancellationToken);

    }
}