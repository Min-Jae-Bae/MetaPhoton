using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMove : MonoBehaviourPun
{
    // 가기 위한 속도가 필요한다.
    public float speed = 5f;
    // 점프 파워
    float jumpPower = 5;
    // 중력
    float gravity = -9.81f;
    // y축 속력
    float yVelocity = 0;
    CharacterController cc;

    void Start()
    {
        //Character Controller 가져오자
        cc = GetComponent<CharacterController>();
    }

    void Update()
    {
        //내가 만든 플레이어라면
        if (photonView.IsMine)
        {
            // WASD 키를 누르면 앞뒤좌우로 움직이고 싶다.
            float hAxis = Input.GetAxis("Horizontal");
            float vAxis = Input.GetAxis("Vertical");

            // 가는 방향이 필요하다.

            Vector3 dirH = transform.right * hAxis;
            Vector3 dirV = transform.forward * vAxis;
            Vector3 dir = dirH + dirV;

            // 속도를 1로 만든다.
            dir.Normalize();



            // 만약 땅에 닿아있다면
            if (cc.isGrounded == true)
            {
                // yVelocity를 0으로 하자
                yVelocity = 0;
            }

            // 스페이스바를 누르면 점프를 하고 싶다.
            if (Input.GetButtonDown("Jump"))
            {
                yVelocity = jumpPower;
            }
            yVelocity += gravity * Time.deltaTime;
            dir.y = yVelocity;


            // 플레이어를 움직인다.
            cc.Move(dir * speed * Time.deltaTime);
        } 
    }
}
