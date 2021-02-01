using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;



public class correrfun : MonoBehaviour
{

    public GameObject VisionVR;
    public const int AnguloRecto = 90;
    public bool EstaCaminando = false;
    public bool Paro = true;
    public float Velocidad;
    public bool Caminaralpulsar; //al pulsar
    public bool Caminaralmirar;
    public double AngulodeUmbral;
    public bool CongelarLaPosicionY;
    public float CompensarY;
    public Image UIImagen;
    System.IO.Ports.SerialPort ArduinoPort;
    string lectura;
    public Text speed;
    public Text tiempoText;
    public float tiempo = 0.0f;




    public void Caminar()
    {
        Vector3 direccion = new Vector3(VisionVR.transform.forward.x, 0, VisionVR.transform.forward.z).normalized * Velocidad * Time.deltaTime;
        Quaternion Rotacion = Quaternion.Euler(new Vector3(0, transform.rotation.eulerAngles.y, 0));
        transform.Translate(Rotacion * direccion);
        
    }

    public void leer()
    {
        lectura = ArduinoPort.ReadLine();
        if (lectura == "c")
        {
          UIImagen.sprite = Resources.Load<Sprite>("vidas");
        }

        if (lectura != "c")
        {

            speed.text = "" + lectura + " km/h";
            UIImagen.sprite = Resources.Load<Sprite>("corazon2");
        }
        
        ArduinoPort.Close();
    }
    // Update is called once per frame
    void Update()
    {
        //crear Serial Port
        ArduinoPort = new System.IO.Ports.SerialPort();
        ArduinoPort.PortName = "COM4";
        ArduinoPort.BaudRate = 9600;
        UIImagen = GameObject.Find("corazon").GetComponent<Image>();
        ArduinoPort.Open();
        leer();
        tiempo = tiempo + 1 * Time.deltaTime;
        tiempoText.text = "00:" + tiempo.ToString("f0") + " s";

        if (Caminaralmirar && !Caminaralpulsar && !EstaCaminando && VisionVR.transform.eulerAngles.x >= AngulodeUmbral && VisionVR.transform.eulerAngles.x <= AnguloRecto)
        {

            EstaCaminando = true;


        }
        else if (Caminaralmirar && !Caminaralpulsar && EstaCaminando && (VisionVR.transform.eulerAngles.x <= AngulodeUmbral || VisionVR.transform.eulerAngles.x >= AnguloRecto))
        {

            EstaCaminando = false;


        }

        if (EstaCaminando)
        {
            Caminar();
        }

        if (CongelarLaPosicionY)
        {
            transform.position = new Vector3(transform.position.x, CompensarY, transform.position.z);

        }


    }
}
