using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace DecentM.Collections.Generics
{
    /// <summary>
    /// A list of VRCPlayerApis.
    /// </summary>
    [
        AddComponentMenu("DecentM/Collections/Generics/List/VRCPlayerAPIList"),
        DisallowMultipleComponent
    ]
    public sealed class VRCPlayerApiList : List
    {
        public bool Contains(VRCPlayerApi item)
        {
            return this.Contains((object)item);
        }

        public bool Add(VRCPlayerApi item)
        {
            return this.Add((object)item);
        }

        public bool AddRange(VRCPlayerApi[] items)
        {
            return this.AddRange((object[])items);
        }

        public VRCPlayerApi VRCPlayerApiAt(int index)
        {
            return (VRCPlayerApi)this.ElementAt(index);
        }

        public bool Insert(int index, VRCPlayerApi item)
        {
            return this.Insert(index, (object)item);
        }

        public bool InsertRange(int index, VRCPlayerApi[] items)
        {
            return this.InsertRange(index, (object[])items);
        }

        public int IndexOf(VRCPlayerApi item)
        {
            return this.IndexOf((object)item);
        }

        public bool Remove(VRCPlayerApi item)
        {
            return this.Remove((object)item);
        }

        public VRCPlayerApi[] ToVRCPlayerApiArray()
        {
            return (VRCPlayerApi[])this.ToArray();
        }
    }
}
