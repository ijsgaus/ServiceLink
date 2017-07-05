namespace ServiceLink.Linq2Db
{
    public interface IContext
    {
        
    }

    public interface IRepository<TContext> where TContext : IContext
    {
        
    }
}