using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class SimpleConnectionMgr : MonoBehaviourPunCallbacks
{
    void Start()
    {
        //���� ȯ�ἳ���� ������� ������ �õ�
        PhotonNetwork.ConnectUsingSettings();
    }

    void Update()
    {
        
    }

    //������ ���� ���� �Ϸ�
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        print(nameof(OnConnectedToMaster));

        // �κ�����
        JoinLobby();
    }

    //�κ�����
    void JoinLobby()
    {
        //�г��� ����
        PhotonNetwork.NickName = "�����";
        //�⺻ �κ� ����
        PhotonNetwork.JoinLobby();
    }

    //�κ����� �Ϸ�
    public override void OnJoinedLobby()
    {
        base.OnJoinedRoom();
        print(nameof(OnJoinedLobby));

        // �� ���� �Ǵ� ����
        RoomOptions roomOption = new RoomOptions();
        PhotonNetwork.JoinOrCreateRoom("meta_unity_room", roomOption, TypedLobby.Default);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        print(nameof(OnCreateRoomFailed));
    }

    //�� ���� ������ ȣ��Ǵ� �Լ�
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        print(nameof(OnJoinedRoom));
        // ���Ӿ����� �̵�
        PhotonNetwork.LoadLevel("GameScene");
    }

}
