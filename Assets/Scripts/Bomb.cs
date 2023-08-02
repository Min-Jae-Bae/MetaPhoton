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
        //��� ������ ���� �ʹ�.
        transform.position += transform.forward * speed * Time.deltaTime;
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

        //���� ������Ű�� �ʹ�.
        Destroy(gameObject);
    }
}
