using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectObjectsWave : MonoBehaviour
{
    private float raioAtual = 0f;
    private float tempoAumento = 0.5f;
    public SobelController sobelController;
    // Update is called once per frame
    void Update()
    {
        raioAtual += Time.deltaTime * tempoAumento;

        // Atualize o Collider da área de detecção.
        Collider colisor = GetComponent<Collider>();
        if (colisor is SphereCollider)
        {
            ((SphereCollider)colisor).radius = raioAtual;
        }

        // Detete objetos filhos dentro da área de detecção.
        Collider[] objetosDetectados = Physics.OverlapSphere(transform.position, raioAtual);

        foreach (Collider objeto in objetosDetectados)
        {
                // Faça algo com os objetos filhos detectados.
                Debug.Log("Objeto filho detectado: " + objeto.name);
                //sobelController.EnableSobel(objeto.gameObject);
                //Invoke("disableSobel", durationObject + 0.5f);
        }
    }
}
