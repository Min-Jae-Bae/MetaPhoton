using Photon.Pun;
using TMPro;
using UnityEngine;

public class PlayerMove : MonoBehaviourPun, IPunObservable
{
    // ���� ���� �ӵ��� �ʿ��Ѵ�.
    public float speed = 5f;

    // ���� �Ŀ�
    private float jumpPower = 5;

    // �߷�
    private float gravity = -9.81f;

    // y�� �ӷ�
    private float yVelocity = 0;

    private Vector3 receivePos;
    private Quaternion receiveRot = Quaternion.identity;

    private CharacterController cc;
    private float lerpSpeed = 50;

    public TextMeshProUGUI nickName;

    private void Start()
    {
        //Character Controller ��������
        cc = GetComponent<CharacterController>();

        //�г��� ����
        nickName.text = photonView.Owner.NickName;
    }

    private void Update()
    {
        //���� ���� �÷��̾���
        if (photonView.IsMine)
        {
            // WASD Ű�� ������ �յ��¿�� �����̰� �ʹ�.
            float hAxis = Input.GetAxis("Horizontal");
            float vAxis = Input.GetAxis("Vertical");

            // ���� ������ �ʿ��ϴ�.

            Vector3 dirH = transform.right * hAxis;
            Vector3 dirV = transform.forward * vAxis;
            Vector3 dir = dirH + dirV;

            // �ӵ��� 1�� �����.
            dir.Normalize();

            // ���� ���� ����ִٸ�
            if (cc.isGrounded == true)
            {
                // yVelocity�� 0���� ����
                yVelocity = 0;
            }

            // �����̽��ٸ� ������ ������ �ϰ� �ʹ�.
            if (Input.GetButtonDown("Jump"))
            {
                yVelocity = jumpPower;
            }
            yVelocity += gravity * Time.deltaTime;
            dir.y = yVelocity;

            // �÷��̾ �����δ�.
            cc.Move(dir * speed * Time.deltaTime);
        }
        //���� Player�� �ƴ϶��
        else
        {
            //��ġ ����
            transform.position = Vector3.Lerp(transform.position, receivePos, lerpSpeed * Time.deltaTime);

            //ȸ���� ����
            transform.rotation = Quaternion.Lerp(transform.rotation, receiveRot, lerpSpeed * Time.deltaTime);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //�� player��
        if (stream.IsWriting)
        {
            //���� ��ġ ���� ������
            stream.SendNext(transform.position);
            //���� ȸ������ ������.
            stream.SendNext(transform.rotation);
        }
        //�� Player���
        else
        {
            //��ġ, ȸ���� ����
            receivePos = (Vector3)stream.ReceiveNext();
            receiveRot = (Quaternion)stream.ReceiveNext();
        }
    }
}