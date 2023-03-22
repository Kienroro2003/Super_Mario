using UnityEngine;
using System.Collections;

public class NamDocChet : MonoBehaviour
{
    
    // Use this for initialization
    void Start()
    {
        StartCoroutine(NamDoChet());
    }
    IEnumerator NamDoChet()
    {
        yield return new WaitForSeconds(1.0f);
        Destroy(gameObject);
    }

}
