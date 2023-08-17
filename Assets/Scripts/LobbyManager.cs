using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    //Input Room name
    public InputField inputRoomName;

    //Input Max Player
    public InputField inputMaxPlayer;

    public InputField inputPassword;

    //방 참여 버튼
    public Button btnJoinRoom;

    //방 생성 버튼
    public Button btnCreateRoom;

    //RoomItem Prefab
    public GameObject roomItemFactory;

    //RoomListView -> Content -> RectTransform
    public RectTransform rtContent;

    private Dictionary<string, RoomInfo> roomCache = new Dictionary<string, RoomInfo>();

    private void Start()
    {
        SoundManager.instance.PlayBGM(SoundManager.EBgm.BGM_LOBBY);

        //방 참여, 생성 비활성화
        btnJoinRoom.interactable = btnCreateRoom.interactable = false;
        //InputRoomName의 내용이 변경될 때 호출되는 함수
        inputRoomName.onValueChanged.AddListener(OnValueChangedRoomName);
        //InputMaxPlayer의 내용이 변경될 때 호출되는 함수
        inputMaxPlayer.onValueChanged.AddListener(OnValueChangedMaxPlayer);
    }

    //참여 & 생성 버튼에 관여
    private void OnValueChangedRoomName(string room)
    {
        //참여 버튼 활성 / 비활성
        btnJoinRoom.interactable = room.Length > 0;
        //생성 버튼 활성 / 비활성
        btnCreateRoom.interactable = room.Length > 0 && inputMaxPlayer.text.Length > 0;
    }

    //생성 버튼에 관여
    private void OnValueChangedMaxPlayer(string max)
    {
        btnCreateRoom.interactable = max.Length > 0 && inputRoomName.text.Length > 0;
    }

    //방 생성 완료시 호출 되는 함수
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        print("방 생성 완료");
    }

    //방 생성 실패시 호출 되는 함수
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        print(message + "방 생성 실패");
    }

    public void JoinRoom()
    {
        //방 입장 요청
        PhotonNetwork.JoinRoom(inputRoomName.text + inputPassword.text);
    }

    //방 입장 완료시 호출되는 함수
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        print("방 입장 완료");

        //GameScene으로 이동
        PhotonNetwork.LoadLevel("GameScene");
    }

    //방 입장 실패시 호출되는 함수
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        print("방 입장 실패: " + message);
    }

    public void CreateRoom()
    {
        //방 옵션을 설정(최대 인원)
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = int.Parse(inputMaxPlayer.text);
        //방 목록에 보이게 하냐? 안하냐?
        roomOptions.IsVisible = true;
        //방 생성 요청
        roomOptions.IsOpen = false;

        //custom 설정
        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
        hash["room_name"] = inputRoomName.text;
        hash["map_idx"] = 10;
        hash["use_item"] = true;

        //custom 설정을 option에 셋팅
        roomOptions.CustomRoomProperties = hash;

        //custom 정보를 lobby에서 사용할 수 있게 설정
        string[] customKeys = { "room_name", "map_idx", "use_item" };
        roomOptions.CustomRoomPropertiesForLobby = customKeys;

        //특정 로비에 방 생성 요청
        //TypedLobby typedLobby = new TypedLobby("Meta Lobby", LobbyType.Default);
        PhotonNetwork.CreateRoom(inputRoomName.text + inputPassword.text, roomOptions);
    }

    private void RemoveRoomList()
    {
        //rtContent에 있는 자신 GameObject를 모두 삭제
        foreach (Transform tr in rtContent)
        {
            Destroy(tr.gameObject);
        }
    }

    private void UpdateRoomList(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {
            //roomCache에 info의 방 이름으로 되어있는 Key 값 존재하니?
            if (roomCache.ContainsKey(info.Name))
            {
                if (info.RemovedFromList)
                {
                    roomCache.Remove(info.Name);
                    continue;
                }
            }
            //추가, 삭제
            roomCache[info.Name] = info;
        }
    }

    private void CreateRoomList()
    {
        foreach (RoomInfo info in roomCache.Values)
        {
            GameObject goRoomItem = Instantiate(roomItemFactory, rtContent);
            //만들어진 roomItem의 부모를 scrollView -> Content의 tranform으로 한다.
            //goRoomItem.transform.parent = rtContent;

            //custom 정보 뽑아오자.
            string roomName = (string)(info.CustomProperties["room_name"]);
            //int mapIdx = (int)(info.CustomProperties["map_idx"]);
            bool useItem = (bool)(info.CustomProperties["use_item"]);
            //만들어진 roomItem에서 RoomItem 컴포넌트 가져온다.
            RoomItem roomItem = goRoomItem.GetComponent<RoomItem>();
            //가져온 컴포넌트가 가지고 있는 SetInfo 함수 실행
            roomItem.SetInfo(info.Name, info.PlayerCount, info.MaxPlayers);
            //RoomItem이 클릭 되었을 때 호출되는 함수 등록
            roomItem.onChangeRoomName = OnChangeRoomNameField;

            roomItem.onChangeRoomName = (string roomName) =>
            {
                inputRoomName.text = roomName;
            };
        }
    }

    //누군가가 방을 만들거나 수정했을 때 호출되는 함수
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);

        //전체 룸리스트UI 삭제
        RemoveRoomList();
        //내가 따로 관리하는 룸리스트 정보 갱신
        UpdateRoomList(roomList);
        //룸리스트 정보를 가지고 UI를 다시 생성
        CreateRoomList();
    }

    public void OnChangeRoomNameField(string roomName)
    {
        inputRoomName.text = roomName;
    }
}