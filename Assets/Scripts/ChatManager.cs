using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ChatManager : MonoBehaviourPun
{
    public InputField chatInput;

    public GameObject chatItemFactory;
    public RectTransform rtContent;

    // scrollView의 rectTransform
    public RectTransform rtScrollVeiw;

    // 채팅이 추가되기 전에 Content H의 값을 가지고 있는 변수
    private float prevContentH;

    //닉네임 색상
    public Color nickNameColor;

    private void Start()
    {
        //닉네임 색상 랜덤하게 설정
        nickNameColor = new Color32(
            (byte)Random.Range(0, 256),
            (byte)Random.Range(0, 256),
            (byte)Random.Range(0, 256),
            255);
        //엔터키를 누르면 inputField에 있는 텍스트 내용 알려주는 함수 등록
        chatInput.onSubmit.AddListener(OnSubmit);

        //InputField의 내용이 변경 될 때마다 호출해주는 함수 등록
        chatInput.onValueChanged.AddListener(OnValueChanged);

        //InputField의 Focusing이 사라졌을 때 호출해주는 함수 등록
        chatInput.onEndEdit.AddListener(OnEndEdit);
    }

    public void OnSubmit(string s)
    {
        //s의 길이가 0 이라면 함수를 나가라
        if (s.Length == 0) return;
        //새로운 채팅이 추가되기 전의 content의 H 값을 저장
        prevContentH = rtContent.sizeDelta.y;

        string chat = "<color=#" + ColorUtility.ToHtmlStringRGB(nickNameColor) + ">" + PhotonNetwork.NickName + "</color>" + " : " + s;

        //Rpc 함수로 모든 사람한테 채팅 내용을 전달
        photonView.RPC(nameof(AddChatRpc), RpcTarget.All, chat);
        //Input값을 초기화 하자
        chatInput.text = "";
        //chatInput을 활성화
        chatInput.ActivateInputField();
    }

    [PunRPC]
    private void AddChatRpc(string chat)
    {
        GameObject ci = Instantiate(chatItemFactory);
        //만들어진 아이템의 부모를 content로 한다.
        ci.transform.SetParent(rtContent);
        //만들어진 아이템에서 Text 컴포넌트를 가져온다.
        ChatItem item = ci.GetComponent<ChatItem>();
        item.SetText(chat);

        //자동으로 content를 맨 밑으로 내리는 기능
        StartCoroutine(AutoScrollBottom());
    }

    private IEnumerator AutoScrollBottom()
    {
        yield return 0;
        //스크롤뷰의 H 보다 content의 H 값이 크다면 (스크롤이 가능한 상태라면)
        if (rtContent.sizeDelta.y > rtScrollVeiw.sizeDelta.y)
        {
            //이전에 바닥에 닿아있었다면
            if (prevContentH - rtScrollVeiw.sizeDelta.y <= rtContent.anchoredPosition.y)
            {
                //content의 y 값을 재설정한다.
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