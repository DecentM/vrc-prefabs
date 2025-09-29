using UnityEngine;
using UdonSharp;
using DecentM.Collections.Generics;

namespace DecentM.Pubsub
{
    [RequireComponent(typeof(IntList))]
    public abstract class PubsubSubscriber : UdonSharpBehaviour
    {
        private PubsubHost pubsubHost;

        private int subscription = -1;

        protected bool Subscribe(PubsubHost host)
        {
            if (this.subscription != -1 || this.pubsubHost != null)
                return false;

            this.pubsubHost = host;
            this.subscription = this.pubsubHost.Subscribe(this);
            return this.subscription != -1;
        }

        protected bool Unsubscribe()
        {
            if (this.subscription == -1 || this.pubsubHost == null)
                return false;

            if (!this.pubsubHost.Unsubscribe(this.subscription))
                return false;

            this.subscription = -1;
            this.pubsubHost = null;
            return true;
        }

        public abstract void OnPubsubEvent(object name, object[] data);
    }
}