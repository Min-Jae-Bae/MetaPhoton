using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    //총알을 날라가게 하고 싶다.
    private float speed = 10f;
    // 폭발효과 공장
    public GameObject exploFactory;

    void Update()
    {
        //계속 앞으로 가고 싶다.
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    //트리거 발생시 삭제 시키고 싶다
    void OnTriggerEnter(Collider other)
    {
        //폭발효과공장에서 폭발효과를 만들자
        GameObject explo = Instantiate(exploFactory);
        //만든 효과를 나의 위치에 놓자
        explo.transform.position = transform.position;
        // 만든 효과에서 파티클 시스템을 가져오자
        ParticleSystem ps = explo.GetComponent<ParticleSystem>();
        // 가져온 파티클의 기능이 play 를 실행하자
        ps.Play();

        //나를 삭제시키고 싶다.
        Destroy(gameObject);
    }
}
