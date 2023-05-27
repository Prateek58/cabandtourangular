using Contracts;
using HillYatraAPI.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository
{
    public class UserRepository:RepositoryBase<Shedule>, IUserRepository 
    {

        public UserRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }
    }
}
