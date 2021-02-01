using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class vida : MonoBehaviour {

    public Image  UIImagen;

	// Use this for initialization
	void Start () {

        UIImagen = GameObject.Find("corazon").GetComponent<Image>();

    }
	
	void Update () {

        if (Input.GetKeyDown("q"))
        {

            UIImagen.sprite = Resources.Load<Sprite>("vidas");
        }
        if (Input.GetKeyDown("w"))
        {

            UIImagen.sprite = Resources.Load<Sprite>("corazon2");
        }
    }
}
