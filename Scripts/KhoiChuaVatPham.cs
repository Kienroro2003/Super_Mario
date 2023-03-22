using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KhoiChuaVatPham : MonoBehaviour
{
    public float DoNayCuaKhoi = 0.5f;
    public float TocDoNay = 4f;
    private bool DuocNay = true;
    private Vector2 ViTriBanDau;

    public bool ChuaNam;
    public bool ChuaXu;
    public bool ChuaSao;

    GameObject Mario;

    private void Awake()
    {
        Mario = GameObject.FindGameObjectWithTag("Player");
    }

    // Start is called before the first frame update
    void Start()
    {
        ViTriBanDau = transform.position;   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "VaCham" && collision.contacts[0].normal.y > 0)
        {
            
            KhoiNayLen();
            
        }
    }

    private void KhoiNayLen()
    {
        Debug.Log(ViTriBanDau.x + " " + ViTriBanDau.y);
        if (DuocNay)
        {
            DuocNay = false;
            StartCoroutine(KhoiNay());
            if (ChuaNam)
            {
                NamVaHoa();
            }
        }
    }

    IEnumerator KhoiNay()
    {
        while (true)
        {
            transform.localPosition = new Vector2(transform.localPosition.x, transform.localPosition.y + TocDoNay * Time.deltaTime);
            if (transform.localPosition.y >= ViTriBanDau.y + DoNayCuaKhoi) break;
            yield return null;

        }
        while (true)
        {
            transform.localPosition = new Vector2(transform.localPosition.x, transform.localPosition.y - TocDoNay * Time.deltaTime);
            if (transform.localPosition.y <= ViTriBanDau.y ) break;
            Destroy(gameObject);
            GameObject KhoiRong = (GameObject)Instantiate(Resources.Load("Prefabs/KhoiRong"));
            KhoiRong.transform.position = ViTriBanDau;
            yield return null;

        }
    }

    void NamVaHoa()
    {
        int CapDoHienTai = Mario.GetComponent<MarioScript>().CapDo;
        GameObject Nam = null;
        if (CapDoHienTai == 0) Nam = (GameObject)Instantiate(Resources.Load("Prefabs/Nam"));
        else Nam = (GameObject)Instantiate(Resources.Load("Prefabs/Nam"));
        //Mario.GetComponent<MarioScript>().TaoAmThanh("")
        Nam.transform.SetParent(this.transform.parent);
        Nam.transform.position = new Vector2(ViTriBanDau.x, ViTriBanDau.y + 1f);
    }
}
