using SearchGenieAPI.Entities;

namespace SearchGenieAPI.Repositories
{
    /// <summary>
    /// Sample DynamoDB Table SearchProfile CRUD
    /// </summary>
    public interface ISearchProfileRepository
    {
        /// <summary>
        /// Include new searchProfile to the DynamoDB Table
        /// </summary>
        /// <param name="searchProfile">searchProfile to include</param>
        /// <returns>success/failure</returns>
        Task<bool> CreateAsync(SearchProfile searchProfile);

        /// <summary>
        /// Remove existing searchProfile from DynamoDB Table
        /// </summary>
        /// <param name="searchProfile">searchProfile to remove</param>
        /// <returns></returns>
        Task<bool> DeleteAsync(SearchProfile searchProfile);

        /// <summary>
        /// List searchProfile from DynamoDb Table with items limit (default=10)
        /// </summary>
        /// <param name="limit">limit (default=10)</param>
        /// <returns>Collection of Search profiles</returns>
        Task<IList<SearchProfile>> GetSearchProfilesAsync(int limit = 10);

        /// <summary>
        /// Get searchProfile by PK
        /// </summary>
        /// <param name="id">searchProfile`s PK</param>
        /// <returns>SearchProfile object</returns>
        Task<SearchProfile?> GetByIdAsync(Guid id);

        /// <summary>
        /// Update searchProfile content
        /// </summary>
        /// <param name="searchProfile">searchProfile to be updated</param>
        /// <returns></returns>
        Task<bool> UpdateAsync(SearchProfile searchProfile);
    }
}