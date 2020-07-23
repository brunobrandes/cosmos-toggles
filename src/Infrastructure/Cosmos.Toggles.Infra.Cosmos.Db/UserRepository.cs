using Azure.Cosmos;
using Cosmos.Db.Sql.Api.Infra.Entities.Repositories;
using Cosmos.Toggles.Domain.Entities;
using Cosmos.Toggles.Domain.Entities.Repositories;
using Cosmos.Toggles.Domain.Service.Extensions;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Cosmos.Toggles.Infra.Cosmos.Db
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly CosmosContainer _userContainer;

        public UserRepository(CosmosClient cosmosClient)
          : base(cosmosClient)
        {
            _userContainer = cosmosClient.GetContainer(DatabaseId, ContainerId);
        }

        public override string DatabaseId => "CosmosToggles";

        public override string ContainerId => "Users";

        public async Task<User> GetByEmailAsync(string email)
        {
            var queryDefinition = new QueryDefinition("SELECT * FROM Users C WHERE C.Email = @email")
              .WithParameter("@email", email);

            var pageable = _userContainer.GetItemQueryIterator<User>(queryDefinition, null);

            await foreach (var page in pageable?.AsPages())
            {
                return page?.Values.FirstOrDefault();
            }

            return null;
        }

        public async Task<User> GetByEmailPasswordAsync(string email, string password)
        {
            var queryDefinition = new QueryDefinition("SELECT * FROM Users C WHERE C.Email = @email AND C.Password = @password")
              .WithParameter("@email", email)
              .WithParameter("@password", password);

            try
            {
                var pageable = _userContainer.GetItemQueryIterator<User>(queryDefinition, null);

                await foreach (var page in pageable?.AsPages())
                {
                    return page?.Values.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                ex.IgnoreCosmosExceptionStatus(HttpStatusCode.NotFound);
            }


            return null;
        }
    }
}