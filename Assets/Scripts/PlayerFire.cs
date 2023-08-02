using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    // 폭탄 공장
    public GameObject bombFactory;
    public GameObject fragmentFactory;

    void Update()
    {
        //1번 키를 누르면
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //폭탄공장에서 폭탄을 만든다.
            GameObject bomb = Instantiate(bombFactory);
            //만들어진 폭탄을 카메라 앞방향으로 1만큼 떨어진 지점에 놓는다.
            bomb.transform.position = Camera.main.transform.position + Camera.main.transform.forward;
            //만들어진 총알의 앞 방향을 카메라가 보는 방향으로 설정
            bomb.transform.forward = Camera.main.transform.forward;
        }


        //2번키 누르면
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            //카메라 위치, 카메라 앞방향으로 Ray를 만들자
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            RaycastHit hitInfo;
            //만약에 레이를 발사해서 부딪힌 곳이 있다면
            if (Physics.Raycast(ray, out hitInfo))
            {
                GameObject fragment = Instantiate(fragmentFactory);
                //그 위치에 파편효과공장에서 파편효과를 만든다.
                fragment.transform.position = hitInfo.point;
                //만들어진 파편효과를 부딪힌 위치에 놓는다.
                fragment.transform.forward = hitInfo.normal;
                //2초 뒤에 파편효과를 파괴하자
                Destroy(fragment, 2);
            }
        }
    }
}
