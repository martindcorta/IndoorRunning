using System;
using UnityEngine;
using UnityEngine.UI;

namespace RayCaster.Utils
{
    public class RayCaster : MonoBehaviour
    {
        public event Action<RaycastHit> SeGolpeoConRayCast;    // Este evento s'intitule cada fotograma Que le mirada del usuario es mayor de un colisionador.

        public Transform Camara;
        public LayerMask CapasDeExclucion;           // Capas para excluir de la raycast.
        public PuntoReticula Reticula;               // La retícula, en su caso.
        public EntradaVR EntradaVR;                  // Se utiliza para llamar a los eventos de entrada en base a la corriente VRInteractiveItem.
        public bool MostrarDepuracionRay;            // Muestra el rayo de depuración OPCIONALMENTE.
        public float LongitudDeDepuracionRay = 5f;   // Depuración longitud ray.
        public float DuracionDeDepuracionRay = 1f;   // La duración de la depuración de rayos permanecerá visible.
        public float LongitudDeRay = 500f;           // ¿En qué fase de la scene el rayo está echada.

        public InteractivoVR InteractibleActual; // El elemento actual interactivo
        public InteractivoVR InteractibleUltima; // El último elemento interactivo

        // Utilidad para demás clases para obtener el elemento actual interactivo
        public InteractivoVR InteractivoActual
        {
            get { return InteractibleActual; }
        }

        private void OnEnable()
        {
            EntradaVR.AlHacerClick += HagaClickEnManejar;
            EntradaVR.AlHacerDobleClick += DobleClickEnManejar;
            EntradaVR.HaciaArriba += ManejarHasta;
            EntradaVR.HaciaAbajo += ManijaHaciaAbajo;
        }


        private void OnDisable()
        {
            EntradaVR.AlHacerClick -= HagaClickEnManejar;
            EntradaVR.AlHacerDobleClick -= DobleClickEnManejar;
            EntradaVR.HaciaArriba -= ManejarHasta;
            EntradaVR.HaciaAbajo -= ManijaHaciaAbajo;
        }


        private void Update()
        {
            OjoRaycast();
        }


        public void OjoRaycast()
        {
            // Muestra el rayo de depuración, si es necesario
            if (MostrarDepuracionRay)
            {
                Debug.DrawRay(Camara.position, Camara.forward * LongitudDeDepuracionRay, Color.blue, DuracionDeDepuracionRay);
            }

            // Crear un rayo que apunte hacia delante de la cámara.
            Ray RayCast = new Ray(Camara.position, Camara.forward);
            RaycastHit hit;

            // Hacer las forweards Raycast para ver si llegamos a un elemento interactivo
            if (Physics.Raycast(RayCast, out hit, LongitudDeRay, ~CapasDeExclucion))
            {

                InteractivoVR Interactible = hit.collider.GetComponent<InteractivoVR>(); // El intento por obtener la VRInteractiveItem golpeado en el objeto
                InteractibleActual = Interactible;

                // Si llegamos a un elemento interactivo y no tienen el último elemento interactivo, entonces llamada a través de
                if (Interactible && Interactible != InteractibleUltima)
                    Interactible.Entrando();

                // Desactivado el último elemento interactivo
                if (Interactible != InteractibleUltima)
                    DesactivarUltimaInteractible();
                InteractibleUltima = Interactible;

                // Algo fue golpeado, fijado en la posición de golpe.
                if (Reticula)
                    Reticula.Configurar(hit);

                if (SeGolpeoConRayCast != null)
                    SeGolpeoConRayCast(hit);
            }
            else
            {

                // Nada fue golpeado, discapacitados interactiva en el último elemento.
                DesactivarUltimaInteractible();
                InteractibleActual = null;

                // Posición del punto de mira a una distancia predeterminada.
                if (Reticula)
                    Reticula.Configurar();
            }
        }


        private void DesactivarUltimaInteractible()
        {
            if (InteractibleUltima == null)
                return;

            InteractibleUltima.Saliendo();
            InteractibleUltima = null;
        }


        private void ManejarHasta()
        {
            if (InteractibleActual != null)
                InteractibleActual.Arriba();
        }


        private void ManijaHaciaAbajo()
        {
            if (InteractibleActual != null)
                InteractibleActual.Abajo();
        }


        private void HagaClickEnManejar()
        {
            if (InteractibleActual != null)
                InteractibleActual.Click();
        }


        private void DobleClickEnManejar()
        {
            if (InteractibleActual != null)
                InteractibleActual.DobleClick();

        }
    }
}