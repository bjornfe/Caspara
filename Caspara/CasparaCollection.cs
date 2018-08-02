using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Caspara
{
    [DataContract]
    public class CasparaCollection<T> : ICasparaCollection<T>
    {

        [DataMember]
        protected ConcurrentDictionary<object, T> Items { get; set; } = new ConcurrentDictionary<object, T>();

        protected object _lockObject = new object();


        public CasparaCollection()
        {
            if(Items == null)
                Items = new ConcurrentDictionary<object, T>();

            _lockObject = new object();
        }


        public void Set(object Key, T Item)
        {
            //lock (_lockObject)
            //{
            Items[Key] = Item;
            //}
        }

        public T1 Get<T1>() where T1 : T
        {
            //lock (_lockObject)
            //{
            if (Items.Count > 0 && Items.Values.Any(v => v != null && v is T1))
            {
                return (T1)Items.Values.Where(v => v != null && v is T1).First();
            }
            return default(T1);
            //}

        }

        public T1 Get<T1>(object Key) where T1 : T
        {
            //lock (_lockObject)
            //{
            if (Items.ContainsKey(Key) && Items[Key] is T1)
                return (T1)Items[Key];
            else if (!typeof(T1).IsInterface && !typeof(T1).IsAbstract)
            {
                T1 item = default(T1);

                try { item = Activator.CreateInstance<T1>(); }
                catch { }

                Items[Key] = item;
                return item;
            }
            else
                return default(T1);
            //}
        }

        public T Get(object Key)
        {
           
            //lock (_lockObject)
            //{
            if (Items.ContainsKey(Key))
                return Items[Key];
            else if (!typeof(T).IsInterface && !typeof(T).IsAbstract)
            {
                T item = default(T);

                try { item = Activator.CreateInstance<T>(); }
                catch { }

                Items[Key] = item;
                return item;
            }

            return default(T);
            //}
        }

        public void Remove(T Item)
        {
            //lock (_lockObject)
            //{
            if (Items.Count > 0 && Items.Values.Contains(Item))
            {
                while (Items.Values.Contains(Item))
                {
                    var item = Items.Where(v => v.Value.Equals(Item)).First();
                    Items.TryRemove(item.Key, out Item);
                }
            }
            //}
        }

        public void Remove(object Key)
        {
            //lock (_lockObject)
            //{
            if (Items.ContainsKey(Key))
                Items.TryRemove(Key, out var Item);
            //}
        }
    }
}
