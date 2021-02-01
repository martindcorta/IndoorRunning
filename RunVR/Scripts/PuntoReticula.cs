using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PuntoReticula : MonoBehaviour
{

    public float DistanciaPorDefecto;                // La distancia predeterminada lejos de la cámara se coloca la retícula.
    public bool UsoNormal;                           // Si los deberes del retículo colocado en paralelo a una superficie.
    public Image ImagenDelPunto;                     // Referencia al componente de imagen que representa la retícula.
    public Transform TransformacionDeReticula;       // Tenemos que afectan a la retícula de transformación.
    public Transform Camara;                         // La retícula se coloca siempre en relación con la cámara.

    private Vector3 EscalaOriginal;                   // Como la escala de la retícula de cambio, la escala original tiene que ser almacenado.
    private Quaternion RotacionOriginal;              // Se utiliza para almacenar la rotación original de la retícula.


    public bool UsarNormal
    {
        get { return UsoNormal; }
        set { UsoNormal = value; }
    }


    public Transform TransformacionDelPunto { get { return TransformacionDeReticula; } }


    private void Awake()
    {
        // Almacena la escala original y rotación.
        EscalaOriginal = TransformacionDeReticula.localScale;
        RotacionOriginal = TransformacionDeReticula.localRotation;
    }


    public void OcultarPunto()
    {
        ImagenDelPunto.enabled = false;
    }


    public void MostrarPunto()
    {
        ImagenDelPunto.enabled = true;
    }


    // Esta sobrecarga de SetPosition se utiliza cuando el Raycaster no ha tocado nada.
    public void Configurar()
    {
        // Establecer la posición del punto de mira a la distancia predeterminada en frente de la cámara.
        TransformacionDeReticula.position = Camara.position + Camara.forward * DistanciaPorDefecto;

        // Establecer la escala basada en el original y la distancia de la cámara.
        TransformacionDeReticula.localScale = EscalaOriginal * DistanciaPorDefecto;

        // Los deberes de rotación sólo ser el valor por defecto.
        TransformacionDeReticula.localRotation = RotacionOriginal;
    }


    // Esta sobrecarga de SetPosition se utiliza cuando el Raycaster ha golpeado algo.
    public void Configurar(RaycastHit hit)
    {
        TransformacionDeReticula.position = hit.point;
        TransformacionDeReticula.localScale = EscalaOriginal * hit.distance;

        // Si los deberes del retículo utilizan la normal de lo que ha sido golpeado.
        if (UsoNormal)
            // Conjunto ... es la rotación sobre la base de que está mirando hacia adelante a lo largo del vector normal.
            TransformacionDeReticula.rotation = Quaternion.FromToRotation(Vector3.forward, hit.normal);
        else
            // Sin embargo, si no está utilizando el local normal Entonces es la rotación deberías ser como era originalmente.
            TransformacionDeReticula.localRotation = RotacionOriginal;
    }
}