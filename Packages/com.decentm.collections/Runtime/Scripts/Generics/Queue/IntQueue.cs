using UnityEngine;

namespace DecentM.Collections.Generics
{
    /// <summary>
    /// A queue of ints.
    /// </summary>
    [
        AddComponentMenu("DecentM/Collections/Generics/Queue/IntQueue"),
        DisallowMultipleComponent
    ]
    public sealed class IntQueue : Queue
    {
        public bool Contains(int item)
        {
            return this.Contains((object)item);
        }

        public bool Enqueue(int item)
        {
            return this.Enqueue((object)item);
        }

        public bool EnqueueFirst(int item)
        {
            return this.EnqueueFirst((object)item);
        }

        public int PeekInt()
        {
            return (int)this.Peek();
        }

        public int DequeueInt()
        {
            return (int)this.Dequeue();
        }
    }
}
