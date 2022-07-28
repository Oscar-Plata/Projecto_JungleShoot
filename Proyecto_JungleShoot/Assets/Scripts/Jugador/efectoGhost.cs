using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class efectoGhost : MonoBehaviour
{
    //public GameObject Padre;
    public float tiempoVida;

    public Color colorGhost;

    private SpriteRenderer sr;

    public SpriteRenderer srOtro;

    public Transform trfOtro;

    public Animator an;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        an = GetComponent<Animator>();
        // srOtro = Padre.GetComponent<SpriteRenderer>();
        // trfOtro = Padre.GetComponent<Transform>();
    }

    // Start is called before the first frame update
    void Start()
    {
        sr.sprite = srOtro.sprite;
        transform.localScale = trfOtro.localScale;
        sr.color = colorGhost;
        StartCoroutine("Fade");
    }

    private IEnumerator Fade()
    {
        an.SetTrigger("Desvanecer");
        yield return new WaitForSeconds(tiempoVida);
        Destroy (gameObject);
    }
}
