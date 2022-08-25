namespace AntiData.Repo;

public interface IMediaRepository<T, in TV>
{
    IEnumerable<T> FindAll();
    T FindById(TV id);    
    T Create(T entity);
    T Update(T entity);
    T Delete(T entity);
    T DeleteById(TV id);
}