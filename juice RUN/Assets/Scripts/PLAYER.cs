using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Dreamteck.Splines;
public class PLAYER : MonoBehaviour
{
    public static PLAYER inst;
   public SplineFollower sf;
   public Animator anim;
  public  bool ongamestart = false;
  public List<GameObject>Hats=new List<GameObject>();
    // Start is called before the first frame update

    public AudioSource buble;
    public AudioClip drop;
    public AudioClip spill;
    public AudioClip change;
    public AudioClip coin;
    void Awake()
    {
        inst = this;
        Speed = 45f;
        sf = transform.parent.GetComponent<SplineFollower>();
        anim = GetComponent<Animator>();
        scalevalue = 1;
        Hats[PlayerPrefs.GetInt("SELHAT")].SetActive(true);
    } 
    public float Speed, xmin, xmax;
    // Update is called once per frame
    private void Start()
    {
        if (PlayerPrefs.GetInt("LEVEL") <= 5)
        {
            sf.followSpeed = 20;
            CAMFOLLOW.inst.SF.followSpeed = 20;
        }
        else if (PlayerPrefs.GetInt("LEVEL") <= 10)
        {
            sf.followSpeed = 23;
            CAMFOLLOW.inst.SF.followSpeed = 23;

        }
        else if (PlayerPrefs.GetInt("LEVEL") <= 15)
        {
            CAMFOLLOW.inst.SF.followSpeed = 28;
            sf.followSpeed = 28;
        }
        else if (PlayerPrefs.GetInt("LEVEL") <= 25)
        {
            CAMFOLLOW.inst.SF.followSpeed = 33;
            sf.followSpeed = 33;
        }
        else if (PlayerPrefs.GetInt("LEVEL") <= 50)
        {
            CAMFOLLOW.inst.SF.followSpeed = 37;
            sf.followSpeed = 37;
        }
        else
        {
            CAMFOLLOW.inst.SF.followSpeed = 40;
            sf.followSpeed = 40;
        }
    }
    void Update()
    {
        if (!ongamestart)
            return;
#if UNITY_EDITOR
            if (Input.GetMouseButton(0))
            {
                float rotX = Input.GetAxis("Mouse X") * Speed * Mathf.Deg2Rad;
                //float roty = Input.GetAxis("Mouse Y") * Speed * Mathf.Deg2Rad;
                this.transform.position += new Vector3(rotX, 0, 0);
                // this.transform.position = new Vector3(Mathf.Clamp(this.transform.position.x, -1.1f, 1.1f), this.transform.position.y, Mathf.Clamp(this.transform.position.z, -0.82f, 2.0f));
                this.transform.localPosition = new Vector3(Mathf.Clamp(this.transform.localPosition.x, xmin, xmax), this.transform.localPosition.y,0);
            }
#else
        //---------------------------------------------------------------------------------
	
        if(Input.touchCount>0 && Input.GetTouch(0).phase == TouchPhase.Moved )
		{
			Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
			float rotX = touchDeltaPosition.x* Speed/18* Mathf.Deg2Rad;
			float roty = touchDeltaPosition.y* Speed/18* Mathf.Deg2Rad;

             this.transform.position += new Vector3(rotX, 0, 0);
            this.transform.localPosition = new Vector3(Mathf.Clamp(this.transform.localPosition.x, xmin, xmax), this.transform.localPosition.y, this.transform.localPosition.z);
		}	
        
#endif
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "OPP" && this.transform.GetChild(1).GetChild(1).GetComponent<Renderer>().materials[0].color == other.transform.GetChild(1).GetChild(1).GetComponent<Renderer>().materials[0].color)
        {
            incvalue = 0.1f;
            scalevalue = scalevalue+ incvalue;
            changesize();
            Material mat= new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended"));
            mat.color = other.transform.GetChild(1).GetChild(1).GetComponent<Renderer>().material.color;
            other.transform.GetChild(2).GetComponent<Renderer>().material.SetColor("_TintColor",mat.color);
            other.transform.GetChild(2).GetComponent<ParticleSystem>().Play();
            other.transform.GetChild(1).GetChild(1).GetComponent<Renderer>().enabled = false;
            buble.PlayOneShot(drop);
            Destroy(other.GetComponent<Collider>());
            Destroy(other.gameObject,1f);
#if UNITY_ANDROID
            if (PlayerPrefs.GetInt("VIBRATE") == 0)
            {
                Vibration.Vibrate(30);
            }
#endif

        }
        else if(other.tag == "OPP" && this.transform.GetChild(1).GetChild(1).GetComponent<Renderer>().materials[0].color != other.transform.GetChild(1).GetChild(1).GetComponent<Renderer>().material.color)
        {
            incvalue = 0.1f;
            scalevalue = scalevalue - incvalue;
            print(scalevalue);
            if (scalevalue <= 0.41f)
            {
                UIMANAGER.inst.LF();
                sf.follow = false;
                CAMFOLLOW.inst.SF.follow = false;
                anim.SetTrigger("SAD");
            }
            else
            {
                changesize();
            }
            Material mat = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended"));
            mat.color = other.transform.GetChild(1).GetChild(1).GetComponent<Renderer>().material.color;
            other.transform.GetChild(2).GetComponent<Renderer>().material.SetColor("_TintColor", mat.color);
            other.transform.GetChild(2).GetComponent<ParticleSystem>().Play();

            other.transform.GetChild(1).GetChild(1).GetComponent<Renderer>().enabled = false;
            buble.PlayOneShot(spill);
            Destroy(other.GetComponent<Collider>());
            Destroy(other.gameObject, 1f);
#if UNITY_ANDROID
            if (PlayerPrefs.GetInt("VIBRATE") == 0)
            {
                Vibration.Vibrate(60);
            }
#endif
        }
        if (other.tag == "Finish")
        {
            sf.follow = false;
            CAMFOLLOW.inst.SF.follow = false;
            StartCoroutine(checkatlast());
            transform.DOMoveX(0, 0.3f);
            ongamestart = false;
            Destroy(other.gameObject);
        }
        if (other.tag == "COLOR")
        {
            if (scalevalue >= other.GetComponent<COLORCHANGER>().height)
            {
                this.transform.GetChild(1).GetChild(1).GetComponent<Renderer>().material = other.GetComponent<COLORCHANGER>().cubes[0].GetComponent<Renderer>().material;
                buble.PlayOneShot(change);
                other.GetComponent<COLORCHANGER>().pass();
            }
            else
            {
                UIMANAGER.inst.LF();
                sf.follow = false;
                CAMFOLLOW.inst.SF.follow = false;
                anim.SetTrigger("wallfail");
            }
            
        
                if (this.transform.GetChild(1).GetChild(1).GetComponent<Renderer>().material != other.GetComponent<COLORCHANGER>().cubes[0].GetComponent<Renderer>().material )
                {
                    incvalue = 0.1f;
                    scalevalue = scalevalue - incvalue;
                    changesize();
                }
            
            
#if UNITY_ANDROID
            if (PlayerPrefs.GetInt("VIBRATE") == 0)
            {
                Vibration.Vibrate(60);
            }
#endif
            Destroy(other.GetComponent<Collider>());
        }


        if (other.tag == "COIN")
        {
            other.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
            other.transform.GetComponent<Renderer>().enabled=false;
            Destroy(other.gameObject, 0.5f);
            Destroy(other.gameObject.GetComponent<Collider>(), 0f);
            PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") + 1);
            UIMANAGER.inst.coinstext.text = PlayerPrefs.GetInt("coins").ToString();

            buble.PlayOneShot(coin);

#if UNITY_ANDROID
            if (PlayerPrefs.GetInt("VIBRATE") == 0)
            {
                Vibration.Vibrate(15);
            }
#endif
        }
    }
    float incvalue = 0,scalevalue;
    void changesize()
    {
        this.transform.rotation = Quaternion.Euler(0, 0, 0);
        transform.DOScale(scalevalue, 0.4f).SetEase(Ease.Linear);
    }
   public void danceanim()
    {
        int i = Random.Range(0, 2);
        switch (i)
        {
            case 0:
                anim.SetTrigger("D1");
                break;
            case 1:
                anim.SetTrigger("D2");
                break;
        }
    }
    IEnumerator checkatlast()
    {
        this.transform.DOLookAt(Gamemanager.inst.finalboss.transform.position, 0.1f);
        Gamemanager.inst.finalboss.transform.DOLookAt(this.transform.position, 0.1f);
        anim.SetTrigger("FIGHT");
        Gamemanager.inst.finalboss.GetComponent<Animator>().SetTrigger("fight");
        UIMANAGER.inst.hud.SetActive(false);
        yield return new WaitForSeconds(1f);
        CAMFOLLOW.inst.rotatearound = true;
        if (scalevalue >= Gamemanager.inst.finalboss.transform.localScale.x)
        {
            Gamemanager.inst.win();
        }
        else
        {
            Gamemanager.inst.lose();
        }

    }

}
