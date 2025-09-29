using UnityEngine;

namespace DecentM.Collections.Generics
{
    /// <summary>
    /// A stack of GameObjects
    /// </summary>
    [
        AddComponentMenu("DecentM/Collections/Generics/Stack/GameObjectStack"),
        DisallowMultipleComponent,
    ]
    public sealed class GameObjectStack : Stack
    {
        public bool Contains(GameObject item)
        {
            return this.Contains((object)item);
        }

        public bool Push(GameObject item)
        {
            return this.Push((object)item);
        }

        public GameObject PeekGameObject()
        {
            return (GameObject)this.Peek();
        }

        public GameObject PopGameObject()
        {
            return (GameObject)this.Pop();
        }
    }
}
