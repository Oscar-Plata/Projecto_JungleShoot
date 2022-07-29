using UnityEngine;
using UnityEngine.Audio;

public class soundManager : MonoBehaviour
{
    public static soundManager Instance;

    [SerializeField]
    AudioSource[] AS; //0: Fondo, 1: EfectosPlayer, 2: EfectosEnemigo, 3:UI

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

    public void PlayEnemigo(AudioClip sonido, float distorsion)
    {
        AS[2].clip = sonido;
        AS[2].pitch = distorsion;
        AS[2].PlayOneShot(sonido);
    }

    public void StopFondo()
    {
        AS[0].Stop();
    }

    public void PausarSonidos()
    {
        AS[0].Pause();
        AS[1].Pause();
        AS[2].Pause();
    }

    public void ReanudarSonidos()
    {
        AS[0].UnPause();
        AS[1].UnPause();
        AS[2].UnPause();
    }

    public void PlayUI(AudioClip sonido)
    {
        AS[3].clip = sonido;
        AS[3].Play();
    }
}
