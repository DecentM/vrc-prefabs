using UnityEngine;

namespace DecentM.Collections.Generics
{
    /// <summary>
    /// A queue of floats.
    /// </summary>
    [
        AddComponentMenu("DecentM/Collections/Generics/Queue/FloatQueue"),
        DisallowMultipleComponent,
    ]
    public sealed class FloatQueue : Queue
    {
        public bool Contains(float item)
        {
            return this.Contains((object)item);
        }

        public bool Enqueue(float item)
        {
            return this.Enqueue((object)item);
        }

        public bool EnqueueFirst(float item)
        {
            return this.EnqueueFirst((object)item);
        }

        public float PeekFloat()
        {
            return (float)this.Peek();
        }

        public float DequeueFloat()
        {
            return (float)this.Dequeue();
        }
    }
}
