using JetBrains.Annotations;
using UnityEngine;

namespace DecentM.Collections
{
    /// <summary>
    /// A queue is a collection that accepts new items at the end, and removes items from the front (First-In-Fist-Out).
    /// </summary>
    [AddComponentMenu("DecentM/Collections/Queue")]
    public class Queue : Collection
    {
        /// <summary>
        /// Check if the queue contains the specified item
        /// </summary>
        [PublicAPI]
        public bool Contains(object item)
        {
            return this.Contains(this.value, item);
        }

        /// <summary>
        /// Add an item to the end of the queue
        /// </summary>
        [PublicAPI]
        public void Enqueue(object item)
        {
            this.value = this.Add(this.value, item);
        }

        /// <summary>
        /// Add an item to the front of the queue
        /// </summary>
        [PublicAPI]
        public bool EnqueueFirst(object item)
        {
            int length = this.value.Length;
            this.value = this.Insert(this.value, 0, item);

            return length + 1 == this.value.Length;
        }

        /// <summary>
        /// Get the item at the front of the queue without removing it, or null if the queue is empty
        /// </summary>
        [PublicAPI]
        public object Peek()
        {
            return this.ElementAt(this.value, 0);
        }

        /// <summary>
        /// Get the item at the front of the queue and remove it, or null if the queue is empty
        /// </summary>
        [PublicAPI]
        public object Dequeue()
        {
            object item = this.Peek();

            if (item == null)
                return null;

            this.value = this.RemoveAt(this.value, 0);

            return item;
        }
    }
}
