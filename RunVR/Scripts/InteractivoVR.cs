using System;
using UnityEngine;
using UnityEngine.UI;

namespace RayCaster.Utils
{
    public class InteractivoVR : MonoBehaviour
    {
        public event Action Mirando;            // Se llama cuando la mirada se mueve sobre este objeto
        public event Action NoEstaMirando;      // Se llama cuando la mirada deja este objeto
        public event Action AlHacerClick;       // Se llama cuando se detecta la entrada de clic mientras la mirada esta sobre este objeto.
        public event Action AlHacerDobleClick;  // Se llama cuando se detecta la entrada de doble clic mientras la mirada esta sobre este objeto.
        public event Action HaciaArriba;        // Se llama cuando Fire1 se libera mientras que la mirada esta sobre este objeto.
        public event Action HaciaAbajo;         // Se llama cuando se pulsa Fire1 mientras la mirada est� sobre este objeto.

        protected bool SeAcabo;

        public bool EstasMirando = false;

        public Image ImagenPunto;
        public Image ImagenCirculo;
        public Animator AnimacionCirculo;

        public bool SeAcaba
        {
            get { return SeAcabo; }              // Esta la mirada actualmente sobre este objeto?
        }


        // Las funciones siguientes son llamadas por el Raycaster cuando se detecta la entrada apropiada.
        // A su vez llaman a los eventos apropiados si tienen suscriptores.
        public void Entrando()
        {
            SeAcabo = true;

            if (Mirando != null)
                Mirando();

            EstasMirando = true;

            // Si llegamos a un elemento interactivo se desactiva el punto y se activa el circulo y su animacion
            ImagenPunto.enabled = false;
            ImagenCirculo.enabled = true;
            AnimacionCirculo.enabled = true;
        }


        public void Saliendo()
        {
            SeAcabo = false;

            if (NoEstaMirando != null)
                NoEstaMirando();

            EstasMirando = false;

            // Si llegamos a un elemento interactivo se desactiva el circulo y su animacion y se activa el punto 
            ImagenPunto.enabled = true;
            ImagenCirculo.enabled = false;
            AnimacionCirculo.enabled = false;
        }


        public void Click()
        {
            if (AlHacerClick != null)
                AlHacerClick();
        }


        public void DobleClick()
        {
            if (AlHacerDobleClick != null)
                AlHacerDobleClick();
        }


        public void Arriba()
        {
            if (HaciaArriba != null)
                HaciaArriba();
        }


        public void Abajo()
        {
            if (HaciaAbajo != null)
                HaciaAbajo();
        }
    }
}