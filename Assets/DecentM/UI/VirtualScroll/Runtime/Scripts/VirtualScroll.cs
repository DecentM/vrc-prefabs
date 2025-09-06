using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using UdonSharp;
using DecentM.Collections;
using System;

namespace DecentM.UI
{ 
    public class VirtualScroll : UdonSharpBehaviour
    {
        [SerializeField] private RectTransform window;
        [SerializeField] private VirtualScrollItem itemTemplate;
        [SerializeField] private Scrollbar verticalScrollbar;

        private List/*<VirtualScrollItem>*/ items;

        [SerializeField] private float windowHeight = 1000;
        [SerializeField] private float itemHeight = 100;

        private int itemsPerScreen
        {
            get { return Mathf.CeilToInt(this.windowHeight / this.itemHeight); }
        }

        private void UpdateWindowSize()
        {
            this.window.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Mathf.Max(this.itemHeight * this.data.Count, this.itemHeight * this.itemsPerScreen));

            foreach (VirtualScrollItem item in this.items.ToArray())
            {
                Destroy(item.gameObject);
            }

            this.items.Clear();

            // Create enough items to fit the screen, plus a buffer on the top and bottom to prevent odd flickering
            for (int i = 0; i < itemsPerScreen; i++)
            {
                GameObject item = Instantiate(this.itemTemplate.gameObject, this.window);
                VirtualScrollItem vsItem = item.GetComponent<VirtualScrollItem>();

                if (vsItem == null)
                {
                    Debug.LogError("[DecentM.VirtualScroll] Item instantiation failed, no component of base class VirtualScrollItem was found on the instance.");
                    Destroy(item);
                    return;
                }

                item.name = $"Item_{i}";
                item.transform.SetParent(this.itemTemplate.transform.parent, true);
                item.transform.localPosition = Vector3.zero;
                item.SetActive(true);
                this.items.Add(vsItem);
            }

            this.verticalScrollbar.value = 1;
        }

        private void Start()
        {
            this.itemTemplate.gameObject.SetActive(false);
            this.UpdateWindowSize();
        }

        [NonSerialized]
        public List/*<object>*/ data;

        [PublicAPI]
        public void SetData(object[] data)
        {
            this.data.FromArray(data);
            this.UpdateWindowSize();
        }

        private object[] GetVisibleDataRange()
        {
            object[] result = new object[this.itemsPerScreen];
            int startIndex = this.firstVisibleIndex;

            for (int i = startIndex; i < startIndex + this.itemsPerScreen; i++)
            {
                result[i - startIndex] = this.data.ElementAt(i);
            }

            return result;
        }

        private float GetHeightForIndex(int index)
        {
            return -1 * index * this.itemHeight + ((1 - this.verticalScrollbar.value) * (this.windowHeight - this.itemHeight)); //  + (this.contentHeight * this.verticalScrollbar.value);
        }

        private int GetIndexForHeight(float normalisedHeight)
        {
            int index = Mathf.FloorToInt(normalisedHeight * this.data.Count);

            return Mathf.Clamp(index, 0, this.data.Count - 1);
        }

        private int firstVisibleIndex
        {
            get { return this.GetIndexForHeight(1 - this.verticalScrollbar.value); }
        }

        private float prevScroll = 0;

        private float GetScrollSpeed(float scroll)
        {
            float speed = scroll - this.prevScroll;
            this.prevScroll = scroll;

            return speed < 0 ? speed * -1 : speed;
        }

        [SerializeField] private Image placeholderBackground;

        private bool lastCheckPending = false;

        private void ScheduleLastCheck()
        {
            if (this.lastCheckPending) return;

            this.lastCheckPending = true;
            this.SendCustomEventDelayedSeconds(nameof(this.LastCheck), 0.1f);
        }

        public void LastCheck()
        {
            this.lastCheckPending = false;
            this.ProcessScroll(false);
        }

        private void ProcessScroll(bool withCheck)
        {
            object[] dataRange = this.GetVisibleDataRange();
            float speed = this.GetScrollSpeed(this.verticalScrollbar.value * 100);

            if (withCheck) this.ScheduleLastCheck();

            if (speed > 0.05)
            {
                this.placeholderBackground.enabled = true;

                foreach (VirtualScrollItem item in this.items.ToArray())
                {
                    item.gameObject.SetActive(false);
                }

                return;
            }

            this.placeholderBackground.enabled = false;

            for (int i = 0; i < dataRange.Length; i++)
            {
                VirtualScrollItem item = (VirtualScrollItem)this.items.ElementAt(i);
                if (item == null) continue;

                item.gameObject.SetActive(true);
                item.index = i;
                item.rectTransform.transform.localPosition = new Vector3(item.rectTransform.transform.localPosition.x, this.GetHeightForIndex(i + this.firstVisibleIndex), item.rectTransform.transform.localPosition.z);
                item.SetData(dataRange[i]);
            }
        }

        // Called from the viewport
        public void OnScroll()
        {
            this.ProcessScroll(true);
        }
    }
}
