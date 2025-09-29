using UnityEngine;

namespace DecentM.Collections.Generics
{
    /// <summary>
    /// A queue of GameObjects.
    /// </summary>
    [
        AddComponentMenu("DecentM/Collections/Generics/Queue/GameObjectQueue"),
        DisallowMultipleComponent
    ]
    public sealed class GameObjectQueue : Queue
    {
        public bool Contains(GameObject item)
        {
            return this.Contains((object)item);
        }

        public bool Enqueue(GameObject item)
        {
            return this.Enqueue((object)item);
        }

        public bool EnqueueFirst(GameObject item)
        {
            return this.EnqueueFirst((object)item);
        }

        public GameObject PeekGameObject()
        {
            return (GameObject)this.Peek();
        }

        public GameObject DequeueGameObject()
        {
            return (GameObject)this.Dequeue();
        }
    }
}
