using System.Collections;
using UnityEngine;

/*  Script para crear un spawner de objetos, util para generar enemigos en el mapa del juego
    ## USO
    - Agregar este script a un objeto vacio y asignar valores desde el inspector
    - Colocar en escena, seleccionar la casilla de activacion y ejecutar el juego
    @Autor Oscar JL Plata 
    @Version 1.2
*/
public class Generador : MonoBehaviour
{
    public bool activador; //booleano activador del generador

    public Vector2 rangoSpawn; //Cuadrado en el que puede generar objetos

    public GameObject objetoAGenerar; //Prefab del objeto a spawnear, agregar desde el inspector

    public float tiempoSpawn; //Segundos de descanzo entre hordas o spawns

    public int cantidad; //Objetos maximos que puede spawnear

    public bool cantidadFija; // Cambiar modo siempre generar cantidad fija o en un rango de[1 a cantidad]

    public ParticleSystem efectoSpawn;

    // Update is called once per frame
    void Update()
    {
        if (activador) StartCoroutine(Spawnear()); //Llamda a corrutina para spawnear si esta activado
    }

    private IEnumerator Spawnear()
    {
        activador = false; //desactivar el generador
        int totalEnemigos = cantidad;
        if (!cantidadFija) totalEnemigos = (int) Random.Range(1f, (float) cantidad); //Calcula al azar cuantos objetos se van a generar en esta horda
        for (int i = 0; i < totalEnemigos; i++)
        {
            yield return new WaitForSeconds(.1f); //tiempo de espera entre hordas
            Vector3 posicion = new Vector3(transform.position.x + Random.Range(rangoSpawn.x * -1, rangoSpawn.x), transform.position.y + Random.Range(rangoSpawn.y * -1, rangoSpawn.y), transform.position.z);
            if (efectoSpawn != null)
            {
                efectoSpawn.transform.position = posicion;
                efectoSpawn.Play();
            }
            GameObject go = Instantiate(objetoAGenerar, posicion, Quaternion.identity); //generar objeto en escena
            go.transform.parent = this.transform; //hacer objetos hijos del spawner
        }
        yield return new WaitForSeconds(tiempoSpawn); //tiempo de espera entre hordas
        activador = true; //reactivar el generardor
    }

    //Dibujar Gizmo para ver el rango de spawneo en pantalla
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(transform.position, new Vector2(rangoSpawn.x * 2, rangoSpawn.y * 2));
    }
}
