using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class STTGoogling : MonoBehaviour
{
    // STT로 받은 문장을 목표 지점 설정으로 검색 입력하는 코드
    
    public TextMeshProUGUI stt; // 원본 텍스트 (STT 텍스트)
    public TextMeshProUGUI googling; // 복사할 대상 텍스트 (Googling 텍스트)

    void Update()
    {
        // stt 텍스트를 googling 텍스트로 복사
        googling.text = stt.text;
    }
}
