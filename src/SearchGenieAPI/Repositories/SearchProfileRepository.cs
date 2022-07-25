using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using SearchGenieAPI.Entities;

namespace SearchGenieAPI.Repositories
{


    public class SearchProfileRepository : ISearchProfileRepository
    {
        private readonly IDynamoDBContext context;
        private readonly ILogger<SearchProfileRepository> logger;

        public SearchProfileRepository(IDynamoDBContext context, ILogger<SearchProfileRepository> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public async Task<bool> CreateAsync(SearchProfile searchProfile)
        {
            try
            {
                searchProfile.Id = Guid.NewGuid();
                await context.SaveAsync(searchProfile);
                logger.LogInformation("searchProfile {} is added", searchProfile.Id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "fail to persist to DynamoDb Table");
                return false;
            }

            return true;
        }

        public async Task<bool> DeleteAsync(SearchProfile searchProfile)
        {
            bool result;
            try
            {
                // Delete the searchProfile.
                await context.DeleteAsync<SearchProfile>(searchProfile.Id);
                // Try to retrieve deleted searchProfile. It should return null.
                SearchProfile deletedSearchProfile = await context.LoadAsync<SearchProfile>(searchProfile.Id, new DynamoDBContextConfig
                {
                    ConsistentRead = true
                });

                result = deletedSearchProfile == null;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "fail to delete searchProfile from DynamoDb Table");
                result = false;
            }

            if (result) logger.LogInformation("SearchProfile {Id} is deleted", searchProfile);

            return result;
        }

        public async Task<bool> UpdateAsync(SearchProfile searchProfile)
        {
            if (searchProfile == null) return false;

            try
            {
                await context.SaveAsync(searchProfile);
                logger.LogInformation("SearchProfile {Id} is updated", searchProfile);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "fail to update searchProfile from DynamoDb Table");
                return false;
            }

            return true;
        }

        public async Task<SearchProfile?> GetByIdAsync(Guid id)
        {
            try
            {
                return await context.LoadAsync<SearchProfile>(id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "fail to update searchProfile from DynamoDb Table");
                return null;
            }
        }

        public async Task<IList<SearchProfile>> GetSearchProfilesAsync(int limit = 10)
        {
            var result = new List<SearchProfile>();

            try
            {
                if (limit <= 0)
                {
                    return result;
                }

                var filter = new ScanFilter();
                filter.AddCondition("Id", ScanOperator.IsNotNull);
                var scanConfig = new ScanOperationConfig()
                {
                    Limit = limit,
                    Filter = filter
                };
                var queryResult = context.FromScanAsync<SearchProfile>(scanConfig);

                do
                {
                    result.AddRange(await queryResult.GetNextSetAsync());
                }
                while (!queryResult.IsDone && result.Count < limit);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "fail to list searchProfiles from DynamoDb Table");
                return new List<SearchProfile>();
            }

            return result;
        }
    }
}
