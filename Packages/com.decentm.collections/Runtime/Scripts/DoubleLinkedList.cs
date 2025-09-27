using JetBrains.Annotations;
using UnityEngine;

namespace DecentM.Collections
{
    /// <summary>
    /// A Double Linked List (DLL) is a sequence of elements in which each element points to both its previous and next element.<br />
    /// Useful when you need to frequently add and remove elements from both ends of the list or from the middle, as these operations can be performed in constant time.<br />
    /// <br />
    /// This implementation is not performing in constant time, but it does implement the interface of a DLL.
    /// </summary>
    [AddComponentMenu("DecentM/Collections/DoubleLinkedList")]
    public class DoubleLinkedList : Collection
    {
        /*
         * Value structure:
         * new object[] { prev, id, value, next };
         */

        /// <summary>
        /// Returns true if the specified value is in the list.
        /// </summary>
        [PublicAPI]
        public bool Contains(object value)
        {
            return this.Contains(this.Values, value);
        }

        /// <summary>
        /// Returns true if an item with the specified ID is in the list.
        /// </summary>
        [PublicAPI]
        public bool Contains(int id)
        {
            return this.Contains(this.Ids, id);
        }

        /// <summary>
        /// Returns the elements of the list as an array, in order from first to last, by following the "next" pointers.<br />
        /// If the chain is broken, the result will contain only the elements up to the break.
        /// </summary>
        [PublicAPI]
        public override object[] ToArray()
        {
            object[] result = new object[0];
            int currentId = this.FirstId;

            while (true)
            {
                int[] boundaries = this.Boundaries(currentId);
                result = this.Add(result, this.ElementById(currentId));

                if (boundaries[1] == -1)
                    break;

                currentId = boundaries[1];
            }

            return result;
        }

        /// <summary>
        /// Replaces the contents of the list with the elements from the specified array, in the order they appear in the array.
        /// </summary>
        [PublicAPI]
        public override void FromArray(object[] newValue)
        {
            this.Clear();

            for (int i = 0; i < newValue.Length; i++)
            {
                this.Add(newValue[i]);
            }
        }

        /// <summary>
        /// Returns the ID of the item at the specified index.
        /// </summary>
        [PublicAPI]
        public int IdByIndex(int index)
        {
            object[] item = (object[])this.ElementAt(this.value, index);

            if (item == null || item[1] == null)
                return -1;

            return (int)item[1];
        }

        /// <summary>
        /// Returns the index of the item with the specified ID, or -1 if not found.
        /// </summary>
        [PublicAPI]
        public int IndexById(int id)
        {
            for (int i = 0; i < this.value.Length; i++)
            {
                if (this.IdByIndex(i) == id)
                    return i;
            }

            return -1;
        }

        /// <summary>
        /// Returns the index of the first occurrence of the specified value in the list, or -1 if not found.
        /// </summary>
        [PublicAPI]
        public int IndexOf(object searchValue)
        {
            for (int i = 0; i < this.value.Length; i++)
            {
                object[] current = (object[])this.ElementAt(this.value, i);

                if (current == null)
                    continue;

                object value = current[2];

                if (value == searchValue)
                    return i;
            }

            return -1;
        }

        private int nextItemId = 0;

        private object[] CreateItem(object prev, object id, object value, object next)
        {
            return new object[] { prev, id, value, next };
        }

        private object[] CreateItem(object prev, object value, object next)
        {
            return new object[] { prev, this.nextItemId++, value, next };
        }

        private object[] CreateItem(object value)
        {
            return new object[] { -1, this.nextItemId++, value, -1 };
        }

        private int FindWithNegativeIndex(int nullIndex)
        {
            if (nullIndex < 0)
                return -1;

            for (int i = 0; i < this.value.Length; i++)
            {
                object[] item = (object[])this.value[i];
                if (item == null || nullIndex >= item.Length || item[nullIndex] == null)
                    continue;

                if ((int)item[nullIndex] == -1)
                    return i;
            }

            return -1;
        }

        /// <summary>
        /// The ID of the first item in the list, or -1 if the list is empty.
        /// </summary>
        [PublicAPI]
        public int FirstId
        {
            get { return this.IdByIndex(this.FindWithNegativeIndex(0)); }
        }

        /// <summary>
        /// The ID of the last item in the list, or -1 if the list is empty.
        /// </summary>
        [PublicAPI]
        public int LastId
        {
            get { return this.IdByIndex(this.FindWithNegativeIndex(3)); }
        }

        /// <summary>
        /// The first item in the list, or null if the list is empty.
        /// </summary>
        [PublicAPI]
        public object First
        {
            get { return this.ElementById(this.FirstId); }
        }

        /// <summary>
        /// The last item in the list, or null if the list is empty.
        /// </summary>
        [PublicAPI]
        public object Last
        {
            get { return this.ElementById(this.LastId); }
        }

        private void UpdateItemNext(int id, int next)
        {
            int index = this.IndexById(id);
            object itemOrNull = this.ElementAt(this.value, index);

            if (itemOrNull == null)
                return;

            object[] item = (object[])itemOrNull;
            this.value[index] = this.CreateItem(item[0], item[1], item[2], next);
        }

        private void UpdateItemPrev(int id, int prev)
        {
            int index = this.IndexById(id);
            object itemOrNull = this.ElementAt(this.value, index);

            if (itemOrNull == null)
                return;

            object[] item = (object[])itemOrNull;
            this.value[index] = this.CreateItem(prev, item[1], item[2], item[3]);
        }

        private object[] Values
        {
            get
            {
                object[] result = new object[this.value.Length];

                for (int i = 0; i < this.value.Length; i++)
                {
                    result[i] = ((object[])this.value[i])[2];
                }

                return result;
            }
        }

        private object[] Ids
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
        /// Returns the item with the specified ID, or null if not found.
        /// </summary>
        [PublicAPI]
        public object ElementById(int id)
        {
            return this.ElementAt(this.Values, this.IndexById(id));
        }

        private int[] GetBoundaries(object[] item)
        {
            int prev = -1;
            int next = -1;

            if (item[0] != null)
                prev = (int)item[0];
            if (item[3] != null)
                next = (int)item[3];

            return new int[] { prev, next };
        }

        /// <summary>
        /// Returns a tuple (int[] { prev, next }) of IDs of the previous and next items in the list for the item with the specified ID.
        /// </summary>
        [PublicAPI]
        public int[] Boundaries(int id)
        {
            object[] item = (object[])this.ElementAt(this.value, this.IndexById(id));

            if (item == null)
                return new int[] { -1, -1 };

            return this.GetBoundaries(item);
        }

        /// <summary>
        /// Adds the specified item to the end of the list and returns its ID.
        /// </summary>
        [PublicAPI]
        public int Add(object item)
        {
            int lastItemId = this.LastId;

            // If there's an item with no "next" property, we need to add the new item as the next one.
            // The "next" property of the current item is always null, as we're always adding the newest item.
            if (lastItemId >= 0)
            {
                this.value = this.Add(this.value, this.CreateItem(item));

                this.UpdateItemPrev(this.IdByIndex(this.value.Length - 1), lastItemId);
                this.UpdateItemNext(lastItemId, this.IdByIndex(this.value.Length - 1));
            }
            // Otherwise, we're adding the first item, so we create it with no "prev" property.
            else
            {
                this.value = this.Add(this.value, this.CreateItem(item));
            }

            return this.IdByIndex(this.value.Length - 1);
        }

        /// <summary>
        /// Adds the specified item after the item with the specified ID and returns the added item's ID, or -1 if the specified ID was not found.
        /// </summary>
        [PublicAPI]
        public int AddAfter(int id, object item)
        {
            if (!this.Contains(id))
                return -1;

            int[] bounds = this.Boundaries(id);

            // Add the item to the end of the array and connect it to the upper bounds of the
            // item before
            this.value = this.Add(this.value, this.CreateItem(id, item, bounds[1]));

            int newId = this.IdByIndex(this.value.Length - 1);

            // update the "next" property of the item specified
            this.UpdateItemNext(id, newId);
            // update the "prev" property of the item after the item specified
            this.UpdateItemPrev(bounds[1], newId);

            return newId;
        }

        /// <summary>
        /// Removes the item with the specified ID from the list.<br />
        /// Returns true if successful, or false if not found.
        /// </summary>
        [PublicAPI]
        public bool Remove(int id)
        {
            int index = this.IndexById(id);
            int[] bounds = this.Boundaries(id);
            int length = this.value.Length;

            this.value = this.RemoveAt(this.value, index);

            this.UpdateItemPrev(bounds[1], bounds[0]);
            this.UpdateItemNext(bounds[0], bounds[1]);

            return this.value.Length == length - 1;
        }
    }
}
