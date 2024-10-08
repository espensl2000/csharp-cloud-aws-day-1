using exercise.wwwapi.Models;
using System.Linq.Expressions;

namespace exercise.wwwapi.Repository
{
  public interface IDatabaseRepository<T> where T : class
  {
    IEnumerable<T> GetAll();
    T GetById(object id);
    void Insert(T obj);
    void Update(T obj);
    void Delete(object id);
    void Save();
    IEnumerable<T> Include(params Expression<Func<T, object>>[] includes);
  }
}