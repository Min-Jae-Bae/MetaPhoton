using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        //���� �÷��̾� ����
        PhotonNetwork.Instantiate("Player", new Vector3(0, 1, 0), Quaternion.identity);
    }
}