using UnityEngine;

public class Billboard : MonoBehaviour
{
    private void Update()
    {
        //나의 앞방향을 카메라의 앞 방향으로 설정.
        transform.forward = Camera.main.transform.forward;
    }
}