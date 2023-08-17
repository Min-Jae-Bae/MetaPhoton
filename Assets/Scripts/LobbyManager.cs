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

    //�� ���� ��ư
    public Button btnJoinRoom;

    //�� ���� ��ư
    public Button btnCreateRoom;

    //RoomItem Prefab
    public GameObject roomItemFactory;

    //RoomListView -> Content -> RectTransform
    public RectTransform rtContent;

    private Dictionary<string, RoomInfo> roomCache = new Dictionary<string, RoomInfo>();

    private void Start()
    {
        SoundManager.instance.PlayBGM(SoundManager.EBgm.BGM_LOBBY);

        //�� ����, ���� ��Ȱ��ȭ
        btnJoinRoom.interactable = btnCreateRoom.interactable = false;
        //InputRoomName�� ������ ����� �� ȣ��Ǵ� �Լ�
        inputRoomName.onValueChanged.AddListener(OnValueChangedRoomName);
        //InputMaxPlayer�� ������ ����� �� ȣ��Ǵ� �Լ�
        inputMaxPlayer.onValueChanged.AddListener(OnValueChangedMaxPlayer);
    }

    //���� & ���� ��ư�� ����
    private void OnValueChangedRoomName(string room)
    {
        //���� ��ư Ȱ�� / ��Ȱ��
        btnJoinRoom.interactable = room.Length > 0;
        //���� ��ư Ȱ�� / ��Ȱ��
        btnCreateRoom.interactable = room.Length > 0 && inputMaxPlayer.text.Length > 0;
    }

    //���� ��ư�� ����
    private void OnValueChangedMaxPlayer(string max)
    {
        btnCreateRoom.interactable = max.Length > 0 && inputRoomName.text.Length > 0;
    }

    //�� ���� �Ϸ�� ȣ�� �Ǵ� �Լ�
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        print("�� ���� �Ϸ�");
    }

    //�� ���� ���н� ȣ�� �Ǵ� �Լ�
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        print(message + "�� ���� ����");
    }

    public void JoinRoom()
    {
        //�� ���� ��û
        PhotonNetwork.JoinRoom(inputRoomName.text + inputPassword.text);
    }

    //�� ���� �Ϸ�� ȣ��Ǵ� �Լ�
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        print("�� ���� �Ϸ�");

        //GameScene���� �̵�
        PhotonNetwork.LoadLevel("GameScene");
    }

    //�� ���� ���н� ȣ��Ǵ� �Լ�
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        print("�� ���� ����: " + message);
    }

    public void CreateRoom()
    {
        //�� �ɼ��� ����(�ִ� �ο�)
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = int.Parse(inputMaxPlayer.text);
        //�� ��Ͽ� ���̰� �ϳ�? ���ϳ�?
        roomOptions.IsVisible = true;
        //�� ���� ��û
        roomOptions.IsOpen = false;

        //custom ����
        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
        hash["room_name"] = inputRoomName.text;
        hash["map_idx"] = 10;
        hash["use_item"] = true;

        //custom ������ option�� ����
        roomOptions.CustomRoomProperties = hash;

        //custom ������ lobby���� ����� �� �ְ� ����
        string[] customKeys = { "room_name", "map_idx", "use_item" };
        roomOptions.CustomRoomPropertiesForLobby = customKeys;

        //Ư�� �κ� �� ���� ��û
        //TypedLobby typedLobby = new TypedLobby("Meta Lobby", LobbyType.Default);
        PhotonNetwork.CreateRoom(inputRoomName.text + inputPassword.text, roomOptions);
    }

    private void RemoveRoomList()
    {
        //rtContent�� �ִ� �ڽ� GameObject�� ��� ����
        foreach (Transform tr in rtContent)
        {
            Destroy(tr.gameObject);
        }
    }

    private void UpdateRoomList(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {
            //roomCache�� info�� �� �̸����� �Ǿ��ִ� Key �� �����ϴ�?
            if (roomCache.ContainsKey(info.Name))
            {
                if (info.RemovedFromList)
                {
                    roomCache.Remove(info.Name);
                    continue;
                }
            }
            //�߰�, ����
            roomCache[info.Name] = info;
        }
    }

    private void CreateRoomList()
    {
        foreach (RoomInfo info in roomCache.Values)
        {
            GameObject goRoomItem = Instantiate(roomItemFactory, rtContent);
            //������� roomItem�� �θ� scrollView -> Content�� tranform���� �Ѵ�.
            //goRoomItem.transform.parent = rtContent;

            //custom ���� �̾ƿ���.
            string roomName = (string)(info.CustomProperties["room_name"]);
            //int mapIdx = (int)(info.CustomProperties["map_idx"]);
            bool useItem = (bool)(info.CustomProperties["use_item"]);
            //������� roomItem���� RoomItem ������Ʈ �����´�.
            RoomItem roomItem = goRoomItem.GetComponent<RoomItem>();
            //������ ������Ʈ�� ������ �ִ� SetInfo �Լ� ����
            roomItem.SetInfo(info.Name, info.PlayerCount, info.MaxPlayers);
            //RoomItem�� Ŭ�� �Ǿ��� �� ȣ��Ǵ� �Լ� ���
            roomItem.onChangeRoomName = OnChangeRoomNameField;

            roomItem.onChangeRoomName = (string roomName) =>
            {
                inputRoomName.text = roomName;
            };
        }
    }

    //�������� ���� ����ų� �������� �� ȣ��Ǵ� �Լ�
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);

        //��ü �븮��ƮUI ����
        RemoveRoomList();
        //���� ���� �����ϴ� �븮��Ʈ ���� ����
        UpdateRoomList(roomList);
        //�븮��Ʈ ������ ������ UI�� �ٽ� ����
        CreateRoomList();
    }

    public void OnChangeRoomNameField(string roomName)
    {
        inputRoomName.text = roomName;
    }
}