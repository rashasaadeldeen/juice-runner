using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShowInGameIcon : MonoBehaviour {

    SpriteRenderer spr;
	Sprite loadedsprite;
	string loadedlink;
	Image thisimage;
	void Awake(){
        if (this.GetComponent<SpriteRenderer>())
            spr = this.GetComponent<SpriteRenderer>();
        if (this.GetComponent<Image>())
            thisimage = GetComponent<Image> ();
	}
	void OnEnable(){
		
	
		if (AdManager.instance && AdManager.instance.IconLoaded) {

			int i = Random.Range(0,AdManager.instance.Iconlist.Count-1);
			loadedsprite = AdManager.instance.Iconlist[i];
			loadedlink = AdManager.instance.IconToList[i];
            if(thisimage)
			    thisimage.sprite = loadedsprite;
            if(spr)
                spr.sprite = loadedsprite;
			Debug.Log (loadedlink);
		} else {
			gameObject.SetActive (false);
		}

	
	}
	public 	void OnButtonClick(){
		Application.OpenURL (loadedlink);
	}



}
