namespace PokerPlanningWebApi.Intefaces;

public interface IBaseRepository<TEntity> where TEntity : class
{
    Task<TEntity?> GetById(string id);
    Task<List<TEntity>> GetAll();
    Task<bool> DoesExist(string id);
    Task AddEntity(TEntity entity);
    Task UpdateEntity(string id, TEntity entity);
    Task Delete(string id);
}