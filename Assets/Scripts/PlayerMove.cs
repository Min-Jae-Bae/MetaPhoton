using Photon.Pun;
using TMPro;
using UnityEngine;

public class PlayerMove : MonoBehaviourPun, IPunObservable
{
    // 가기 위한 속도가 필요한다.
    public float speed = 5f;

    // 점프 파워
    private float jumpPower = 5;

    // 중력
    private float gravity = -9.81f;

    // y축 속력
    private float yVelocity = 0;

    private Vector3 receivePos;
    private Quaternion receiveRot = Quaternion.identity;

    private CharacterController cc;
    private float lerpSpeed = 50;

    public TextMeshProUGUI nickName;

    //UI Canvas
    public GameObject myUI;

    //animator를 가져온다.
    public Animator anim;

    //가로 방향을 결정
    private float h, v;

    //점프 중이니??
    private bool isJump = false;

    private void Start()
    {
        //Character Controller 가져오자
        cc = GetComponent<CharacterController>();

        //만약에 내가 만든 player라면
        if (photonView.IsMine)
        {
            //UI를 비활성화 하자
            myUI.SetActive(false);
        }
        else
        {//닉네임 설정
            nickName.text = photonView.Owner.NickName;
        }
    }

    private void Update()
    {
        //내가 만든 플레이어라면
        if (photonView.IsMine)
        {
            //.만약에 마우스 커서가 활성화 되어 있으면 함수를 나가자
            if (Cursor.visible == true) return;
            // WASD 키를 누르면 앞뒤좌우로 움직이고 싶다.
            h = Input.GetAxis("Horizontal");
            v = Input.GetAxis("Vertical");

            // 가는 방향이 필요하다.

            Vector3 dirH = transform.right * h;
            Vector3 dirV = transform.forward * v;
            Vector3 dir = dirH + dirV;

            // 속도를 1로 만든다.
            dir.Normalize();

            // 만약 땅에 닿아있다면
            if (cc.isGrounded == true)
            {
                // yVelocity를 0으로 하자
                yVelocity = 0;

                //만약에 점프 중이라면
                if (isJump == true)
                {                //착지 트리거 발생
                    photonView.RPC(nameof(SetTriggerRpc), RpcTarget.All, "Land");

                    //점프 아니라고 설정
                    isJump = false;
                }
            }

            // 스페이스바를 누르면 점프를 하고 싶다.
            if (Input.GetButtonDown("Jump"))
            {
                yVelocity = jumpPower;

                //점프 트리거 실행
                photonView.RPC(nameof(SetTriggerRpc), RpcTarget.All, "Jump");

                //점프 중이라고 설정
                isJump = true;
                SoundManager.instance.PlaySFX(SoundManager.ESfx.SFX_JUMP);
            }
            yVelocity += gravity * Time.deltaTime;
            dir.y = yVelocity;

            // 플레이어를 움직인다.
            cc.Move(dir * speed * Time.deltaTime);
        }
        //나의 Player가 아니라면
        else
        {
            //위치 보정
            transform.position = Vector3.Lerp(transform.position, receivePos, lerpSpeed * Time.deltaTime);

            //회전값 보정
            transform.rotation = Quaternion.Lerp(transform.rotation, receiveRot, lerpSpeed * Time.deltaTime);
        }

        //애니메이션 파라미터 값 전달
        anim.SetFloat("Horizontal", h);

        //
        anim.SetFloat("Vertical", v);

        //나의 photonView 게임매니저에 알려주자
        GameManager.Instance.AddPlayer(photonView);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //내 player면
        if (stream.IsWriting)
        {
            //나의 위치 값을 보낸다
            stream.SendNext(transform.position);
            //나의 회전값을 보낸다.
            stream.SendNext(transform.rotation);
            //h값 보낸다.
            stream.SendNext(h);
            //y값 보낸다.
            stream.SendNext(v);
        }
        //내 Player라면
        else
        {
            //위치, 회전을 받자
            receivePos = (Vector3)stream.ReceiveNext();
            receiveRot = (Quaternion)stream.ReceiveNext();
            //h값 받자
            h = (float)stream.ReceiveNext();
            //v값 받자
            v = (float)stream.ReceiveNext();
        }
    }

    [PunRPC]
    private void SetTriggerRpc(string parameter)
    {
        anim.SetTrigger(parameter);
    }
}