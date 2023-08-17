using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance;

    private void Awake()
    {
        //만약에 인스턴스 값이 널이라면
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            //인스턴스에 나 자신을 셋팅
            //그렇지 않으면
            //나를 파괴하자
            Destroy(gameObject);
        }
    }

    //모든 플레이어들의 포톤뷰를 가지는 list
    public List<PhotonView> listPlayer;

    private void Start()
    {
        SoundManager.instance.PlayBGM(SoundManager.EBgm.BGM_GAME);

        PhotonNetwork.SendRate = 30;
        //OnPhotonSerializeView 호출 빈도
        PhotonNetwork.SerializationRate = 60;
        SetSpawnPos();
        //내가 위치하는 idx 구하자
        int idx = PhotonNetwork.CurrentRoom.PlayerCount - 1;
        //나의 플레이어 생성
        PhotonNetwork.Instantiate("Player", spawnPos[idx], Quaternion.identity);
        //마우스 포인터를 비활성화
        Cursor.visible = false;
    }

    //플레이어의 위치 값을 초기를 지정한다.
    public Transform trSpawnPosGroup;

    //spawn 위치를 담아놓을 변수
    public Vector3[] spawnPos;

    private void SetSpawnPos()
    {
        //최대 인원 만큼 spawnPos의 공간을 할당
        spawnPos = new Vector3[PhotonNetwork.CurrentRoom.MaxPlayers];
        //PhotonNetwork.CurrentRoom.MaxPlayers
        //각도를 360도에서 갯수로 나눈다.
        float angle = 360 / spawnPos.Length;

        //플레이어를 생성한다.
        for (int i = 0; i < spawnPos.Length; i++)
        {
            trSpawnPosGroup.Rotate(0, angle, 0);
            spawnPos[i] = trSpawnPosGroup.position + trSpawnPosGroup.forward * 5;
        }
    }

    private void Update()
    {
        //만약에 esc 키를 누르면
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //마우스 포인터를 활성화
            Cursor.visible = true;
        }

        //마우스 클릭했을 때
        if (Input.GetMouseButtonDown(0))
        {
            //마우스 클릭시 해당 위치에 UI가 없으면
            if (EventSystem.current.IsPointerOverGameObject() == false)
            {                //마우스 포인터를 비활성화
                Cursor.visible = false;
            }
        }
    }

    //참여한 player의 photonView 추가
    public void AddPlayer(PhotonView pv)
    {
        listPlayer.Add(pv);

        // 모든 플레이어가 참여했다면
        if (listPlayer.Count == PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            ChangeTun();
        }
    }

    //현재 Turn idx
    private int currTurnIdx = -1;

    private void ChangeTun()
    {
        //방장이 아니라면 함수를 나가자
        if (PhotonNetwork.IsMasterClient == false) return;
        //발사한 사람 Turn 종료
        if (currTurnIdx != -1)
        {
            listPlayer[currTurnIdx].RPC(nameof(ChangeTun), RpcTarget.All, false);
        }

        currTurnIdx++;

        //만약에 currTurnIdx가 3이면
        currTurnIdx = currTurnIdx % listPlayer.Count;

        //if (currTurnIdx >= listPlayer.Count)
        //{
        //    currTurnIdx = 0;
        //}
        //currTurnInd을 0으로 한다.
        //다음 사람 Turn 시작
        listPlayer[currTurnIdx].RPC(nameof(ChangeTun), RpcTarget.All, true);
    }

    //새로운 인원이 방에 들어왔을 때 호출되는 함수
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        print(newPlayer.NickName + "들어왔습니다.");
    }
}