using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerFire : MonoBehaviourPun
{
    // 폭탄 공장
    public GameObject bombFactory;
    public GameObject fragmentFactory;

    void Start()
    {
        //내가 만든 플레이어가 아닐 때
        if (photonView.IsMine) enabled = false;
        //playerFire 컴포넌트를 비활성화 한다
    }

    void Update()
    {
        //1번 키를 누르면
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //FireBulletByInstantiate();
            Vector3 pos = Camera.main.transform.position + Camera.main.transform.forward;

            Vector3 forward = Camera.main.transform.forward;

            photonView.RPC(nameof(FireBulletByRpc), RpcTarget.All, pos, forward);
        }


        //2번키 누르면
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {

            photonView.RPC(nameof(FireRayByRpc), RpcTarget.All, Camera.main.transform.position, Camera.main.transform.forward);
        }
    }

    void FireBulletByInstantiate()
    {
        Vector3 pos = Camera.main.transform.position + Camera.main.transform.forward;

        Quaternion rot = Camera.main.transform.rotation;
        //폭탄공장에서 폭탄을 만든다.
        GameObject bomb = PhotonNetwork.Instantiate("Bomb", pos, rot);

    }

    [PunRPC]
    void FireBulletByRpc(Vector3 firePos, Vector3 fireForward)
    {
        GameObject bomb = Instantiate(bombFactory);
        bomb.transform.position = firePos;
        bomb.transform.forward = fireForward;
    }

    [PunRPC]
    void FireRayByRpc(Vector3 firePos, Vector3 fireForward)
    {
        Ray ray = new Ray(firePos, fireForward);
        RaycastHit hitInfo;
        //만약에 레이를 발사해서 부딪힌 곳이 있다면
        if (Physics.Raycast(ray, out hitInfo))
        {
            GameObject fragment = Instantiate(fragmentFactory);
            //그 위치에 파편효과공장에서 파편효과를 만든다.
            fragment.transform.position = hitInfo.point;
            //만들어진 파편효과를 부딪힌 위치에 놓는다.
            fragment.transform.forward = hitInfo.normal;
            //2초 뒤에 파편효과를 파괴하자
            Destroy(fragment, 2);
        }
    }
}
