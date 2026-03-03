using System;
using UnityEngine;

namespace Ball
{
    public class Button : MonoBehaviour
    {
        [Header("상태 설정")]
        [Tooltip("체크하면 게임 시작 시 버튼이 눌린 상태(ON)로 시작합니다.")]
        [SerializeField] bool isOn = false;

        [Header("연결된 오브젝트")]
        [Tooltip("버튼이 ON일 때 켜지고, OFF일 때 꺼질 오브젝트들 (정상 작동)")]
        [SerializeField] GameObject[] targetOn;

        [Tooltip("버튼이 ON일 때 꺼지고, OFF일 때 켜질 오브젝트들 (반전 작동)")]
        [SerializeField] GameObject[] targetOff;

        private void Start()
        {
            // 시작 시 현재 isOn 상태에 맞춰 블록들 상태 동기화
            UpdateBlocksState();
        }

        private void OnCollisionEnter(Collision collision)
        {
            // 플레이어(공) 충돌 로직이 필요하다면 태그 체크 등을 추가하세요.
            // 여기서는 닿으면 무조건 토글(Toggle)되도록 합니다.

            isOn = !isOn; // 상태 반전 (ON <-> OFF)
            UpdateBlocksState();
        }

        public void ResetButton()
        {
            isOn = false; // 초기화 시 기본값 (필요에 따라 true로 변경)
            UpdateBlocksState();
        }

        // 상태에 따라 블록들을 켜고 끄는 함수
        void UpdateBlocksState()
        {
            // 1. 정방향 그룹 (버튼이 켜지면 켜짐)
            if (targetOn != null)
            {
                foreach (var block in targetOn)
                {
                    if (block != null) block.SetActive(isOn);
                }
            }

            // 2. 역방향 그룹 (버튼이 켜지면 꺼짐) -> 여기가 핵심!
            if (targetOff != null)
            {
                foreach (var block in targetOff)
                {
                    if (block != null) block.SetActive(!isOn);
                }
            }
        }
    }
}

