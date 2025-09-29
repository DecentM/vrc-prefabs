using UdonSharp;
using UnityEngine;

namespace DecentM.Collections.Generics
{
    /// <summary>
    /// A queue of UdonSharpBehaviours.
    /// </summary>
    [
        AddComponentMenu("DecentM/Collections/Generics/Queue/UdonSharpBehaviourQueue"),
        DisallowMultipleComponent
    ]
    public sealed class UdonSharpBehaviourQueue : Queue
    {
        public bool Contains(UdonSharpBehaviour item)
        {
            return this.Contains((object)item);
        }

        public bool Enqueue(UdonSharpBehaviour item)
        {
            return this.Enqueue((object)item);
        }

        public bool EnqueueFirst(UdonSharpBehaviour item)
        {
            return this.EnqueueFirst((object)item);
        }

        public UdonSharpBehaviour PeekUdonSharpBehaviour()
        {
            return (UdonSharpBehaviour)this.Peek();
        }

        public UdonSharpBehaviour DequeueUdonSharpBehaviour()
        {
            return (UdonSharpBehaviour)this.Dequeue();
        }
    }
}
