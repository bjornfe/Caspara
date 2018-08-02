using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Caspara.Repositories
{
    [DataContract]
    public class Repository : ObjectBase, IRepository
    {

        [DataMember]
        private ConcurrentDictionary<object, IObject> Objects { get; set; }

        public Repository()
        {
            Objects = new ConcurrentDictionary<object, IObject>();
        }

        public void Add(IObject item)
        {
            bool OK = false;
            while (!OK)
                OK = Objects.TryAdd(item.ID, item);
        }

        public IObject Get(object ID)
        {
            if (Objects.ContainsKey(ID))
            {
                var OK = false;
                IObject obj = null;
                while (!OK)
                    OK = Objects.TryGetValue(ID, out obj);

                return obj;
            }
            return null;

        }

        public List<IObject> GetAll()
        {
            List<IObject> objects = Objects.Values.ToList();
            return objects;
        }

        public void Remove(object ID)
        {
            if (Objects.ContainsKey(ID))
            {
                var OK = false;
                while (!OK)
                    OK = Objects.TryRemove(ID, out IObject obj);
            }
        }
    }
}
