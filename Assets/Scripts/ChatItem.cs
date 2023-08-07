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
        //텍스트 갱신
        chatText.text = s;
        //텍스트에 맞춰서 크기를 조절
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, chatText.preferredHeight);
    }
}