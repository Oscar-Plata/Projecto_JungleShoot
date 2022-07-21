using UnityEngine;

public class MovimientoNubes : MonoBehaviour
{
    private Vector3 posInicio;

    public Transform target;

    public float velocidad;

    public int distancia;

    //public float dif;
    void Start()
    {
        posInicio = this.GetComponent<Transform>().position;
    }

    void Update()
    {
        transform.Translate(Vector3.right * velocidad * Time.deltaTime);

        //dif = transform.position.x - target.position.x;
        if (transform.position.x - target.position.x > distancia) transform.position = new Vector3(target.position.x - distancia * 3, posInicio.y + Random.Range(-1.0f, 1.0f), posInicio.z);
    }
}
