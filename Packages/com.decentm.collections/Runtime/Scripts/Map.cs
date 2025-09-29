using JetBrains.Annotations;
using UnityEngine;

namespace DecentM.Collections
{
    /// <summary>
    /// A map is a collection of key-value pairs, where each key is unique.<br />
    /// They're used to store values that can be looked up by a specific key. 
    /// </summary>
    [AddComponentMenu("DecentM/Collections/NonGenerics/Map"), DisallowMultipleComponent]
    public class Map : Collection
    {
        // Value structure: new object[]
        // {
        //      new object[] { key, value },
        //      new object[] { key, value },
        //      ...
        // };

        /// <summary>
        /// Add a key-value pair to the map.<br />
        /// Returns false if the key already exists.
        /// </summary>
        [PublicAPI]
        public bool Add(object key, object value)
        {
            if (this.Contains(key))
                return false;

            return this.Set(key, value);
        }

        /// <summary>
        /// Sets the value for the specified key<br />
        /// If the key already exists, it will be overwritten<br />
        /// </summary>
        [PublicAPI]
        public bool Set(object key, object value)
        {
            this.Remove(key);

            int length = this.value.Length;
            this.value = base.Add(this.value, new object[] { key, value });

            return this.value.Length == length + 1;
        }

        /// <summary>
        /// Removes the given key and its value
        /// </summary>
        [PublicAPI]
        public bool Remove(object key)
        {
            int index = this.IndexOf(this.Keys, key);

            if (index == -1)
                return false;

            int length = this.value.Length;
            this.value = this.RemoveAt(this.value, index);

            return this.value.Length == length - 1;
        }

        /// <summary>
        /// Gets the value for the given key, or null if the key doesn't exist
        /// </summary>
        [PublicAPI]
        public object Get(object key)
        {
            int index = this.IndexOf(this.Keys, key);

            if (index == -1)
                return null;

            return this.ElementAt(this.Values, index);
        }

        /// <summary>
        /// Gets the key for the given value, or null if the value doesn't exist<br />
        /// (this operation is slow because it searches through all values until it finds the value)
        /// </summary>
        [PublicAPI]
        public object KeyOf(object value)
        {
            int index = this.IndexOf(this.Values, value);

            if (index == -1)
                return null;

            return this.ElementAt(this.Keys, index);
        }

        /// <summary>
        /// All keys in the map
        /// </summary>
        [PublicAPI]
        public object[] Keys
        {
            get
            {
                object[] result = new object[this.value.Length];

                for (int i = 0; i < this.value.Length; i++)
                {
                    result[i] = ((object[])this.value[i])[0];
                }

                return result;
            }
        }

        /// <summary>
        /// All values in the map
        /// </summary>
        [PublicAPI]
        public object[] Values
        {
            get
            {
                object[] result = new object[this.value.Length];

                for (int i = 0; i < this.value.Length; i++)
                {
                    result[i] = ((object[])this.value[i])[1];
                }

                return result;
            }
        }

        /// <summary>
        /// Check if the map contains the given key
        /// </summary>
        [PublicAPI]
        public bool Contains(object key)
        {
            return this.Contains(this.Keys, key);
        }
    }
}
