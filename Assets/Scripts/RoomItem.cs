using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomItem : MonoBehaviour
{
    public Text roomInfo;

    //Ŭ�� �Ǿ��� �� ȣ�� ���� �Լ��� ���� ����
    public Action<string> onChangeRoomName;

    public void SetInfo(string roomName, int currPlayer, int maxPlayer)
    {
        //���� ���� ������Ʈ �̸��� �� �̸����� ����
        name = roomName;
        // �� ������ Text �� ����
        roomInfo.text = roomName + " ( " + currPlayer + " / " + maxPlayer + " )";
    }

    public void OnClick()
    {
        //onChangeRoomName �� null�� �ƴ϶��
        if (onChangeRoomName != null)
        {
            onChangeRoomName(name);
        }
        /*        //1. InputRoomName ���ӿ�����Ʈ ã��
                GameObject go = GameObject.Find("InputRoomName");
                //2. ã�� ���ӿ�����Ʈ���� InputField ������Ʈ ��������
                InputField inputField = go.GetComponent<InputField>();
                //3. ������ ������Ʈ�� �̿��ؼ� Text �� ����
                inputField.text = name;*/
    }

}