using UnityEngine;

public class BossSlashController : MonoBehaviour
{
    public float deleteTime = 0.2f;

    void Start()
    {
        Destroy(gameObject, deleteTime);
    }
}
