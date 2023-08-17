using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class ConnectionManager : MonoBehaviourPunCallbacks
{
    //닉네임 인풋 필드
    public InputField inputNickName;

    //Connect Button
    public Button btnConnect;

    private void Start()
    {

        SoundManager.instance.PlayBGM(SoundManager.EBgm.BGM_CONNECTION);
        //닉네임의 내용이 변경될 때 호출되는 함수 등록
        inputNickName.onValueChanged.AddListener(onvalueChanged);

        //InputNickName에서 엔터 쳤을 때 호출되는 함수 등록
        inputNickName.onSubmit.AddListener(
            (string s) =>
            {
                //onClickConnect 호출
                if (btnConnect.interactable)
                {
                    OnClickConnect();
                }
            }
        );
        //버튼 비활성화
        btnConnect.interactable = false;
    }

    private void onvalueChanged(string s)
    {
        //만약에 s의 길이가 0보다 크면
        btnConnect.interactable = s.Length > 0;
    }

    public void OnClickConnect()
    {

        SoundManager.instance.PlaySFX(SoundManager.ESfx.SFX_BUTTON);
        //서버 접속 요청
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        //닉네임 설정
        PhotonNetwork.NickName = inputNickName.text;

        //특정 로비 정보 셋팅
        //TypedLobby typedLobby = new TypedLobby("Meta Lobby", LobbyType.Default);

        //기본 로비 진입 요청
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        print(System.Reflection.MethodBase.GetCurrentMethod().Name);
        //로비 씬으로 이동

        PhotonNetwork.LoadLevel("LobbyScene");
    }
}