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
        //���࿡ �ν��Ͻ� ���� ���̶��
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            //�ν��Ͻ��� �� �ڽ��� ����
            //�׷��� ������
            //���� �ı�����
            Destroy(gameObject);
        }
    }

    //��� �÷��̾���� ����並 ������ list
    public List<PhotonView> listPlayer;

    private void Start()
    {
        SoundManager.instance.PlayBGM(SoundManager.EBgm.BGM_GAME);

        PhotonNetwork.SendRate = 30;
        //OnPhotonSerializeView ȣ�� ��
        PhotonNetwork.SerializationRate = 60;
        SetSpawnPos();
        //���� ��ġ�ϴ� idx ������
        int idx = PhotonNetwork.CurrentRoom.PlayerCount - 1;
        //���� �÷��̾� ����
        PhotonNetwork.Instantiate("Player", spawnPos[idx], Quaternion.identity);
        //���콺 �����͸� ��Ȱ��ȭ
        Cursor.visible = false;
    }

    //�÷��̾��� ��ġ ���� �ʱ⸦ �����Ѵ�.
    public Transform trSpawnPosGroup;

    //spawn ��ġ�� ��Ƴ��� ����
    public Vector3[] spawnPos;

    private void SetSpawnPos()
    {
        //�ִ� �ο� ��ŭ spawnPos�� ������ �Ҵ�
        spawnPos = new Vector3[PhotonNetwork.CurrentRoom.MaxPlayers];
        //PhotonNetwork.CurrentRoom.MaxPlayers
        //������ 360������ ������ ������.
        float angle = 360 / spawnPos.Length;

        //�÷��̾ �����Ѵ�.
        for (int i = 0; i < spawnPos.Length; i++)
        {
            trSpawnPosGroup.Rotate(0, angle, 0);
            spawnPos[i] = trSpawnPosGroup.position + trSpawnPosGroup.forward * 5;
        }
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

    //������ player�� photonView �߰�
    public void AddPlayer(PhotonView pv)
    {
        listPlayer.Add(pv);

        // ��� �÷��̾ �����ߴٸ�
        if (listPlayer.Count == PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            ChangeTun();
        }
    }

    //���� Turn idx
    private int currTurnIdx = -1;

    private void ChangeTun()
    {
        //������ �ƴ϶�� �Լ��� ������
        if (PhotonNetwork.IsMasterClient == false) return;
        //�߻��� ��� Turn ����
        if (currTurnIdx != -1)
        {
            listPlayer[currTurnIdx].RPC(nameof(ChangeTun), RpcTarget.All, false);
        }

        currTurnIdx++;

        //���࿡ currTurnIdx�� 3�̸�
        currTurnIdx = currTurnIdx % listPlayer.Count;

        //if (currTurnIdx >= listPlayer.Count)
        //{
        //    currTurnIdx = 0;
        //}
        //currTurnInd�� 0���� �Ѵ�.
        //���� ��� Turn ����
        listPlayer[currTurnIdx].RPC(nameof(ChangeTun), RpcTarget.All, true);
    }

    //���ο� �ο��� �濡 ������ �� ȣ��Ǵ� �Լ�
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        print(newPlayer.NickName + "���Խ��ϴ�.");
    }
}