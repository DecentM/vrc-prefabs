using UdonSharp;
using UnityEngine;

namespace DecentM.Collections.Generics
{
    /// <summary>
    /// A list of UdonSharpBehaviours.
    /// </summary>
    [
        AddComponentMenu("DecentM/Collections/Generics/List/UdonSharpBehaviourList"),
        DisallowMultipleComponent
    ]
    public sealed class UdonSharpBehaviourList : List
    {
        public bool Contains(UdonSharpBehaviour item)
        {
            return this.Contains((object)item);
        }

        public bool Add(UdonSharpBehaviour item)
        {
            return this.Add((object)item);
        }

        public bool AddRange(UdonSharpBehaviour[] items)
        {
            return this.AddRange((object[])items);
        }

        public UdonSharpBehaviour UdonSharpBehaviourAt(int index)
        {
            return (UdonSharpBehaviour)this.ElementAt(index);
        }

        public bool Insert(int index, UdonSharpBehaviour item)
        {
            return this.Insert(index, (object)item);
        }

        public bool InsertRange(int index, UdonSharpBehaviour[] items)
        {
            return this.InsertRange(index, (object[])items);
        }

        public int IndexOf(UdonSharpBehaviour item)
        {
            return this.IndexOf((object)item);
        }

        public bool Remove(UdonSharpBehaviour item)
        {
            return this.Remove((object)item);
        }

        public UdonSharpBehaviour[] ToVRCPlayerAPIArray()
        {
            return (UdonSharpBehaviour[])this.ToArray();
        }
    }
}
