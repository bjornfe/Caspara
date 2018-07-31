using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Caspara.Repositories
{
    [DataContract]
    public class RepositoryCollection : ObjectBase, IRepositoryCollection
    {
        [DataMember]
        private Dictionary<object, IRepository> Repositories = new Dictionary<object, IRepository>();

        private readonly object _lockObject = new object();
        public void AddRepository(IRepository Repository)
        {
            lock (_lockObject)
            {
                if (!Repositories.ContainsKey(Repository.ID))
                    Repositories[Repository.ID] = Repository;
            }
        }

        public T GetRepository<T>() where T : IRepository
        {
            lock (_lockObject)
            {
                if (Repositories.Values.Any(v => v.GetType() == typeof(T)))
                    return (T)Repositories.Values.Where(v => v.GetType() == typeof(T)).First();
                else
                    return Activator.CreateInstance<T>();
            }
        }

        public T GetRepository<T>(object ID) where T : IRepository
        {
            lock (_lockObject)
            {
                if (Repositories.ContainsKey(ID) && Repositories[ID] is T)
                    return (T)Repositories[ID];
                else
                    return default(T);
            }
        }

        public void RemoveRepository(IRepository repository)
        {
            if (Repositories.ContainsKey(repository.ID))
                Repositories.Remove(repository.ID);
        }

        public void RemoveRepository(object ID)
        {
            if (Repositories.ContainsKey(ID))
                Repositories.Remove(ID);
        }
    }
}
