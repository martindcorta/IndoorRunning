using UnityEngine;
using System;
using System.Collections;
namespace RayCaster.Utils
{

    public class Run : MonoBehaviour
    {

        public InteractivoVR RayCast;
        public PuntoReticula Reticula;
        public GameObject Objeto;
        public Material ColorRojo;
        public Material ColorVerde;
        public AudioSource AudioControl;
        public AudioClip Entrar;
        public AudioClip Salir;
        
        public bool EsteEsElObjeto;

        void Start()
        {
            EsteEsElObjeto = true;
            gameObject.GetComponent<Renderer>().material = ColorRojo;

        }
        
        void Update()
        {

            if (RayCast.EstasMirando == true && EsteEsElObjeto == true)
            {
                gameObject.GetComponent<Renderer>().material = ColorVerde;
                AudioControl.clip = Entrar;
                AudioControl.Play();
                Application.LoadLevel("SeleccionCamino");
                EsteEsElObjeto = false;
            }
            else if (RayCast.EstasMirando == false && EsteEsElObjeto == false)
            {
                gameObject.GetComponent<Renderer>().material = ColorRojo;
                AudioControl.clip = Salir;
                AudioControl.Play();
                EsteEsElObjeto = true;
            }
        }
    }
}

