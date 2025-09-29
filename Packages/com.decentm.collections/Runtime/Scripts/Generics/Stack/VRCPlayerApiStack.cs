using UnityEngine;
using VRC.SDKBase;

namespace DecentM.Collections.Generics
{
    /// <summary>
    /// A stack of VRCPlayerApis
    /// </summary>
    [
        AddComponentMenu("DecentM/Collections/Generics/Stack/VRCPlayerApiStack"),
        DisallowMultipleComponent,
    ]
    public sealed class VRCPlayerApiStack : Stack
    {
        public bool Contains(VRCPlayerApi item)
        {
            return this.Contains((object)item);
        }

        public bool Push(VRCPlayerApi item)
        {
            return this.Push((object)item);
        }

        public VRCPlayerApi PeekVRCPlayerApi()
        {
            return (VRCPlayerApi)this.Peek();
        }

        public VRCPlayerApi PopVRCPlayerApi()
        {
            return (VRCPlayerApi)this.Pop();
        }
    }
}
