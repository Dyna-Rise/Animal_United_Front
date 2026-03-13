using System.Collections;
using UnityEngine;

public class BossController : MonoBehaviour
{
    GameObject player; //検索対象
    public float range = 3.0f; //索敵範囲

    public float minX;
    public float maxX;
    public float outDisplayY;
    public float topY;
    float currentY;
    public float bottomY;

    public float interval = 5.0f;
    bool outDisplay;

    public GameObject bossSlash;
    public GameObject bossShot;
    public float speed = 3.0f;

    Coroutine hangingCoroutine;
    Coroutine displayOutCoroutine;

    void Start()
    {
        outDisplay = true;
        currentY = topY;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if(!outDisplay && hangingCoroutine == null)
        {
            hangingCoroutine = StartCoroutine(HangingCol());
        }

        if (!outDisplay && hangingCoroutine != null)
        {
            transform.position = Vector3.Lerp(
                transform.position, new Vector3(transform.position.x, currentY, 0), Time.deltaTime * 3);
        }

        if (outDisplay && displayOutCoroutine == null)
        {
            displayOutCoroutine = StartCoroutine(DisplayOutCol());
        }

        if (outDisplay && displayOutCoroutine != null)
        {
            transform.position = Vector3.Lerp(
                transform.position, new Vector3(transform.position.x,outDisplayY,0),Time.deltaTime * 3);
        }
    }

    IEnumerator HangingCol()
    {
        yield return new WaitForSeconds(2); //移動時間
        BossAttack();
        yield return new WaitForSeconds(interval);
        hangingCoroutine = null;
        outDisplay = true;
    }

    IEnumerator DisplayOutCol()
    {
        yield return new WaitForSeconds(2);//移動時間
        yield return new WaitForSeconds(interval);
        float x = Random.Range(minX, maxX + 1);
        transform.position = new Vector3(x, transform.position.y, 0);
        int y = Random.Range(0, 2);
        if (y == 0) currentY = topY;
        else currentY = bottomY;
        displayOutCoroutine = null;
        outDisplay = false;
    }


    void BossAttack()
    {
        //プレイヤーとの直線距離を取得
        float dis = Vector3.Distance(player.transform.position, transform.position);

        //範囲内なら
        if (dis < range)
        {
            BossSlash();
        }
        else
        {
            BossShoot();
        }
    }

    void BossSlash()
    {
        Debug.Log("接近");
        float dx = player.transform.position.x - transform.position.x;
        float dy = player.transform.position.y - transform.position.y;
        float angle = Mathf.Atan2(dy, dx);
        float angleZ = angle * Mathf.Rad2Deg;
        float offsetX = Mathf.Cos(angle);
        float offsetY = Mathf.Sin(angle);
        Vector3 offset = new Vector3(offsetX, offsetY, 0) * 2;
        Instantiate(
            bossSlash,
            transform.position + offset,
            Quaternion.Euler(0, 0, angleZ)
            );
    }

    void BossShoot()
    {
        Debug.Log("ショット");
        float dx = player.transform.position.x - transform.position.x;
        float dy = player.transform.position.y - transform.position.y;
        float angle = Mathf.Atan2(dy, dx);
        float angleZ = angle * Mathf.Rad2Deg;
        float offsetX = Mathf.Cos(angle);
        float offsetY = Mathf.Sin(angle);
        Vector3 offset = new Vector3(offsetX, offsetY, 0) * 2;
        GameObject obj = Instantiate(
            bossShot,
            transform.position + offset,
            Quaternion.Euler(0, 0, angleZ)
            );

        obj.GetComponent<Rigidbody>().AddForce(new Vector3(offsetX, offsetY, 0) * speed, ForceMode.Impulse);

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
