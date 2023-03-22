using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NamChet : MonoBehaviour
{
    Vector2 ViTriChet;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player" && collision.contacts[0].normal.y < 0)
        {
            print("OK");
            print(ViTriChet.x);
            Destroy(gameObject);
            GameObject NamChet = (GameObject)Instantiate(Resources.Load("Prefabs/NamChet"));
            ViTriChet.y -= 0.255f;
            NamChet.transform.localPosition = ViTriChet;
          
        }
    }

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ViTriChet = transform.localPosition;
        
    }
}
