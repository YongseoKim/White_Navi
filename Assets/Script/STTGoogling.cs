using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class STTGoogling : MonoBehaviour
{
    // STT�� ���� ������ ��ǥ ���� �������� �˻� �Է��ϴ� �ڵ�
    
    public TextMeshProUGUI stt; // ���� �ؽ�Ʈ (STT �ؽ�Ʈ)
    public TextMeshProUGUI googling; // ������ ��� �ؽ�Ʈ (Googling �ؽ�Ʈ)

    void Update()
    {
        // stt �ؽ�Ʈ�� googling �ؽ�Ʈ�� ����
        googling.text = stt.text;
    }
}
