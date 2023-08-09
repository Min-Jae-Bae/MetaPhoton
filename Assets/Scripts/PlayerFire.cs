using Photon.Pun;
using UnityEngine;

public class PlayerFire : MonoBehaviourPun
{
    // 폭탄 공장
    public GameObject bombFactory;

    public GameObject fragmentFactory;

    private void Start()
    {
        //내가 만든 플레이어가 아닐 때
        if (photonView.IsMine) enabled = false;
        //playerFire 컴포넌트를 비활성화 한다
    }

    private void Update()
    {
        //.만약에 마우스 커서가 활성화 되어 있으면 함수를 나가자
        if (Cursor.visible == true) return;
        //1번 키를 누르면

        //만약에 내가 총을 쏠 수 있다면 총을 발사한다.
        if (canFire == false) return;

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

    private void FireBulletByInstantiate()
    {
        Vector3 pos = Camera.main.transform.position + Camera.main.transform.forward;

        Quaternion rot = Camera.main.transform.rotation;
        //폭탄공장에서 폭탄을 만든다.
        GameObject bomb = PhotonNetwork.Instantiate("Bomb", pos, rot);
    }

    [PunRPC]
    private void FireBulletByRpc(Vector3 firePos, Vector3 fireForward)
    {
        GameObject bomb = Instantiate(bombFactory);
        bomb.transform.position = firePos;
        bomb.transform.forward = fireForward;
    }

    [PunRPC]
    private void FireRayByRpc(Vector3 firePos, Vector3 fireForward)
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

            //만약에 맞은놈의 이름이 플레이어를 포함하고 있다면
            if (hitInfo.transform.gameObject.name.Contains("Player"))
            {
                //플레이어가 가지고 있는 PlayerHP 컴포넌트를 가져오자
                PlayerHP hp = hitInfo.transform.GetComponent<PlayerHP>();
                //가져온 컴포넌트의 updateHP 함수를 실행한다.
            }
        }
        //턴을 넘긴다.
    }

    //내가 총을 쏠 수 있는지 판단
    private bool canFire;

    [PunRPC]
    private void ChangeTurnRpc(bool fire)
    {
        canFire = fire;
    }
}