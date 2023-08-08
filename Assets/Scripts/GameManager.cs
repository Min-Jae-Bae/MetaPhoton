using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    private void Start()
    {
        //OnPhotonSerializeView ȣ�� ��
        PhotonNetwork.SerializationRate = 60;
        //���� �÷��̾� ����
        PhotonNetwork.Instantiate("Player", new Vector3(0, 1, 0), Quaternion.identity);
        //���콺 �����͸� ��Ȱ��ȭ
        Cursor.visible = false;
    }

    private void Update()
    {
        //���࿡ esc Ű�� ������
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //���콺 �����͸� Ȱ��ȭ
            Cursor.visible = true;
        }

        //���콺 Ŭ������ ��
        if (Input.GetMouseButtonDown(0))
        {
            //���콺 Ŭ���� �ش� ��ġ�� UI�� ������
            if (EventSystem.current.IsPointerOverGameObject() == false)
            {                //���콺 �����͸� ��Ȱ��ȭ
                Cursor.visible = false;
            }
        }
    }
}