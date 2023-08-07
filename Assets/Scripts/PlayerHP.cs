using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    //�ִ� HP
    private float maxHP = 100;

    //���� HP
    private float currHP = 0;

    //HPBar
    public Image hpBar;

    private void Start()
    // ���� HP�� �ִ� HP�� ����
    {
        currHP = maxHP;
    }

    // ������ �¾��� �� HP�� �ٿ��ִ� �Լ�
    public void UpdateHP(float damage)
    {
        //���� HP�� damage ��ŭ �ٿ��ش�.
        currHP += damage;
        hpBar.fillAmount = currHP / maxHP;
    }
}