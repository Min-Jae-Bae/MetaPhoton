using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatItem : MonoBehaviour
{
    public Text chatText;
    public RectTransform rectTransform;

    private void Awake()
    {
        chatText = GetComponent<Text>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void SetText(string s)
    {
        //�ؽ�Ʈ ����
        chatText.text = s;
        //�ؽ�Ʈ�� ���缭 ũ�⸦ ����
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, chatText.preferredHeight);
    }
}