using System;
using UnityEngine;

namespace Ball
{
    public class Button : MonoBehaviour
    {
        [SerializeField] bool isOn;
        [SerializeField] GameObject[] Block;

        private void Start()
        {
            ResetButton();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (isOn)
            {
                isOn = false;
                SetBlocks(false);
            }
            else
            {
                isOn = true;
                SetBlocks(true);
            }
        }

        public void ResetButton()
        {
            isOn = false;
            SetBlocks(false);
        }

        void SetBlocks(bool active)
        {
            foreach (var block in Block)
            {
                block.SetActive(active);
            }
        }
    }
}

