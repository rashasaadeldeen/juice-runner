using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MenuAdPage : MonoBehaviour {
	public Image MenuAdImg;
	public GameObject CloseBtn;
	public static MenuAdPage instance;
	public bool  portraitloaded;
	public bool iconLoaded;
	public bool LandscapeLoaded;
	void Awake()
	{
		instance = this;
		gameObject.SetActive (false);
	//	CloseBtn.transform.localScale = Vector3.zero;
	}
	string url;
	public void Openad()
	{
		Application.OpenURL (url);
	}
	public void Close()
	{
		gameObject.SetActive (false);
	}

	public IEnumerator LoadImg()
	{
		int i = Random.Range (0,AdManager.instance.MgImgList.Count-1);
		MenuAdImg.sprite = AdManager.instance.MgImgList [i];
		url = AdManager.instance.MgLinkToList [i];
		yield return new WaitForSeconds (3);		
		gameObject.SetActive (true);

	}
	public IEnumerator Loadland()
	{
		int i = Random.Range (0,AdManager.instance.LandList.Count-1);
		MenuAdImg.sprite = AdManager.instance.LandList [i];
		url = AdManager.instance.LandToList [i];
		yield return new WaitForSeconds (3);		
		gameObject.SetActive (true);

	}

	public IEnumerator ShowAdInGamea()
	{
		int i = Random.Range (0,AdManager.instance.MgImgList.Count-1);
		MenuAdImg.sprite = AdManager.instance.MgImgList [i];
		url = AdManager.instance.MgLinkToList [i];
		yield return new WaitForSeconds (0.2f);		
		gameObject.SetActive (true);

	}

}
