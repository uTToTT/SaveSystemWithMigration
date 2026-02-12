using UnityEngine;

public class ZRotator : MonoBehaviour
{
    [SerializeField] private float _speed;

    private void Update()
    {
        ZRotate();
    }

    public void ZRotate() => transform.Rotate(0f, 0f, _speed * Time.deltaTime);
}
