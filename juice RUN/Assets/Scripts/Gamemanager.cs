using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gamemanager : MonoBehaviour
{
    public static Gamemanager inst;
    public GameObject finalboss;
    // Start is called before the first frame update
    void Awake()
    {
        inst = this;
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ongamestart()
    {
        PLAYER.inst.sf.follow = true;
        CAMFOLLOW.inst.SF.follow = true;
        PLAYER.inst.anim.SetBool("RUN", true);
    }
   public void win()
    {
        finalboss.GetComponent<Animator>().SetTrigger("fail");
        UIMANAGER.inst.Invoke("LC", 1f);
        PLAYER.inst.danceanim();
    }
    public void lose()
    {
        int i = Random.Range(0, 2);
        if (i == 0)
        {
            finalboss.GetComponent<Animator>().SetTrigger("d1");
        }
        else
        {
            finalboss.GetComponent<Animator>().SetTrigger("d2");
        }
        PLAYER.inst.anim.SetTrigger("wallfail");
        UIMANAGER.inst.LF();
    }
}
