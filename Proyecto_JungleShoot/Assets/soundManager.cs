using UnityEngine;
using UnityEngine.Audio;

public class soundManager : MonoBehaviour
{
    public static soundManager Instance;

    [SerializeField]
    AudioSource[] AS; //0: Fondo, 1: Efectos

    private void Awake()
    {
        if (soundManager.Instance == null)
        {
            soundManager.Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy (gameObject);
        }
    }

    //Reproducir musica de fondo
    public void PlayFondo(AudioClip sonido)
    {
        AS[0].clip = sonido;
        AS[0].Play();
    }

    //Reproducir efectos de sonido
    public void PlayEfecto(AudioClip sonido, float distorsion)
    {
        AS[1].clip = sonido;
        AS[1].pitch = distorsion;
        AS[1].PlayOneShot(sonido);
    }
}
