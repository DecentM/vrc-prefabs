using JetBrains.Annotations;
using UnityEngine;

namespace DecentM.Collections
{
    /// <summary>
    /// A stack is a collection that accepts new items at the top, and removes items from the top (Last-In-First-Out).
    /// </summary>
    [AddComponentMenu("DecentM/Collections/Stack")]
    public class Stack : Collection
    {
        /// <summary>
        /// Check if the stack contains the specified item
        /// </summary>
        [PublicAPI]
        public bool Contains(object item)
        {
            return this.Contains(this.value, item);
        }

        /// <summary>
        /// Add an item to the top of the stack
        /// </summary>
        [PublicAPI]
        public bool Push(object item)
        {
            int length = this.value.Length;
            this.value = this.Insert(this.value, 0, item);

            return this.value.Length == length + 1;
        }

        /// <summary>
        /// Get the item at the top of the stack without removing it, or null if the stack is empty
        /// </summary>
        [PublicAPI]
        public object Peek()
        {
            return this.ElementAt(this.value, 0);
        }

        /// <summary>
        /// Get the item at the top of the stack and remove it, or null if the stack is empty
        /// </summary>
        [PublicAPI]
        public object Pop()
        {
            object item = this.Peek();

            if (item == null)
                return null;

            this.value = this.RemoveAt(this.value, 0);

            return item;
        }
    }
}
