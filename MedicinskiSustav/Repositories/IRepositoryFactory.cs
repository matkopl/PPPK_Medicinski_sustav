namespace MedicinskiSustav.Repositories
{
    public interface IRepositoryFactory
    {
        IRepository<T> GetRepository<T>() where T : class;
    }

    public class RepositoryFactory : IRepositoryFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public RepositoryFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IRepository<T> GetRepository<T>() where T : class
        {
            return _serviceProvider.GetRequiredService<IRepository<T>>();
        }
    }
}
