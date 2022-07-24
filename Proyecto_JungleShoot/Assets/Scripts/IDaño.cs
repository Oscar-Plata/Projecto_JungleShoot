using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDaño
{
    void RecibirDaño(float cantidad);

    void Morir(bool check);

    void CurarVida(float cantidad);
}
