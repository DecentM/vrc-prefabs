using UnityEngine;

namespace DecentM.Collections.Generics
{
    /// <summary>
    /// A queue of strings.
    /// </summary>
    [
        AddComponentMenu("DecentM/Collections/Generics/Queue/StringQueue"),
        DisallowMultipleComponent
    ]
    public sealed class StringQueue : Queue
    {
        public bool Contains(string item)
        {
            return this.Contains((object)item);
        }

        public bool Enqueue(string item)
        {
            return this.Enqueue((object)item);
        }

        public bool EnqueueFirst(string item)
        {
            return this.EnqueueFirst((object)item);
        }

        public string PeekString()
        {
            return (string)this.Peek();
        }

        public string DequeueString()
        {
            return (string)this.Dequeue();
        }
    }
}
