using UnityEngine;
using UdonSharp;
using DecentM.Collections;

namespace DecentM.Pubsub
{
    [RequireComponent(typeof(List))]
    public abstract class PubsubSubscriber : UdonSharpBehaviour
    {
        [HideInInspector] public PubsubHost[] pubsubHosts = new PubsubHost[0];

        private List/*<int>*/ subscriptions;

        virtual protected void _Start() { }

        private void Start()
        {
            this.subscriptions = this.GetComponent<List>();

            if (this.pubsubHosts.Length == 0)
            {
                Debug.LogWarning(
                    $"no pubsub host object is attached to {this.name}, this subscriber will not receive events until a host is attached"
                );
            }

            this.SubscribeAll();
            this._Start();
        }

        virtual protected void _Awake() { }

        private void Awake()
        {
            this._Awake();
        }

        private void SubscribeAll()
        {
            for (int i = 0; i < this.pubsubHosts.Length; i++)
            {
                if (this.pubsubHosts[i] == null)
                    continue;

                int subscription = this.pubsubHosts[i].Subscribe(this);

                this.subscriptions.Add(subscription);
            }
        }

        private void UnsubscribeAll()
        {
            for (int i = 0; i < this.pubsubHosts.Length; i++)
            {
                if (this.pubsubHosts[i] == null)
                    continue;
                int subscription = (int)this.subscriptions.ElementAt(i);

                this.pubsubHosts[i].Unsubscribe(subscription);
            }
        }

        // We expect inheriting behaviours to run this if they change our list of pubsub hosts
        protected void ResubscribeAll()
        {
            this.UnsubscribeAll();
            this.subscriptions.Clear();
            this.SubscribeAll();
        }

        public abstract void OnPubsubEvent(object name, object[] data);
    }
}