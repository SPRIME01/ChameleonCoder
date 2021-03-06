﻿using System.Collections.Concurrent;

namespace ChameleonCoder
{
    /// <summary>
    /// generic abstract base collection class that can be used for Resources, RichContentMembers etc.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    [System.Runtime.InteropServices.ComVisible(false)]
    internal abstract class ComponentCollection<TKey, TValue>
    {
        private ConcurrentDictionary<TKey, TValue> components = new ConcurrentDictionary<TKey, TValue>();

        protected void RegisterComponent(TKey key, TValue member)
        {
            components.TryAdd(key, member);
        }

        protected TValue GetComponent(TKey key)
        {
            TValue value;
            if (components.TryGetValue(key, out value))
                return value;
            return default(TValue);
        }

        protected TKey GetKey(TValue value) // a bit dirty workaround
        {
            var keys = new System.Collections.Generic.List<TKey>(components.Keys);
            var values = new System.Collections.Generic.List<TValue>(components.Values);

            return keys[values.IndexOf(value)];
        }

        public System.Collections.Generic.IEnumerable<TValue> GetList()
        {
            return this.components.Values;
        }

        public bool IsRegistered(TKey key)
        {
            return this.components.ContainsKey(key);
        }

        public bool IsRegistered(TValue value)
        {
            return this.components.Values.Contains(value);
        }
    }
}
