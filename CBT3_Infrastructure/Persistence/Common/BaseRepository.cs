namespace CBT_Infrastructure.Common;


public abstract class BaseRepository<TEntity, TModel> : IBaseRepository<TEntity, TModel>
 where TEntity : class
 where TModel : class
{
    private ILogger<TEntity> _logger;
    private UserDetails _userDetails;
    private IConfiguration _configuration;
    private string _connectionstring;
    private string _logheader;
    public string ConnectionString => _connectionstring;
    public string LogHeader => _logheader;
    public ILogger<TEntity> Logger => _logger;
    public UserDetails UserDetails => _userDetails;
    public IConfiguration Configuration => _configuration;

    public BaseRepository(ILogger<TEntity> logger, UserDetails userDetails, IConfiguration configuration)
    {
        _logger = logger;
        _userDetails = userDetails;
        _configuration = configuration;


        _connectionstring = _configuration.GetConnectionString("DefaultConnectionString");
        _logheader = LogSupport.LogEntryHeader(_userDetails);
    }

    //public abstract Task<List<TModel>> GetAllAsync();
    //public abstract Task<TModel> GetByIdAsync(string id);
    //public abstract Task<TModel> AddAsync(TModel model);
    //public abstract Task<bool> UpdateAsync(TModel model);
    //public abstract Task<bool> DeleteAsync(TModel model);
}
