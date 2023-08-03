using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class SimpleConnectionMgr : MonoBehaviourPunCallbacks
{
    void Start()
    {
        //포톤 환결설정을 기반으로 접속을 시도
        PhotonNetwork.ConnectUsingSettings();
    }

    void Update()
    {
        
    }

    //마스터 서버 접속 완료
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        print(nameof(OnConnectedToMaster));

        // 로비진입
        JoinLobby();
    }

    //로비진입
    void JoinLobby()
    {
        //닉네임 설정
        PhotonNetwork.NickName = "배민재";
        //기본 로비 입장
        PhotonNetwork.JoinLobby();
    }

    //로비진입 완료
    public override void OnJoinedLobby()
    {
        base.OnJoinedRoom();
        print(nameof(OnJoinedLobby));

        // 방 생성 또는 참여
        RoomOptions roomOption = new RoomOptions();
        PhotonNetwork.JoinOrCreateRoom("meta_unity_room", roomOption, TypedLobby.Default);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        print(nameof(OnCreateRoomFailed));
    }

    //방 참여 성공시 호출되는 함수
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        print(nameof(OnJoinedRoom));
        // 게임씬으로 이동
        PhotonNetwork.LoadLevel("GameScene");
    }

}
