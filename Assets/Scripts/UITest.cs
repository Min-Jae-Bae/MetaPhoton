using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITest : MonoBehaviour
{
    public GameObject title;
    public GameObject btn;

    private void Start()
    {
        //처음 위치를 저장
        Vector3 originPos = title.transform.position;
        //타이틀의 위치를 화면 밖으로 이동
        RectTransform rt = title.GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(Screen.width, rt.anchoredPosition.y);

        //iTweem을 이용해서 화면 가운데 나오게 하자
        iTween.MoveTo(title, iTween.Hash(
            "x", originPos.x, "easetype", iTween.EaseType.easeInOutBounce, "time", 1
            ));

        btn.transform.localScale = Vector3.zero;
        iTween.ScaleTo(btn, iTween.Hash(
            "delay", 0.5, "scale", Vector3.one, "easetype", iTween.EaseType.easeOutBack));
    }
}