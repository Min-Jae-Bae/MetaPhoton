using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class playerRot : MonoBehaviourPun
{
    // 누적 값
    float rotX, rotY;
    private float speed = 200f;
    public Camera trCam;

    void Start()
    {
        //내가 생성한 플레이어 일때만 카메라를 활성화 하자
        if (photonView.IsMine)
        {
            trCam.gameObject.SetActive(true);
        }

    }
    void Update()
    {
        // 내것이 아닐 때 나간다.
        if (!photonView.IsMine) return;

        //마우스의 움직임따라 플레이를 좌우 회전하고
        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");

        // 값을 누적한다.
        rotX += my * speed * Time.deltaTime;
        rotY += mx * speed * Time.deltaTime;

        //좌우 회전각을 제어하고 싶다.
        rotX = Mathf.Clamp(rotX, -75, 75);
        // 회전속도 값을 준다.
        transform.localEulerAngles = new Vector3(0, rotY, 0);
        trCam.transform.localEulerAngles = new Vector3(-rotX, 0, 0);
    }
}
