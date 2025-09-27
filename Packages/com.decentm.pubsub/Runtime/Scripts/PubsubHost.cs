using UdonSharp;
using DecentM.Collections;
using UnityEngine;

namespace DecentM.Pubsub
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public abstract class PubsubHost : UdonSharpBehaviour
    {
        [SerializeField] private int batchSize = 10;

        [SerializeField] private List/*<PubsubSubscriber>*/ subscribers;

        public int Subscribe(PubsubSubscriber behaviour)
        {
            this.subscribers.Add(behaviour);

            return this.subscribers.Count - 1;
        }

        public bool Unsubscribe(int index)
        {
            return this.subscribers.RemoveAt(index);
        }

        [SerializeField]
        public Queue/*<object[]>*/ queue;

        private void QueuePush(object eventName, object[] data, PubsubSubscriber behaviour)
        {
            object[] queueItem = new object[] { eventName, data, behaviour };

            this.queue.Enqueue(queueItem);
        }

        private object[] QueuePop()
        {
            return (object[])this.queue.Dequeue();
        }

        private void FixedUpdate()
        {
            if (this.queue == null || this.queue.Count == 0)
                return;

            int processedCount = 0;

            // Process a batch of items from the queue
            while (processedCount < this.batchSize)
            {
                // Get the first item from the queue and also remove it from the queue
                object[] queueItem = this.QueuePop();

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
            if (this.subscribers == null || this.subscribers.Count == 0)
                return;

            foreach (PubsubSubscriber subscriber in this.subscribers.ToArray())
            {
                this.QueuePush(eventName, data, subscriber);
            }
        }
    }
}