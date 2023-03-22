using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarioChet : MonoBehaviour
{
    private Vector2 ViTriChet;
    float TocDoNay = 20.5f;
    float DoNayCao = 120.5f;
    // Start is called before the first frame update
    void Update()
    {
        ViTriChet = transform.localPosition;
        StartCoroutine(HMarioChet());
    }

    IEnumerator HMarioChet()
    {
        
        while (true)
        {
            transform.localPosition = new Vector2(transform.localPosition.x, transform.localPosition.y + TocDoNay * Time.deltaTime);
            if (transform.localPosition.y >= ViTriChet.y + DoNayCao + 1) break;
            yield return null;
            transform.localPosition = new Vector2(transform.localPosition.x, transform.localPosition.y - TocDoNay * Time.deltaTime);
            if (transform.localPosition.y <= -10f)
            {
                Destroy(gameObject);
                break;
            }
            yield return null;
        }

    }
}
