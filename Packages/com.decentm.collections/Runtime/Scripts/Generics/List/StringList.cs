using UdonSharp;
using UnityEngine;

namespace DecentM.Collections.Generics
{
    /// <summary>
    /// A list of strings.
    /// </summary>
    [
        AddComponentMenu("DecentM/Collections/Generics/List/StringList"),
        DisallowMultipleComponent
    ]
    public sealed class StringList : List
    {
        public bool Contains(string item)
        {
            return this.Contains((object)item);
        }

        public bool Add(string item)
        {
            return this.Add((object)item);
        }

        public bool AddRange(string[] items)
        {
            return this.AddRange((object[])items);
        }

        public string StringAt(int index)
        {
            return (string)this.ElementAt(index);
        }

        public bool Insert(int index, string item)
        {
            return this.Insert(index, (object)item);
        }

        public bool InsertRange(int index, string[] items)
        {
            return this.InsertRange(index, (object[])items);
        }

        public int IndexOf(string item)
        {
            return this.IndexOf((object)item);
        }

        public bool Remove(string item)
        {
            return this.Remove((object)item);
        }

        public string[] ToStringArray()
        {
            return (string[])this.ToArray();
        }
    }
}
