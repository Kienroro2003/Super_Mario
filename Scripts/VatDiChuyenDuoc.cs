using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VatDiChuyenDuoc : MonoBehaviour
{
    public float VatTocVat;
    public bool DiChuyenTrai = true;
    // Start is called before the first frame update
    private void FixedUpdate()
    {
        Vector2 DiChuyen = transform.position;
        if (DiChuyenTrai)
        {
            DiChuyen.x -= VatTocVat * Time.deltaTime; 
        }
        else
        {
            DiChuyen.x += VatTocVat * Time.deltaTime;
        }
        transform.position = DiChuyen;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag != "Player" &&  collision.contacts[0].normal.x > 0)
        {
            DiChuyenTrai = false;
        }
        else if(collision.collider.tag != "Player" && collision.contacts[0].normal.x <0)
        {
            DiChuyenTrai = true;
        }
    }
}
