using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMove : MonoBehaviourPun
{
    // ���� ���� �ӵ��� �ʿ��Ѵ�.
    public float speed = 5f;
    // ���� �Ŀ�
    float jumpPower = 5;
    // �߷�
    float gravity = -9.81f;
    // y�� �ӷ�
    float yVelocity = 0;
    CharacterController cc;

    void Start()
    {
        //Character Controller ��������
        cc = GetComponent<CharacterController>();
    }

    void Update()
    {
        //���� ���� �÷��̾���
        if (photonView.IsMine)
        {
            // WASD Ű�� ������ �յ��¿�� �����̰� �ʹ�.
            float hAxis = Input.GetAxis("Horizontal");
            float vAxis = Input.GetAxis("Vertical");

            // ���� ������ �ʿ��ϴ�.

            Vector3 dirH = transform.right * hAxis;
            Vector3 dirV = transform.forward * vAxis;
            Vector3 dir = dirH + dirV;

            // �ӵ��� 1�� �����.
            dir.Normalize();



            // ���� ���� ����ִٸ�
            if (cc.isGrounded == true)
            {
                // yVelocity�� 0���� ����
                yVelocity = 0;
            }

            // �����̽��ٸ� ������ ������ �ϰ� �ʹ�.
            if (Input.GetButtonDown("Jump"))
            {
                yVelocity = jumpPower;
            }
            yVelocity += gravity * Time.deltaTime;
            dir.y = yVelocity;


            // �÷��̾ �����δ�.
            cc.Move(dir * speed * Time.deltaTime);
        } 
    }
}
