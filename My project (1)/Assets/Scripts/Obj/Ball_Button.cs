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
            foreach (var block in Block)
            {
                block.SetActive(false);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (isOn)
            {
                isOn = false;
                foreach (var block in Block)
                {

                    block.SetActive(false);
                }
            }
            else
            {
                isOn = true;
                foreach (var block in Block)
                {

                    block.SetActive(true);
                }
            }
        }
    }
}

