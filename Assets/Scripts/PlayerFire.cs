using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    // ��ź ����
    public GameObject bombFactory;
    public GameObject fragmentFactory;

    void Update()
    {
        //1�� Ű�� ������
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //��ź���忡�� ��ź�� �����.
            GameObject bomb = Instantiate(bombFactory);
            //������� ��ź�� ī�޶� �չ������� 1��ŭ ������ ������ ���´�.
            bomb.transform.position = Camera.main.transform.position + Camera.main.transform.forward;
            //������� �Ѿ��� �� ������ ī�޶� ���� �������� ����
            bomb.transform.forward = Camera.main.transform.forward;
        }


        //2��Ű ������
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            //ī�޶� ��ġ, ī�޶� �չ������� Ray�� ������
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            RaycastHit hitInfo;
            //���࿡ ���̸� �߻��ؼ� �ε��� ���� �ִٸ�
            if (Physics.Raycast(ray, out hitInfo))
            {
                GameObject fragment = Instantiate(fragmentFactory);
                //�� ��ġ�� ����ȿ�����忡�� ����ȿ���� �����.
                fragment.transform.position = hitInfo.point;
                //������� ����ȿ���� �ε��� ��ġ�� ���´�.
                fragment.transform.forward = hitInfo.normal;
                //2�� �ڿ� ����ȿ���� �ı�����
                Destroy(fragment, 2);
            }
        }
    }
}
