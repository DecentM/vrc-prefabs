using UnityEngine;
using UdonSharp;

namespace DecentM.Collections.Generics
{
    /// <summary>
    /// A stack of UdonSharpBehaviours
    /// </summary>
    [
        AddComponentMenu("DecentM/Collections/Generics/Stack/UdonSharpBehaviourStack"),
        DisallowMultipleComponent,
    ]
    public sealed class UdonSharpBehaviourStack : Stack
    {
        public bool Contains(UdonSharpBehaviour item)
        {
            return this.Contains((object)item);
        }

        public bool Push(UdonSharpBehaviour item)
        {
            return this.Push((object)item);
        }

        public UdonSharpBehaviour PeekUdonSharpBehaviour()
        {
            return (UdonSharpBehaviour)this.Peek();
        }

        public UdonSharpBehaviour PopUdonSharpBehaviour()
        {
            return (UdonSharpBehaviour)this.Pop();
        }
    }
}
