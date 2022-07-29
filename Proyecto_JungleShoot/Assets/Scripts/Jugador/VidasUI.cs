using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VidasUI : MonoBehaviour
{
    public int vidaActual;

    public int vidaMaxima;

    public Image[] vidas;

    public Sprite vidaLLena;

    public Sprite vidaVacia;

    public MovimientoPlayer scriptPlayer;

    public int countinues;

    public int score;

    public TextMeshProUGUI scoreTXT;

    public TextMeshProUGUI contTXT;

    private void Awake()
    {
        scriptPlayer = GetComponent<MovimientoPlayer>();
        scriptPlayer.topeVidas = vidas.Length;
        vidaMaxima = (int) scriptPlayer.vidasTotales;
        countinues = scriptPlayer.continues;
        scoreTXT.text = "Score: 00000";
    }

    // Update is called once per frame
    void Update()
    {
        vidaActual = (int) scriptPlayer.vidas;
        vidaMaxima = (int) scriptPlayer.vidasTotales;
        countinues = scriptPlayer.continues;
        contTXT.text = countinues.ToString();
        scoreTXT.text = "Score:" + ScoreManager.Instance.score.ToString("00000");
        if (vidaActual > vidaMaxima)
        {
            vidaActual = vidaMaxima;
        }
        for (var i = 0; i < vidas.Length; i++)
        {
            if (i < vidaActual)
                vidas[i].sprite = vidaLLena;
            else
                vidas[i].sprite = vidaVacia;

            if (i < vidaMaxima)
                vidas[i].enabled = true;
            else
                vidas[i].enabled = false;
        }
    }
}
