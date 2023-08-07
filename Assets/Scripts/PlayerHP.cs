using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    //최대 HP
    private float maxHP = 100;

    //현재 HP
    private float currHP = 0;

    //HPBar
    public Image hpBar;

    private void Start()
    // 현재 HP를 최대 HP로 설정
    {
        currHP = maxHP;
    }

    // 데미지 맞았을 때 HP를 줄여주는 함수
    public void UpdateHP(float damage)
    {
        //현재 HP를 damage 만큼 줄여준다.
        currHP += damage;
        hpBar.fillAmount = currHP / maxHP;
    }
}