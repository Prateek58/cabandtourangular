using Contracts;

using HillYatraAPI.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository
{
    public class SheduleRepository:RepositoryBase<Shedule>, ISheduleRepository 
    {

        public SheduleRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }
    }
}
