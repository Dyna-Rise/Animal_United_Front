using UnityEngine;

public class BossShotController : MonoBehaviour
{
    public float deleteTime = 5.0f;

    void Start()
    {
        Destroy(gameObject, deleteTime);
    }
}
