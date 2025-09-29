using System;
using UnityEngine;
using UnityEngine.UI;
using UdonSharp;
using DecentM.Collections;

namespace DecentM.UI
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class Dropdown : UdonSharpBehaviour
    {
        public GameObject optionTemplate;
        public Transform optionsRoot;
        public Animator animator;
        public Button button;

        private UdonSharpBehaviour listener;
        private string onChangeEventName;

        [SerializeField]
        private List/*<GameObject>*/ instantiatedOptions;

        private void Start()
        {
            this.optionTemplate.SetActive(false);
            this.Clear();
        }

        public void SetListener(UdonSharpBehaviour listener, string onChangeEventName)
        {
            this.listener = listener;
            this.onChangeEventName = onChangeEventName;
        }

        public void Clear()
        {
            foreach (GameObject option in this.instantiatedOptions.ToArray())
            {
                Destroy(option);
            }

            this.options = new object[0][];
            this.instantiatedOptions.Clear();
        }

        private float elapsed = 0;
        private float instantinatingDelay = 0.15f;

        private object[][] options;

        private void FixedUpdate()
        {
            if (this.options.Length == this.instantiatedOptions.Count())
                return;

            this.elapsed += Time.fixedUnscaledDeltaTime;
            if (elapsed <= this.instantinatingDelay)
                return;
            this.elapsed = 0;

            int newIndex = this.instantiatedOptions.Count();
            if (newIndex >= this.options.Length || this.options[newIndex] == null)
                return;

            this.AddOption(this.options[newIndex]);
        }

        private void AddOption(object[] optionKvp)
        {
            GameObject instance = Instantiate(this.optionTemplate);

            instance.transform.SetParent(this.optionsRoot);
            instance.name = $"{optionKvp[0]}_{optionKvp[1]}";
            instance.transform.SetPositionAndRotation(
                this.optionTemplate.transform.position,
                this.optionTemplate.transform.rotation
            );
            instance.transform.localScale = this.optionTemplate.transform.localScale;

            DropdownOption option = instance.GetComponent<DropdownOption>();
            if (option == null)
                return;

            option.SetData(this, optionKvp[0], (string)optionKvp[1]);
            instance.SetActive(true);

            this.instantiatedOptions.Add(instance);
        }

        public void SetOptions(string[][] options)
        {
            this.Clear();
            this.options = new object[options.Length][];

            for (int i = 0; i < options.Length; i++)
            {
                this.options[i] = new object[] { options[i][0], options[i][1] };
            }
        }

        private object value = null;

        public object GetValue()
        {
            return this.value;
        }

        public void OnValueClick(object value)
        {
            this.value = value;
            this.listener.SendCustomEvent(this.onChangeEventName);
            this.animator.SetBool("DropdownOpen", false);
        }

        public void OnButtonClick()
        {
            this.animator.SetBool("DropdownOpen", !this.animator.GetBool("DropdownOpen"));
        }
    }
}
