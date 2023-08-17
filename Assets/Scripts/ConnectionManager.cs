using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class ConnectionManager : MonoBehaviourPunCallbacks
{
    //�г��� ��ǲ �ʵ�
    public InputField inputNickName;

    //Connect Button
    public Button btnConnect;

    private void Start()
    {

        SoundManager.instance.PlayBGM(SoundManager.EBgm.BGM_CONNECTION);
        //�г����� ������ ����� �� ȣ��Ǵ� �Լ� ���
        inputNickName.onValueChanged.AddListener(onvalueChanged);

        //InputNickName���� ���� ���� �� ȣ��Ǵ� �Լ� ���
        inputNickName.onSubmit.AddListener(
            (string s) =>
            {
                //onClickConnect ȣ��
                if (btnConnect.interactable)
                {
                    OnClickConnect();
                }
            }
        );
        //��ư ��Ȱ��ȭ
        btnConnect.interactable = false;
    }

    private void onvalueChanged(string s)
    {
        //���࿡ s�� ���̰� 0���� ũ��
        btnConnect.interactable = s.Length > 0;
    }

    public void OnClickConnect()
    {

        SoundManager.instance.PlaySFX(SoundManager.ESfx.SFX_BUTTON);
        //���� ���� ��û
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        //�г��� ����
        PhotonNetwork.NickName = inputNickName.text;

        //Ư�� �κ� ���� ����
        //TypedLobby typedLobby = new TypedLobby("Meta Lobby", LobbyType.Default);

        //�⺻ �κ� ���� ��û
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        print(System.Reflection.MethodBase.GetCurrentMethod().Name);
        //�κ� ������ �̵�

        PhotonNetwork.LoadLevel("LobbyScene");
    }
}