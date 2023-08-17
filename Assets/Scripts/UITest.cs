using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITest : MonoBehaviour
{
    public GameObject title;
    public GameObject btn;

    private void Start()
    {
        //ó�� ��ġ�� ����
        Vector3 originPos = title.transform.position;
        //Ÿ��Ʋ�� ��ġ�� ȭ�� ������ �̵�
        RectTransform rt = title.GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(Screen.width, rt.anchoredPosition.y);

        //iTweem�� �̿��ؼ� ȭ�� ��� ������ ����
        iTween.MoveTo(title, iTween.Hash(
            "x", originPos.x, "easetype", iTween.EaseType.easeInOutBounce, "time", 1
            ));

        btn.transform.localScale = Vector3.zero;
        iTween.ScaleTo(btn, iTween.Hash(
            "delay", 0.5, "scale", Vector3.one, "easetype", iTween.EaseType.easeOutBack));
    }
}