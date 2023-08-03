using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bomb : MonoBehaviour
{
    //�Ѿ��� ���󰡰� �ϰ� �ʹ�.
    private float speed = 10f;
    // ����ȿ�� ����
    public GameObject exploFactory;

    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
        //���� �� �Ѿ˸� �����̰� �ϰ� �ʹ�.
        //��� ������ ���� �ʹ�.
 /*       if (photonView.IsMine)
        {
            transform.position += transform.forward * speed * Time.deltaTime; 
        }*/
    }

    //Ʈ���� �߻��� ���� ��Ű�� �ʹ�
    void OnTriggerEnter(Collider other)
    {
        //����ȿ�����忡�� ����ȿ���� ������
        GameObject explo = Instantiate(exploFactory);
        //���� ȿ���� ���� ��ġ�� ����
        explo.transform.position = transform.position;
        // ���� ȿ������ ��ƼŬ �ý����� ��������
        ParticleSystem ps = explo.GetComponent<ParticleSystem>();
        // ������ ��ƼŬ�� ����� play �� ��������
        ps.Play();

        Destroy(gameObject);
/*        if (photonView.IsMine)
        {
            //���� ������Ű�� �ʹ�.
            PhotonNetwork.Destroy(gameObject); 
        }
*/
    }
}
