using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;

public class UIMANAGER : MonoBehaviour
{
    public static UIMANAGER inst;
    public GameObject startpanel, LCPANEL, LFPANEL,shopbutton,shoppanel,hud;
    public Text levtxt,coinstext;
    public List<GameObject> hats = new List<GameObject>();
    public List<GameObject> Buttons = new List<GameObject>();
    public List<int> price = new List<int>();
    public int coins;
    public GameObject usebutton;
    public Text lccointxt;
    public Image vibration;
    public Sprite on, off;
    // Start is called before the first frame update
    private void Awake()
    {
        inst = this;
        if (!PlayerPrefs.HasKey("SELHAT"))
        {
            PlayerPrefs.SetInt("SELHAT", 0);
        }
        if (!PlayerPrefs.HasKey("VIBRATE"))
        {
            PlayerPrefs.SetInt("VIBRATE", 0);//vibration on
        }
       
        for (int i = 0; i < hats.Count; i++)
        {
            if (i == 0)
            {
                if (!PlayerPrefs.HasKey("unlockhat" + i))
                {
                    PlayerPrefs.SetInt("unlockhat" + i, 1);
                }
            }
            else
            {
                if (!PlayerPrefs.HasKey("unlockhat" + i))
                {
                    PlayerPrefs.SetInt("unlockhat" + i, 0);
                }
            }
        }
        if (!PlayerPrefs.HasKey("coins"))
        {
            PlayerPrefs.SetInt("coins", 0);
        }
            hats[PlayerPrefs.GetInt("SELHAT")].SetActive(true);
        if (!PlayerPrefs.HasKey("OVER"))
        {
            PlayerPrefs.SetInt("OVER", 0);
        }
    }
    void Start()
    {
        levtxt.text ="LEVEL "+ PlayerPrefs.GetInt("LEVEL").ToString();
        coinstext.text= PlayerPrefs.GetInt("coins").ToString();
        prices();
    }
    public void Swipestart()
    {
        PLAYER.inst.ongamestart = true;
        Gamemanager.inst.ongamestart();
        startpanel.GetComponent<Image>().enabled = false;
        shopbutton.SetActive(false);
        Invoke("disablestartpanel", 1f);
    }
    void disablestartpanel()
    {
        startpanel.SetActive(false);
    }
    public void LC()
    {
        LCPANEL.SetActive(true);
        hud.SetActive(false);
        PlayerPrefs.SetInt("LEVEL", PlayerPrefs.GetInt("LEVEL") + 1);
        if (PlayerPrefs.GetInt("LEVEL") <= 5)
        {
            lccointxt.text = "20";
            PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins")+20);
        }
        else if (PlayerPrefs.GetInt("LEVEL") <= 10)
        {
            lccointxt.text = "30";
            PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") + 30);

        }
        else if (PlayerPrefs.GetInt("LEVEL") <= 15)
        {
            lccointxt.text = "40";
            PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") + 40);

        }
        else if (PlayerPrefs.GetInt("LEVEL") <= 25)
        {
            lccointxt.text = "50";
            PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") + 50);

        }
        else if (PlayerPrefs.GetInt("LEVEL") <= 50)
        {
            lccointxt.text = "60";
            PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") + 60);

        }
        else
        {
            lccointxt.text = "80";
            PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") + 80);

        }
        if (PlayerPrefs.GetInt("LEVEL") >= 21) {
            PlayerPrefs.SetInt("OVER", 1);
        }
        coinstext.text = PlayerPrefs.GetInt("coins").ToString();
        setAds();

    }
    public void LF()
    {
        LFPANEL.SetActive(true);
        hud.SetActive(false);
       
    }
    public void retry()
    {
        setAds();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void NEXT()
    {
        if (PlayerPrefs.GetInt("OVER") == 0)
        {
            int sceneno = PlayerPrefs.GetInt("LEVEL");
            SceneManager.LoadScene(sceneno);
        }
        else
        {
            int sceneno = Random.Range(2,20 );
            SceneManager.LoadScene(sceneno);
        }
    }
    int selctedhat=0;
    public void shop()
    {
        shoppanel.SetActive(true);
    }
    public void shopback()
    {
        shoppanel.SetActive(false);
        PLAYER.inst.Hats[PlayerPrefs.GetInt("SELHAT")].SetActive(true);
       
    }
    public void hatsel(int i)
    {
        if (PlayerPrefs.GetInt("unlockhat" + i)== 1)
        {
            PlayerPrefs.SetInt("SELHAT", i);
            sethat();
        }
       else if (PlayerPrefs.GetInt("coins") >= price[i])
        {
            PlayerPrefs.SetInt("unlockhat" + i, 1);
            PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins")- price[i]);
            coinstext.text = PlayerPrefs.GetInt("coins").ToString();
            PlayerPrefs.SetInt("SELHAT", i);
            prices();
            sethat();
        }
    }

    void sethat()
    {
        for (int i = 0; i < hats.Count; i++)
        {
                if (i == PlayerPrefs.GetInt("SELHAT"))
                {
                    hats[i].SetActive(true);
                PLAYER.inst.Hats[PlayerPrefs.GetInt("SELHAT")].SetActive(true);
            }
            else
                {
                    hats[i].SetActive(false);
                PLAYER.inst.Hats[PlayerPrefs.GetInt("SELHAT")].SetActive(false);
            }
        }
    }
    void prices()
    {
        for (int i = 0; i < hats.Count; i++)
        {
            if (PlayerPrefs.GetInt("unlockhat" + i) == 1)
            {
                Buttons[i].transform.GetChild(0).GetComponent<Text>().text="Use";
            }
            else
            {
                Buttons[i].transform.GetChild(0).GetComponent<Text>().text =price[i].ToString();
            }
        }
    }
    void setAds()
    {
       
        if(PlayerPrefs.GetInt("adsshown") == 0)
         {
            AdManager.instance.showInterstitial(); 
            PlayerPrefs.SetInt("adsshown", PlayerPrefs.GetInt("adsshown")+1);
        }
        else if(PlayerPrefs.GetInt("adsshown") == 1)
        {
            PlayerPrefs.SetInt("adsshown", 0);
        }
    }
   public void vibratebutton()
    {
        if (PlayerPrefs.GetInt("VIBRATE") == 0)
        {
            vibration.GetComponent<Image>().sprite = off;
            PlayerPrefs.SetInt("VIBRATE", 1);
        }
        else
        {
            PlayerPrefs.SetInt("VIBRATE", 0);
            vibration.GetComponent<Image>().sprite = on;
        }

    }
}
