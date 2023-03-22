using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DongVang : MonoBehaviour
{

    private int diem = 10;
    public int Diem
    {
        get { return diem; }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Collider collider = GetComponent<Collider>();
        if (collision.collider.tag == "Player")
        {
            Destroy(gameObject);
        }
        else
        {
            collider.isTrigger = true;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
