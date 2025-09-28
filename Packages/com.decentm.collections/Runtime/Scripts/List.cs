using JetBrains.Annotations;
using UnityEngine;

namespace DecentM.Collections
{
    /// <summary>
    /// A list is a collection of items that can be accessed by their index.
    /// </summary>
    [AddComponentMenu("DecentM/Collections/List"), DisallowMultipleComponent]
    public class List : Collection
    {
        /// <summary>
        /// Check if the list contains the specified item
        /// </summary>
        [PublicAPI]
        public bool Contains(object item)
        {
            return this.Contains(this.value, item);
        }

        /// <summary>
        /// Get the item at the specified index, or null if the index is out of range
        /// </summary>
        [PublicAPI]
        public object ElementAt(int index)
        {
            return this.ElementAt(this.value, index);
        }

        /// <summary>
        /// Add an item to the end of the list
        /// </summary>
        [PublicAPI]
        public bool Add(object item)
        {
            int length = this.value.Length;
            this.value = this.Add(this.value, item);

            return this.value.Length == length + 1;
        }

        /// <summary>
        /// Add multiple items to the end of the list
        /// </summary>
        [PublicAPI]
        public bool AddRange(object[] items)
        {
            return this.InsertRange(0, items);
        }

        /// <summary>
        /// Add multiple items to the end of the list from another collection
        /// </summary>
        [PublicAPI]
        public bool AddRange(Collection collection)
        {
            return this.AddRange(collection.ToArray());
        }

        /// <summary>
        /// Insert an item at the specified index
        /// </summary>
        [PublicAPI]
        public bool Insert(int index, object item)
        {
            int length = this.value.Length;
            this.value = this.Insert(this.value, index, item);

            return this.value.Length == length + 1;
        }

        /// <summary>
        /// Insert multiple items starting at the specified index
        /// </summary>
        [PublicAPI]
        public bool InsertRange(int startIndex, object[] items)
        {
            int length = this.value.Length;
            this.value = this.InsertRange(this.value, startIndex, items);

            return this.value.Length == length + items.Length;
        }

        /// <summary>
        /// Insert multiple items starting at the specified index from another collection
        /// </summary>
        [PublicAPI]
        public bool InsertRange(int startIndex, Collection collection)
        {
            return this.InsertRange(startIndex, collection.ToArray());
        }

        /// <summary>
        /// Get the index of the specified item, or -1 if the item is not found
        /// </summary>
        [PublicAPI]
        public int IndexOf(object item)
        {
            return this.IndexOf(this.value, item);
        }

        /// <summary>
        /// Remove the item at the specified index
        /// </summary>
        [PublicAPI]
        public bool RemoveAt(int index)
        {
            int length = this.value.Length;
            this.value = this.RemoveAt(this.value, index);

            // Return success if the result array is one smaller than the previous value
            return this.value.Length == length - 1;
        }

        /// <summary>
        /// Remove the specified item
        /// </summary>
        [PublicAPI]
        public bool Remove(object item)
        {
            int index = this.IndexOf(item);

            if (index == -1)
                return false;

            return this.RemoveAt(index);
        }

        /// <summary>
        /// Remove a range of items from startIndex to endIndex (inclusive)
        /// </summary>
        [PublicAPI]
        public bool RemoveRange(int startIndex, int endIndex)
        {
            if (startIndex > endIndex)
                return false;

            int length = this.value.Length;
            this.value = this.RemoveRange(this.value, startIndex, endIndex);

            return this.value.Length == length - (endIndex - startIndex);
        }

        /// <summary>
        /// Reverse the order of items in the list
        /// </summary>
        [PublicAPI]
        public void Reverse()
        {
            object[] tmp = new object[this.value.Length];

            for (int i = 0; i < this.value.Length; i++)
            {
                tmp[i] = this.value[this.value.Length - 1 - i];
            }

            this.value = tmp;
        }
    }
}
