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

    //UI Canvas
    public GameObject myUI;

    //animator�� �����´�.
    public Animator anim;

    //���� ������ ����
    private float h, v;

    //���� ���̴�??
    private bool isJump = false;

    private void Start()
    {
        //Character Controller ��������
        cc = GetComponent<CharacterController>();

        //���࿡ ���� ���� player���
        if (photonView.IsMine)
        {
            //UI�� ��Ȱ��ȭ ����
            myUI.SetActive(false);
        }
        else
        {//�г��� ����
            nickName.text = photonView.Owner.NickName;
        }
    }

    private void Update()
    {
        //���� ���� �÷��̾���
        if (photonView.IsMine)
        {
            //.���࿡ ���콺 Ŀ���� Ȱ��ȭ �Ǿ� ������ �Լ��� ������
            if (Cursor.visible == true) return;
            // WASD Ű�� ������ �յ��¿�� �����̰� �ʹ�.
            h = Input.GetAxis("Horizontal");
            v = Input.GetAxis("Vertical");

            // ���� ������ �ʿ��ϴ�.

            Vector3 dirH = transform.right * h;
            Vector3 dirV = transform.forward * v;
            Vector3 dir = dirH + dirV;

            // �ӵ��� 1�� �����.
            dir.Normalize();

            // ���� ���� ����ִٸ�
            if (cc.isGrounded == true)
            {
                // yVelocity�� 0���� ����
                yVelocity = 0;

                //���࿡ ���� ���̶��
                if (isJump == true)
                {                //���� Ʈ���� �߻�
                    photonView.RPC(nameof(SetTriggerRpc), RpcTarget.All, "Land");

                    //���� �ƴ϶�� ����
                    isJump = false;
                }
            }

            // �����̽��ٸ� ������ ������ �ϰ� �ʹ�.
            if (Input.GetButtonDown("Jump"))
            {
                yVelocity = jumpPower;

                //���� Ʈ���� ����
                photonView.RPC(nameof(SetTriggerRpc), RpcTarget.All, "Jump");

                //���� ���̶�� ����
                isJump = true;
                SoundManager.instance.PlaySFX(SoundManager.ESfx.SFX_JUMP);
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

        //�ִϸ��̼� �Ķ���� �� ����
        anim.SetFloat("Horizontal", h);

        //
        anim.SetFloat("Vertical", v);

        //���� photonView ���ӸŴ����� �˷�����
        GameManager.Instance.AddPlayer(photonView);
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
            //h�� ������.
            stream.SendNext(h);
            //y�� ������.
            stream.SendNext(v);
        }
        //�� Player���
        else
        {
            //��ġ, ȸ���� ����
            receivePos = (Vector3)stream.ReceiveNext();
            receiveRot = (Quaternion)stream.ReceiveNext();
            //h�� ����
            h = (float)stream.ReceiveNext();
            //v�� ����
            v = (float)stream.ReceiveNext();
        }
    }

    [PunRPC]
    private void SetTriggerRpc(string parameter)
    {
        anim.SetTrigger(parameter);
    }
}