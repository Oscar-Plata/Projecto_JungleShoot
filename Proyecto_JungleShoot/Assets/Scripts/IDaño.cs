//Interfaz que permite danar o curar vida de objetos
public interface IDaño
{
    void RecibirDaño(float cantidad);

    void Morir(bool check);

    void CurarVida(float cantidad);
}
