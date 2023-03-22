using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MarioScript : MonoBehaviour
{
    private float VanTocToiDa = 10f;
    private float TGGiuPhim = 0;
    private float KTGiuPhim = 0.2f;
    private float NhayCao = 500;
    private float NhayThap = 5;
    private float RoiXuong = 5;
    private float VanToc = 7;
    private float TocDo = 0;
    private bool DuoiDat = true;
    private bool ChuyenHuong = false;
    private bool QuayPhai = true;

    private int score;

    public int CapDo = 0;
    public bool BienHinh = false;

    private int maxLives = 3;

    private Rigidbody2D r2d;
    private Animator HoatHoa;
    private AudioSource AmThanh;
    private Vector2 ViTriChet;
    [SerializeField]
    private GameObject gameOver;
    [SerializeField]
    private Button resetButton;
    [SerializeField]
    private Text scoreLabel;

    // Start is called before the first frame update
    void Start()
    {
        HoatHoa = GetComponent<Animator>();
        r2d = GetComponent<Rigidbody2D>();
        AmThanh = GetComponent<AudioSource>();
    }

    private void Awake()
    {
        score = 0;
        scoreLabel.text = String.Format("Score: {0:000}", score);
        gameOver.gameObject.SetActive(false);

        resetButton.onClick.AddListener(() =>
        {
            Debug.Log("Reset");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            Time.timeScale = 1f;
        });
        resetButton.gameObject.SetActive(false);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Nam" && CapDo != 1)
        {
            CapDo = 1;
            BienHinh = true;
        }
        if(collision.collider.tag == "DongVang")
        {
            GameObject DongVang = GameObject.FindGameObjectWithTag("DongVang");
            score += DongVang.GetComponent<DongVang>().Diem;
            UpdateScore();

        }
    }

    void UpdateScore()
    {
        TaoAmThanh("smb_coin");
        scoreLabel.text = String.Format("Score: {0:000}", score);
    }

    // Update is called once per frame
    void Update()
    {
        HoatHoa.SetFloat("TocDo", TocDo);
        HoatHoa.SetBool("DuoiDat", DuoiDat);
        HoatHoa.SetBool("ChuyenHuong", ChuyenHuong);
        NhayLen();
        KiemTraTangToc();
        if(BienHinh == true)
        {
            switch (CapDo)
            {
                case 0:
                    {
                        StartCoroutine(MarioThuNho());
                        TaoAmThanh("smb_pipe");
                        BienHinh = false;
                        break;
                    }
                case 1:
                    {
                        StartCoroutine(MarioAnNam());
                        TaoAmThanh("smb_powerup");
                        BienHinh = false;
                        break;
                    }
                case 2:
                    {
                        StartCoroutine(MarioAnHoa());
                        TaoAmThanh("smb_powerup_appears");
                        BienHinh = false;
                        break;
                    }
                default:BienHinh = false; break;
            }
        }
        if (gameObject.transform.position.y < -10f)
        {
            TaoAmThanh("smb_mariodie");
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        DiChuyen();
    }

    private void DiChuyen()
    {
        float PhimNhanPhaiTrai = Input.GetAxis("Horizontal");
        r2d.velocity = new Vector2(PhimNhanPhaiTrai * VanToc, r2d.velocity.y);
        TocDo = Mathf.Abs(PhimNhanPhaiTrai * VanToc);
        if (PhimNhanPhaiTrai > 0 && !QuayPhai) HuongMatMario();
        if (PhimNhanPhaiTrai < 0 && QuayPhai) HuongMatMario();
    }

    void HuongMatMario()
    {
        QuayPhai = !QuayPhai;
        Vector2 HuongQuay = transform.localScale;
        HuongQuay.x *= -1;
        transform.localScale = HuongQuay;
        if(TocDo > 0)StartCoroutine(MarioChuyenHuong());
    }

    void NhayLen()
    {
        if (Input.GetKeyDown(KeyCode.W) && DuoiDat == true)
        {
            r2d.AddForce(Vector2.up * NhayCao);
            DuoiDat = false;
            TaoAmThanh("smb_jump-small");
        }
        if(r2d.velocity.y < 0)
        {
            r2d.velocity += Vector2.up * Physics2D.gravity.y * (RoiXuong - 1) * Time.deltaTime;
        }else if(r2d.velocity.y > 0 && !Input.GetKey(KeyCode.W))
        {
            r2d.velocity += Vector2.up * Physics2D.gravity.y * (NhayThap - 1) * Time.deltaTime;
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "NenDat")
        {
            print("Trigger Enter");
            DuoiDat = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag != "NenDat")
        {
            print("Trigger Exit");
            DuoiDat = false;
        }
    }

    IEnumerator MarioChuyenHuong()
    {
        ChuyenHuong = true;
        yield return new WaitForSeconds(0.1f);
        ChuyenHuong = false;
    }

    private void KiemTraTangToc()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            TGGiuPhim += Time.deltaTime;
            if (TGGiuPhim < KTGiuPhim)
            {
                print("Chiu chiu");
            }
            else if(VanToc <= VanTocToiDa)
            {
                VanToc = VanToc * 1.5f;
                Debug.Log(VanToc);
            }
        }
        else
        {
            TGGiuPhim = 0;
            VanToc = 7f;
        }
        
    }

    IEnumerator MarioAnNam()
    {
        float DoTre = 0.1f;
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioNho"),0);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioLon"), 1);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioAH"), 0);
        yield return new WaitForSeconds(DoTre);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioNho"), 1);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioLon"), 0);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioAH"), 0);
        yield return new WaitForSeconds(DoTre);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioNho"), 0);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioLon"), 1);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioAH"), 0);
        yield return new WaitForSeconds(DoTre);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioNho"), 1);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioLon"), 0);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioAH"), 0);
        yield return new WaitForSeconds(DoTre);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioNho"), 0);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioLon"), 1);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioAH"), 0);
        yield return new WaitForSeconds(DoTre);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioNho"), 1);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioLon"), 0);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioAH"), 0);
        yield return new WaitForSeconds(DoTre);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioNho"), 0);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioLon"), 1);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioAH"), 0);
        yield return new WaitForSeconds(DoTre);
    }

    IEnumerator MarioAnHoa()
    {
        float DoTre = 0.1f;
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioNho"), 0);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioLon"), 0);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioAH"), 1);
        yield return new WaitForSeconds(DoTre);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioNho"), 1);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioLon"), 0);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioAH"), 0);
        yield return new WaitForSeconds(DoTre);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioNho"), 0);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioLon"), 0);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioAH"), 1);
        yield return new WaitForSeconds(DoTre);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioNho"), 1);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioLon"), 0);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioAH"), 0);
        yield return new WaitForSeconds(DoTre);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioNho"), 0);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioLon"), 0);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioAH"), 1);
        yield return new WaitForSeconds(DoTre);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioNho"), 1);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioLon"), 0);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioAH"), 0);
        yield return new WaitForSeconds(DoTre);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioNho"), 0);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioLon"), 0);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioAH"), 1);
        yield return new WaitForSeconds(DoTre);
    }

    IEnumerator MarioThuNho()
    {
        float DoTre = 0.1f;
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioNho"), 1);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioLon"), 0);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioAH"), 0);
        yield return new WaitForSeconds(DoTre);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioNho"), 0);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioLon"), 1);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioAH"), 0);
        yield return new WaitForSeconds(DoTre);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioNho"), 1);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioLon"), 0);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioAH"), 0);
        yield return new WaitForSeconds(DoTre);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioNho"), 0);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioLon"), 1);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioAH"), 0);
        yield return new WaitForSeconds(DoTre);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioNho"), 1);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioLon"), 0);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioAH"), 0);
        yield return new WaitForSeconds(DoTre);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioNho"), 0);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioLon"), 1);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioAH"), 0);
        yield return new WaitForSeconds(DoTre);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioNho"), 1);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioLon"), 0);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioAH"), 0);
        yield return new WaitForSeconds(DoTre);
    }

    public void TaoAmThanh(string FileAmThanh)
    {
        AmThanh.PlayOneShot(Resources.Load<AudioClip>("Audio/" + FileAmThanh));
    }

    

    public void MarioChet()
    {
        print("Chet");
        ViTriChet = transform.localPosition;
        GameObject MarioChet = (GameObject)Instantiate(Resources.Load("Prefabs/MarioChet"));
        MarioChet.transform.localPosition = ViTriChet;
        gameOver.SetActive(true);
        resetButton.gameObject.SetActive(true);
        //StartCoroutine(HMarioChet());
        Destroy(gameObject);
    }
}
