using UnityEngine;
using System;
using System.Collections;
using System.Threading;
namespace RayCaster.Utils
{

    public class Camara : MonoBehaviour
    {

        public InteractivoVR RayCast;
        public PuntoReticula Reticula;
        public GameObject Objeto;
        public Material WebCamara;
        public Material ColorVerde;
        public AudioSource AudioControl;
        public AudioClip Entrar;
        public AudioClip Salir;
        
        public bool EsteEsElObjeto;

        void Start()
        {
            EsteEsElObjeto = true;
            gameObject.GetComponent<Renderer>().material = WebCamara;

        }
        
        void Update()
        {

            if (RayCast.EstasMirando == true && EsteEsElObjeto == true)
            {
                gameObject.GetComponent<Renderer>().material = ColorVerde;
                AudioControl.clip = Entrar;
                AudioControl.Play();
          
               
                EsteEsElObjeto = false;
            }
            else if (RayCast.EstasMirando == false && EsteEsElObjeto == false)
            {
                gameObject.GetComponent<Renderer>().material = WebCamara;
                AudioControl.clip = Salir;
                AudioControl.Play();
                EsteEsElObjeto = true;
            }
        }
    }
}

