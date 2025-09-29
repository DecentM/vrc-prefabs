using UdonSharp;
using UnityEngine;

namespace DecentM.Collections.Generics
{
    /// <summary>
    /// A list of GameObjects.
    /// </summary>
    [
        AddComponentMenu("DecentM/Collections/Generics/List/GameObjectList"),
        DisallowMultipleComponent
    ]
    public sealed class GameObjectList : List
    {
        public bool Contains(GameObject item)
        {
            return this.Contains((object)item);
        }

        public bool Add(GameObject item)
        {
            return this.Add((object)item);
        }

        public bool AddRange(GameObject[] items)
        {
            return this.AddRange((object[])items);
        }

        public GameObject GameObjectAt(int index)
        {
            return (GameObject)this.ElementAt(index);
        }

        public bool Insert(int index, GameObject item)
        {
            return this.Insert(index, (object)item);
        }

        public bool InsertRange(int index, GameObject[] items)
        {
            return this.InsertRange(index, (object[])items);
        }

        public int IndexOf(GameObject item)
        {
            return this.IndexOf((object)item);
        }

        public bool Remove(GameObject item)
        {
            return this.Remove((object)item);
        }

        public GameObject[] ToGameObjectArray()
        {
            return (GameObject[])this.ToArray();
        }
    }
}
