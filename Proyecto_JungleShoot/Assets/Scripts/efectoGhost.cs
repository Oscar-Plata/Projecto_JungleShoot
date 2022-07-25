using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class efectoGhost : MonoBehaviour
{
    public GameObject Padre;

    public float tiempoVida;

    public Color color;

    private SpriteRenderer sr;

    private SpriteRenderer srOtro;

    private Transform trfOtro;

    public Animation an;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        an = GetComponent<Animation>();
        srOtro = Padre.GetComponent<SpriteRenderer>();
        trfOtro = Padre.GetComponent<Transform>();
    }

    // Start is called before the first frame update
    void Start()
    {
        sr.sprite = srOtro.sprite;
        sr.color = color;
    }
}
