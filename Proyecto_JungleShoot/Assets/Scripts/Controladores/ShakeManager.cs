using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class ShakeManager : MonoBehaviour
{
    public static ShakeManager Instance;

    [Header("Camara")]
    public CinemachineVirtualCamera cv;

    public float fuerzaTemblor = 10;

    public float tiempoTemblor = 0.5f;

    public bool vibrando;

    CinemachineBasicMultiChannelPerlin cvRuido;

    public bool enJuego;

    void Awake()
    {
        if (ShakeManager.Instance == null)
        {
            ShakeManager.Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy (gameObject);
        }
        cv = GameObject.FindGameObjectWithTag("CamaraVirtual").GetComponent<CinemachineVirtualCamera>();
        if (cv != null) cvRuido = cv.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        enJuego = true;
    }

    private void Update()
    {
        if (cv == null && enJuego)
        {
            cv = GameObject.FindGameObjectWithTag("CamaraVirtual").GetComponent<CinemachineVirtualCamera>();
            if (cv != null) cvRuido = cv.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }
    }

    public void agitarCamara()
    {
        StartCoroutine(AgitarCamara());
    }

    public void agitarCamara(float fuerza, float tiempo)
    {
        StartCoroutine(AgitarCamara(fuerza, tiempo));
    }

    private IEnumerator AgitarCamara()
    {
        vibrando = true;
        cvRuido.m_AmplitudeGain = fuerzaTemblor;
        yield return new WaitForSeconds(tiempoTemblor);
        cvRuido.m_AmplitudeGain = 0;
        vibrando = false;
    }

    private IEnumerator AgitarCamara(float f, float t)
    {
        vibrando = true;
        cvRuido.m_AmplitudeGain = f;
        yield return new WaitForSeconds(t);
        cvRuido.m_AmplitudeGain = 0;
        vibrando = false;
    }
}
