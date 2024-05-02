﻿
namespace CBT_Infrastructure.Common;

public interface IBaseRepository<TEntity, TModel>
where TEntity : class
where TModel : class
{
    public ILogger<TEntity> Logger { get; }
    public UserDetails UserDetails { get; }
    public IConfiguration Configuration { get; }

    public string ConnectionString { get; }
    public string LogHeader { get; }

    //Task<List<TModel>> GetAllAsync();
    //Task<TModel> GetByIdAsync(string id);
    //Task<TModel> AddAsync(TModel model);
    //Task<bool> UpdateAsync(TModel model);
    //Task<bool> DeleteAsync(TModel model);
}
