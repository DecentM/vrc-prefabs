using UdonSharp;
using DecentM.Collections;
using UnityEngine;

namespace DecentM.Pubsub
{
    [RequireComponent(typeof(PubsubSubscriberList), typeof(Queue))]
    public abstract class PubsubHost : UdonSharpBehaviour
    {
        private PubsubSubscriberList subscribers;
        private Queue/*<object[]>*/ queue;

        private void Start()
        {
            this.queue = this.GetComponent<Queue>();
        }

        private const int BatchSpeed = 3;

        private int GetBatchSize()
        {
            return Mathf.RoundToInt(this.subscribers.Count() / PubsubHost.BatchSpeed) + 1;
        }

        public int Subscribe(PubsubSubscriber behaviour)
        {
            // Some clients may call this before Start(), so we initialise here instead of in Start()
            if (this.subscribers == null)
                this.subscribers = this.GetComponent<PubsubSubscriberList>();

            this.subscribers.Add(behaviour);

            return this.subscribers.Count() - 1;
        }

        public bool Unsubscribe(int index)
        {
            return this.subscribers.RemoveAt(index);
        }

        private void QueuePush(object eventName, object[] data, PubsubSubscriber behaviour)
        {
            object[] queueItem = new object[] { eventName, data, behaviour };

            this.queue.Enqueue(queueItem);
        }

        private void FixedUpdate()
        {
            if (this.queue == null || this.queue.Count() == 0)
                return;

            int processedCount = 0;

            // Process a batch of items from the queue
            while (processedCount < this.GetBatchSize())
            {
                // Get the first item from the queue and also remove it from the queue
                object[] queueItem = (object[])this.queue.Dequeue();

                if (queueItem == null)
                {
                    processedCount++;
                    break;
                }

                string eventName = (string)queueItem[0];
                object[] data = (object[])queueItem[1];
                PubsubSubscriber subscriber = (PubsubSubscriber)queueItem[2];

                subscriber.OnPubsubEvent(eventName, data);

                processedCount++;
            }
        }

        protected void BroadcastEvent(object eventName, params object[] data)
        {
            if (this.subscribers == null || this.subscribers.Count() == 0)
                return;

            foreach (PubsubSubscriber subscriber in this.subscribers.ToArray())
            {
                this.QueuePush(eventName, data, subscriber);
            }
        }
    }
}