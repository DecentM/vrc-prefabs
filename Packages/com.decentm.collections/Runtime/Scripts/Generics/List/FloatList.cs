using UdonSharp;
using UnityEngine;

namespace DecentM.Collections.Generics
{
    /// <summary>
    /// A list of floats.
    /// </summary>
    [
        AddComponentMenu("DecentM/Collections/Generics/List/FloatList"),
        DisallowMultipleComponent,
    ]
    public sealed class FloatList : List
    {
        public bool Contains(float item)
        {
            return this.Contains((object)item);
        }

        public bool Add(float item)
        {
            return this.Add((object)item);
        }

        public bool AddRange(float[] items)
        {
            object[] objItems = new object[items.Length];
            for (int i = 0; i < items.Length; i++)
            {
                objItems[i] = items[i];
            }
            return this.AddRange(objItems);
        }

        public float FloatAt(int index)
        {
            return (float)this.ElementAt(index);
        }

        public bool Insert(int index, float item)
        {
            return this.Insert(index, (object)item);
        }

        public bool InsertRange(int index, float[] items)
        {
            object[] objItems = new object[items.Length];
            for (int i = 0; i < items.Length; i++)
            {
                objItems[i] = items[i];
            }
            return this.InsertRange(index, objItems);
        }

        public int IndexOf(float item)
        {
            return this.IndexOf((object)item);
        }

        public bool Remove(float item)
        {
            return this.Remove((object)item);
        }

        public float[] ToFloatArray()
        {
            object[] objArray = this.ToArray();
            float[] floatArray = new float[objArray.Length];
            for (int i = 0; i < objArray.Length; i++)
            {
                floatArray[i] = (float)objArray[i];
            }
            return floatArray;
        }
    }
}
