using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class ChatManager : MonoBehaviourPun
{
    public InputField chatInput;

    public GameObject chatItemFactory;
    public RectTransform chatRectTransform;

    private void Start()
    {
        //����Ű�� ������ inputField�� �ִ� �ؽ�Ʈ ���� �˷��ִ� �Լ� ���
        chatInput.onSubmit.AddListener(OnSubmit);

        //InputField�� ������ ���� �� ������ ȣ�����ִ� �Լ� ���
        chatInput.onValueChanged.AddListener(OnValueChanged);

        //InputField�� Focusing�� ������� �� ȣ�����ִ� �Լ� ���
        chatInput.onEndEdit.AddListener(OnEndEdit);
    }

    public void OnSubmit(string s)
    {
        //�� �������� �����.
        GameObject ci = Instantiate(chatItemFactory);
        //������� �������� �θ� content�� �Ѵ�.
        ci.transform.SetParent(chatRectTransform);
        //������� �����ۿ��� Text ������Ʈ�� �����´�.
        ChatItem item = ci.GetComponent<ChatItem>();
        //"<color=#fff00> ���ϴ� ���� </color>"
        //�г����� �ٿ��� ä�� ������ ������.
        string chat = "<color=#" + ColorUtility.ToHtmlStringRGB(Color.blue) + ">" + PhotonNetwork.NickName + "</color>" + " : " + s;
        item.SetText(chat);

        //Input���� �ʱ�ȭ ����
        chatInput.text = "";
        //chatInput�� Ȱ��ȭ ����
        chatInput.ActivateInputField();
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