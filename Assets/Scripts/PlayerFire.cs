using Photon.Pun;
using UnityEngine;

public class PlayerFire : MonoBehaviourPun
{
    // ��ź ����
    public GameObject bombFactory;

    public GameObject fragmentFactory;

    private void Start()
    {
        //���� ���� �÷��̾ �ƴ� ��
        if (photonView.IsMine) enabled = false;
        //playerFire ������Ʈ�� ��Ȱ��ȭ �Ѵ�
    }

    private void Update()
    {
        //.���࿡ ���콺 Ŀ���� Ȱ��ȭ �Ǿ� ������ �Լ��� ������
        if (Cursor.visible == true) return;
        //1�� Ű�� ������

        //���࿡ ���� ���� �� �� �ִٸ� ���� �߻��Ѵ�.
        if (canFire == false) return;

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //FireBulletByInstantiate();
            Vector3 pos = Camera.main.transform.position + Camera.main.transform.forward;

            Vector3 forward = Camera.main.transform.forward;

            photonView.RPC(nameof(FireBulletByRpc), RpcTarget.All, pos, forward);
        }

        //2��Ű ������
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            photonView.RPC(nameof(FireRayByRpc), RpcTarget.All, Camera.main.transform.position, Camera.main.transform.forward);
        }
    }

    private void FireBulletByInstantiate()
    {
        Vector3 pos = Camera.main.transform.position + Camera.main.transform.forward;

        Quaternion rot = Camera.main.transform.rotation;
        //��ź���忡�� ��ź�� �����.
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
        //���࿡ ���̸� �߻��ؼ� �ε��� ���� �ִٸ�
        if (Physics.Raycast(ray, out hitInfo))
        {
            GameObject fragment = Instantiate(fragmentFactory);
            //�� ��ġ�� ����ȿ�����忡�� ����ȿ���� �����.
            fragment.transform.position = hitInfo.point;
            //������� ����ȿ���� �ε��� ��ġ�� ���´�.
            fragment.transform.forward = hitInfo.normal;
            //2�� �ڿ� ����ȿ���� �ı�����
            Destroy(fragment, 2);

            //���࿡ �������� �̸��� �÷��̾ �����ϰ� �ִٸ�
            if (hitInfo.transform.gameObject.name.Contains("Player"))
            {
                //�÷��̾ ������ �ִ� PlayerHP ������Ʈ�� ��������
                PlayerHP hp = hitInfo.transform.GetComponent<PlayerHP>();
                //������ ������Ʈ�� updateHP �Լ��� �����Ѵ�.
            }
        }
        //���� �ѱ��.
    }

    //���� ���� �� �� �ִ��� �Ǵ�
    private bool canFire;

    [PunRPC]
    private void ChangeTurnRpc(bool fire)
    {
        canFire = fire;
    }
}