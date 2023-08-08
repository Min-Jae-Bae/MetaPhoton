using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ChatManager : MonoBehaviourPun
{
    public InputField chatInput;

    public GameObject chatItemFactory;
    public RectTransform rtContent;

    // scrollView�� rectTransform
    public RectTransform rtScrollVeiw;

    // ä���� �߰��Ǳ� ���� Content H�� ���� ������ �ִ� ����
    private float prevContentH;

    //�г��� ����
    public Color nickNameColor;

    private void Start()
    {
        //�г��� ���� �����ϰ� ����
        nickNameColor = new Color32(
            (byte)Random.Range(0, 256),
            (byte)Random.Range(0, 256),
            (byte)Random.Range(0, 256),
            255);
        //����Ű�� ������ inputField�� �ִ� �ؽ�Ʈ ���� �˷��ִ� �Լ� ���
        chatInput.onSubmit.AddListener(OnSubmit);

        //InputField�� ������ ���� �� ������ ȣ�����ִ� �Լ� ���
        chatInput.onValueChanged.AddListener(OnValueChanged);

        //InputField�� Focusing�� ������� �� ȣ�����ִ� �Լ� ���
        chatInput.onEndEdit.AddListener(OnEndEdit);
    }

    public void OnSubmit(string s)
    {
        //s�� ���̰� 0 �̶�� �Լ��� ������
        if (s.Length == 0) return;
        //���ο� ä���� �߰��Ǳ� ���� content�� H ���� ����
        prevContentH = rtContent.sizeDelta.y;

        string chat = "<color=#" + ColorUtility.ToHtmlStringRGB(nickNameColor) + ">" + PhotonNetwork.NickName + "</color>" + " : " + s;

        //Rpc �Լ��� ��� ������� ä�� ������ ����
        photonView.RPC(nameof(AddChatRpc), RpcTarget.All, chat);
        //Input���� �ʱ�ȭ ����
        chatInput.text = "";
        //chatInput�� Ȱ��ȭ
        chatInput.ActivateInputField();
    }

    [PunRPC]
    private void AddChatRpc(string chat)
    {
        GameObject ci = Instantiate(chatItemFactory);
        //������� �������� �θ� content�� �Ѵ�.
        ci.transform.SetParent(rtContent);
        //������� �����ۿ��� Text ������Ʈ�� �����´�.
        ChatItem item = ci.GetComponent<ChatItem>();
        item.SetText(chat);

        //�ڵ����� content�� �� ������ ������ ���
        StartCoroutine(AutoScrollBottom());
    }

    private IEnumerator AutoScrollBottom()
    {
        yield return 0;
        //��ũ�Ѻ��� H ���� content�� H ���� ũ�ٸ� (��ũ���� ������ ���¶��)
        if (rtContent.sizeDelta.y > rtScrollVeiw.sizeDelta.y)
        {
            //������ �ٴڿ� ����־��ٸ�
            if (prevContentH - rtScrollVeiw.sizeDelta.y <= rtContent.anchoredPosition.y)
            {
                //content�� y ���� �缳���Ѵ�.
                rtContent.anchoredPosition = new Vector2(0, rtContent.sizeDelta.y - rtScrollVeiw.sizeDelta.y);
            }
        }
    }

    public void OnValueChanged(string s)
    {
        print("OnValueChanged :" + s);
    }

    public void OnEndEdit(string s)
    {
        print("OnEndEdit :" + s);
    }
}