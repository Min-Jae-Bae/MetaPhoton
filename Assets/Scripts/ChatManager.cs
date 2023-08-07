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
        //엔터키를 누르면 inputField에 있는 텍스트 내용 알려주는 함수 등록
        chatInput.onSubmit.AddListener(OnSubmit);

        //InputField의 내용이 변경 될 때마다 호출해주는 함수 등록
        chatInput.onValueChanged.AddListener(OnValueChanged);

        //InputField의 Focusing이 사라졌을 때 호출해주는 함수 등록
        chatInput.onEndEdit.AddListener(OnEndEdit);
    }

    public void OnSubmit(string s)
    {
        //쳇 아이템을 만든다.
        GameObject ci = Instantiate(chatItemFactory);
        //만들어진 아이템의 부모를 content로 한다.
        ci.transform.SetParent(chatRectTransform);
        //만들어진 아이템에서 Text 컴포넌트를 가져온다.
        ChatItem item = ci.GetComponent<ChatItem>();
        //"<color=#fff00> 원하는 내용 </color>"
        //닉네임을 붙여서 채팅 내용을 만들자.
        string chat = "<color=#" + ColorUtility.ToHtmlStringRGB(Color.blue) + ">" + PhotonNetwork.NickName + "</color>" + " : " + s;
        item.SetText(chat);

        //Input값을 초기화 하자
        chatInput.text = "";
        //chatInput을 활성화 하자
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