using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Token : MonoBehaviour
{
    private GridManager gridManager;

    void Start()
    {
        // Pega a refer�ncia do GridManager
        gridManager = FindObjectOfType<GridManager>();
    }

    // M�todo para mover o token para uma c�lula na grid
    public void MoveToPosition(Vector2 worldPosition)
    {
        // Converte a posi��o mundial para a c�lula mais pr�xima
        Vector2 cellPosition = gridManager.GetNearestCellPosition(worldPosition);

        // Move o token para a c�lula
        transform.position = new Vector3(cellPosition.x, cellPosition.y, 0);
    }
}

