using UnityEngine;
using VRC.SDKBase;

namespace DecentM.Collections.Generics
{
    /// <summary>
    /// A queue of VRCPlayerApis.
    /// </summary>
    [
        AddComponentMenu("DecentM/Collections/Generics/Queue/VRCPlayerApiQueue"),
        DisallowMultipleComponent
    ]
    public sealed class VRCPlayerApiQueue : Queue
    {
        public bool Contains(VRCPlayerApi item)
        {
            return this.Contains((object)item);
        }

        public bool Enqueue(VRCPlayerApi item)
        {
            return this.Enqueue((object)item);
        }

        public bool EnqueueFirst(VRCPlayerApi item)
        {
            return this.EnqueueFirst((object)item);
        }

        public VRCPlayerApi PeekVRCPlayerApi()
        {
            return (VRCPlayerApi)this.Peek();
        }

        public VRCPlayerApi DequeueVRCPlayerApi()
        {
            return (VRCPlayerApi)this.Dequeue();
        }
    }
}
