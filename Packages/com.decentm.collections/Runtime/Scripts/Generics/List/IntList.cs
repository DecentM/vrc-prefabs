using UdonSharp;
using UnityEngine;

namespace DecentM.Collections.Generics
{
    /// <summary>
    /// A list of ints.
    /// </summary>
    [
        AddComponentMenu("DecentM/Collections/Generics/List/IntList"),
        DisallowMultipleComponent
    ]
    public sealed class IntList : List
    {
        public bool Contains(int item)
        {
            return this.Contains((object)item);
        }

        public bool Add(int item)
        {
            return this.Add((object)item);
        }

        public bool AddRange(int[] items)
        {
            object[] objItems = new object[items.Length];
            for (int i = 0; i < items.Length; i++)
            {
                objItems[i] = items[i];
            }
            return this.AddRange(objItems);
        }

        public int IntAt(int index)
        {
            return (int)this.ElementAt(index);
        }

        public bool Insert(int index, int item)
        {
            return this.Insert(index, (object)item);
        }

        public bool InsertRange(int index, int[] items)
        {
            object[] objItems = new object[items.Length];
            for (int i = 0; i < items.Length; i++)
            {
                objItems[i] = items[i];
            }
            return this.InsertRange(index, objItems);
        }

        public int IndexOf(int item)
        {
            return this.IndexOf((object)item);
        }

        public bool Remove(int item)
        {
            return this.Remove((object)item);
        }

        public int[] ToIntArray()
        {
            object[] objArray = this.ToArray();
            int[] intArray = new int[objArray.Length];
            for (int i = 0; i < objArray.Length; i++)
            {
                intArray[i] = (int)objArray[i];
            }
            return intArray;
        }
    }
}
