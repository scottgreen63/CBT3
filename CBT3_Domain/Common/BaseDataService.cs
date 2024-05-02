
using CBT3_Shared.Common;

namespace CBT3_Domain.Common;

public abstract class BaseDataService<TEntity>
where TEntity : class


{
    // Implement the interface methods here

    private ILogger<TEntity> _logger;
    private UserDetails _userDetails;
    private IConfiguration _configuration;

    protected ILogger<TEntity> Logger => _logger;
    protected UserDetails UserDetails => _userDetails;
    protected IConfiguration Configuration => _configuration;
    protected string LogHeader => LogSupport.LogEntryHeader(_userDetails);

    public BaseDataService(ILogger<TEntity> logger, UserDetails userDetails, IConfiguration configuration)
    {
        this._logger = logger;
        this._userDetails = userDetails;
        this._configuration = configuration;

    }
    //public abstract Task<List<TModel>> GetAllAsync();
    //public abstract Task<TModel> GetByIdAsync(object id);
    //public abstract Task<TModel> CreateAsync(TModel model);
    //public abstract Task<TModel> UpdateAsync(object id, TModel model);
    //public abstract Task<bool> DeleteAsync(object id);


}
