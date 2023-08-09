using Photon.Pun;
using UnityEngine;

public class playerRot : MonoBehaviourPun
{
    // ���� ��
    private float rotX, rotY;

    private float speed = 200f;
    public Transform trCam;

    private void Start()
    {
        //���� ������ �÷��̾� �϶��� ī�޶� Ȱ��ȭ ����
        if (photonView.IsMine)
        {
            trCam.GetChild(0).gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        // ������ �ƴ� �� ������.
        if (!photonView.IsMine) return;

        //���콺�� �����ӵ��� �÷��̸� �¿� ȸ���ϰ�
        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");

        // ���� �����Ѵ�.
        rotX += my * speed * Time.deltaTime;
        rotY += mx * speed * Time.deltaTime;

        //�¿� ȸ������ �����ϰ� �ʹ�.
        rotX = Mathf.Clamp(rotX, -75, 75);
        // ȸ���ӵ� ���� �ش�.
        transform.localEulerAngles = new Vector3(0, rotY, 0);
        trCam.transform.localEulerAngles = new Vector3(-rotX, 0, 0);
    }
}