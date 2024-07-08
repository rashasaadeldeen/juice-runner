using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class COLORCHANGER : MonoBehaviour
{
    public static COLORCHANGER inst;
    public float height = 0;
    public List<GameObject> cubes = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        inst = this;

        foreach (Transform child in transform)
        {
            cubes.Add(child.gameObject);
        }
    }
    public void pass()
    {
        for (int i = 0; i < cubes.Count; i++)
        {
            cubes[i].GetComponent<Rigidbody>().isKinematic = false;
            Destroy(cubes[i].gameObject, 5f);
        }
    }


    // Update is called once per frame
}
