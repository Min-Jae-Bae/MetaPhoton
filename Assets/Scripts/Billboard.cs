using UnityEngine;

public class Billboard : MonoBehaviour
{
    private void Update()
    {
        //���� �չ����� ī�޶��� �� �������� ����.
        transform.forward = Camera.main.transform.forward;
    }
}