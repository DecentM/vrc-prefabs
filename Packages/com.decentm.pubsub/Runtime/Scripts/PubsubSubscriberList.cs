using UdonSharp;
using UnityEngine;
using DecentM.Collections;

namespace DecentM.Pubsub
{
    /// <summary>
    /// A list of PubsubSubscribers.
    /// </summary>
    [
        AddComponentMenu("DecentM/Collections/Generics/List/PubsubSubscriberList"),
        DisallowMultipleComponent
    ]
    internal sealed class PubsubSubscriberList : List
    {
        public bool Contains(PubsubSubscriber item)
        {
            return this.Contains((object)item);
        }

        public bool Add(PubsubSubscriber item)
        {
            return this.Add((object)item);
        }

        public bool AddRange(PubsubSubscriber[] items)
        {
            return this.AddRange((object[])items);
        }

        public PubsubSubscriber PubsubSubscriberAt(int index)
        {
            return (PubsubSubscriber)this.ElementAt(index);
        }

        public bool Insert(int index, PubsubSubscriber item)
        {
            return this.Insert(index, (object)item);
        }

        public bool InsertRange(int index, PubsubSubscriber[] items)
        {
            return this.InsertRange(index, (object[])items);
        }

        public int IndexOf(PubsubSubscriber item)
        {
            return this.IndexOf((object)item);
        }

        public bool Remove(PubsubSubscriber item)
        {
            return this.Remove((object)item);
        }

        public PubsubSubscriber[] ToPubsubSubscriberArray()
        {
            return (PubsubSubscriber[])this.ToArray();
        }
    }
}
