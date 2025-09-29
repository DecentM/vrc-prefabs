using UnityEngine;

namespace DecentM.Collections.Generics
{
    /// <summary>
    /// A stack of ints
    /// </summary>
    [
        AddComponentMenu("DecentM/Collections/Generics/Stack/IntStack"),
        DisallowMultipleComponent,
    ]
    public sealed class IntStack : Stack
    {
        public bool Contains(int item)
        {
            return this.Contains((object)item);
        }

        public bool Push(int item)
        {
            return this.Push((object)item);
        }

        public int PeekInt()
        {
            return (int)this.Peek();
        }

        public int PopInt()
        {
            return (int)this.Pop();
        }
    }
}
