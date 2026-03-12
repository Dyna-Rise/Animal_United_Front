using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2Controller : MonoBehaviour
{
    GameObject player; //Player情報


    [Header("投擲物・発射位置")]
    public GameObject throwObject;
    public GameObject gate;

    [Header("投擲間隔・角度・パワー")]
    public float interval = 5.0f;
    public float angle = 135.0f;
    public float thowPower = 10.0f;

    [Header("索敵範囲")]
    public float range = 5.0f;


    //投擲コルーチン
    Coroutine throwingCoroutine;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        
    }

    // Update is called once per frame
    void Update()
    {
        if(throwingCoroutine == null)
        {
            throwingCoroutine = StartCoroutine(ThrowingCol());
        }
    }

    IEnumerator ThrowingCol()
    {
        yield return new WaitForSeconds(interval);
        Throwing();
        throwingCoroutine = null;
    }

    void Throwing()
    {
        GameObject obj = Instantiate(
            throwObject,
            gate.transform.position,
            Quaternion.identity);

        float dx = Mathf.Cos(angle * Mathf.Deg2Rad);
        float dy = Mathf.Sin(angle * Mathf.Deg2Rad);
        Vector3 v = new Vector3(dx, dy, 0).normalized * thowPower;

        obj.GetComponent<Rigidbody>().AddForce(v, ForceMode.Impulse);
    }
}
