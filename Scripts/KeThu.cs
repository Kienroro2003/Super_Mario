using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeThu : MonoBehaviour
{
    GameObject mario;
    Vector2 ViTriChet;
    private void Update()
    {
        ViTriChet = transform.localPosition;
    }

    private void Awake()
    {
        mario = GameObject.FindGameObjectWithTag("Player");
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Player" && (collision.contacts[0].normal.x < 0 || collision.contacts[0].normal.x > 0)){
            if (mario.GetComponent<MarioScript>().CapDo > 1)
            {
                mario.GetComponent<MarioScript>().CapDo = 1;
                mario.GetComponent<MarioScript>().BienHinh = true;
            }
            else
            {
                mario.GetComponent<MarioScript>().TaoAmThanh("smb_mariodie");
                mario.GetComponent<MarioScript>().MarioChet();
            }
        }
       
    }
}
