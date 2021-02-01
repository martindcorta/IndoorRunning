using System;
using UnityEngine;

namespace RayCaster.Utils
{
    public class EntradaVR : MonoBehaviour
    {
        //Swipe directions
        public enum SwipeDirection
        {
            NINGUNA,
            ARRIBA,
            ABAJO,
            IZQUIERDA,
            DERECHA
        };


        public event Action<SwipeDirection> AlDeslizar;             // Called every frame passing in the swipe, including if there is no swipe.
        public event Action AlHacerClick;                           // Called when Fire1 is released and it's not a double click.
        public event Action HaciaAbajo;                             // Called when Fire1 is pressed.
        public event Action HaciaArriba;                            // Called when Fire1 is released.
        public event Action AlHacerDobleClick;                      // Called when a double click is detected.
        public event Action AlCancelar;                             // Called when Cancel is pressed.


        public float TiempoDeDobleClick = 0.3f;                     //The max time allowed between double clicks
        public float AnchoDeDeslizamiento = 0.3f;                   //The width of a swipe

        
        private Vector2 PosicionDelMouseHaciaAbajo;                 // The screen position of the mouse when Fire1 is pressed.
        private Vector2 PosicionDelMouseHaciaArriba;                // The screen position of the mouse when Fire1 is released.
        private float UltimaHoraDelMouse;                           // The time when Fire1 was last released.
        private float UltimoValorHorizontal;                        // The previous value of the horizontal axis used to detect keyboard swipes.
        private float UltimoValorVertical;                          // The previous value of the vertical axis used to detect keyboard swipes.


        public float TiempoDobleClick { get { return TiempoDeDobleClick; } }


        private void Update()
        {
            ComprobarEntrada();
        }


        private void ComprobarEntrada()
        {
            // Set the default swipe to be none.
            SwipeDirection GolpeFuerte = SwipeDirection.NINGUNA;

            if (Input.GetButtonDown("Fire1"))
            {
                // When Fire1 is pressed record the position of the mouse.
                PosicionDelMouseHaciaAbajo = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            
                // If anything has subscribed to OnDown call it.
                if (HaciaAbajo != null)
                    HaciaAbajo();
            }

            // This if statement is to gather information about the mouse when the button is up.
            if (Input.GetButtonUp ("Fire1"))
            {
                // When Fire1 is released record the position of the mouse.
                PosicionDelMouseHaciaArriba = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);

                // Detect the direction between the mouse positions when Fire1 is pressed and released.
                GolpeFuerte = DetectarDeslizar ();
            }

            // If there was no swipe this frame from the mouse, check for a keyboard swipe.
            if (GolpeFuerte == SwipeDirection.NINGUNA)
                GolpeFuerte = DetectarElGolpeDeTecladoEmulado();

            // If there are any subscribers to OnSwipe call it passing in the detected swipe.
            if (AlDeslizar != null)
                AlDeslizar(GolpeFuerte);

            // This if statement is to trigger events based on the information gathered before.
            if(Input.GetButtonUp ("Fire1"))
            {
                // If anything has subscribed to OnUp call it.
                if (HaciaArriba != null)
                    HaciaArriba();

                // If the time between the last release of Fire1 and now is less
                // than the allowed double click time then it's a double click.
                if (Time.time - UltimaHoraDelMouse < TiempoDeDobleClick)
                {
                    // If anything has subscribed to OnDoubleClick call it.
                    if (AlHacerDobleClick != null)
                        AlHacerDobleClick();
                }
                else
                {
                    // If it's not a double click, it's a single click.
                    // If anything has subscribed to OnClick call it.
                    if (AlHacerClick != null)
                        AlHacerClick();
                }

                // Record the time when Fire1 is released.
                UltimaHoraDelMouse = Time.time;
            }

            // If the Cancel button is pressed and there are subscribers to OnCancel call it.
            if (Input.GetButtonDown("Cancel"))
            {
                if (AlCancelar != null)
                    AlCancelar();
            }
        }

        private SwipeDirection DetectarDeslizar ()
        {
            // Get the direction from the mouse position when Fire1 is pressed to when it is released.
            Vector2 DeslizarLosDatos = (PosicionDelMouseHaciaArriba - PosicionDelMouseHaciaAbajo).normalized;

            // If the direction of the swipe has a small width it is vertical.
            bool ElGolpeEsVertical = Mathf.Abs (DeslizarLosDatos.x) < AnchoDeDeslizamiento;

            // If the direction of the swipe has a small height it is horizontal.
            bool ElGolpeEsHorizontal = Mathf.Abs(DeslizarLosDatos.y) < AnchoDeDeslizamiento;

            // If the swipe has a positive y component and is vertical the swipe is up.
            if (DeslizarLosDatos.y > 0f && ElGolpeEsVertical)
                return SwipeDirection.ARRIBA;

            // If the swipe has a negative y component and is vertical the swipe is down.
            if (DeslizarLosDatos.y < 0f && ElGolpeEsVertical)
                return SwipeDirection.ABAJO;

            // If the swipe has a positive x component and is horizontal the swipe is right.
            if (DeslizarLosDatos.x > 0f && ElGolpeEsHorizontal)
                return SwipeDirection.DERECHA;

            // If the swipe has a negative x component and is vertical the swipe is left.
            if (DeslizarLosDatos.x < 0f && ElGolpeEsHorizontal)
                return SwipeDirection.IZQUIERDA;

            // If the swipe meets none of these requirements there is no swipe.
            return SwipeDirection.NINGUNA;
        }


        private SwipeDirection DetectarElGolpeDeTecladoEmulado()
        {
            // Store the values for Horizontal and Vertical axes.
            float Horizontal = Input.GetAxis ("Horizontal");
            float Vertical = Input.GetAxis ("Vertical");

            // Store whether there was horizontal or vertical input before.
            bool NoEntradaHorizontalPreviamente = Mathf.Abs (UltimoValorHorizontal) < float.Epsilon;
            bool NoEntradaVerticalPreviamente = Mathf.Abs(UltimoValorVertical) < float.Epsilon;

            // The last horizontal values are now the current ones.
            UltimoValorHorizontal = Horizontal;
            UltimoValorVertical = Vertical;

            // If there is positive vertical input now and previously there wasn't the swipe is up.
            if (Vertical > 0f && NoEntradaVerticalPreviamente)
                return SwipeDirection.ARRIBA;

            // If there is negative vertical input now and previously there wasn't the swipe is down.
            if (Vertical < 0f && NoEntradaVerticalPreviamente)
                return SwipeDirection.ABAJO;

            // If there is positive horizontal input now and previously there wasn't the swipe is right.
            if (Horizontal > 0f && NoEntradaHorizontalPreviamente)
                return SwipeDirection.DERECHA;

            // If there is negative horizontal input now and previously there wasn't the swipe is left.
            if (Horizontal < 0f && NoEntradaHorizontalPreviamente)
                return SwipeDirection.IZQUIERDA;

            // If the swipe meets none of these requirements there is no swipe.
            return SwipeDirection.NINGUNA;
        }
        

        private void OnDestroy()
        {
            // Ensure that all events are unsubscribed when this is destroyed.
            AlDeslizar = null;
            AlHacerClick = null;
            AlHacerDobleClick = null;
            HaciaAbajo = null;
            HaciaArriba = null;
        }
    }
}