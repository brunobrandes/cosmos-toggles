using Cosmos.Db.Sql.Api.Domain.Entities;
using System;

namespace Cosmos.Toggles.Domain.Entities
{
    public class Flag : Entity
    {
        public Flag() :
            base(false)
        {

        }

        public Environment Environment { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Enabled { get; set; }
        public DateTime Created { get; set; }
    }
}
