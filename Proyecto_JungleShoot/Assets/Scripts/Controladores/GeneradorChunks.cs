using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneradorChunks : MonoBehaviour
{
    public List<GameObject> chunks;

    public GameObject[] lista;

    public int cantidadChunks;

    public string semilla = "";

    private int selector;

    private int selAnterior = -1;

    public Transform posicionInicial;

    public Vector2 offset;

    public AudioClip musicaFondo;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        //generarSemilla
        if (semilla == null || semilla.Equals(""))
        {
            for (int i = 0; i < cantidadChunks; i++)
            {
                do
                {
                    selector = (int) Random.Range(0, lista.Length);
                }
                while (selector == selAnterior);
                selAnterior = selector;
                Debug.Log("Sel" + selector);
                semilla = semilla + selector.ToString();
            }
        }

        semilla = semilla.Trim();
        Debug.Log("Semilla: " + semilla);
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        Transform posChunk = posicionInicial;
        int aux = 0;
        for (int i = 0; i < semilla.Length; i++)
        {
            aux = semilla[i] - '0';
            Debug.Log (aux);
            chunks.Add((GameObject) Instantiate(lista[aux], posChunk.position, Quaternion.identity));
            posChunk.position = new Vector3(posChunk.position.x + offset.x, posChunk.position.y + offset.y, posChunk.position.z);
        }
        soundManager.Instance.PlayFondo (musicaFondo);
        ShakeManager.Instance.enJuego = true;
    }
}
