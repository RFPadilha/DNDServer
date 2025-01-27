using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] PlacementInputManager placementInputManager;
    [SerializeField] Grid grid;

    [SerializeField] ObjectsDatabaseSO database;
    [SerializeField] GameObject gridVisualization;

    private GridData floorData;
    private GridData furnitureData;

    [SerializeField] private PreviewSystem preview;
    [SerializeField] private ObjectPlacer objectPlacer;
    private Vector3Int lastDetectedPosition = Vector3Int.zero;

    IBuildingState buildingState;

    private void Start()
    {
        StopPlacement();
        floorData = new();
        furnitureData = new();
    }
    public void StartPlacement(int ID)
    {
        StopPlacement();
        gridVisualization.SetActive(true);

        buildingState = new PlacementState(ID, grid, preview, database, floorData, furnitureData, objectPlacer);

        placementInputManager.OnClicked += PlaceStructure;
        placementInputManager.OnExit += StopPlacement;
    }
    public void StartRemoving()
    {
        StopPlacement();
        gridVisualization.SetActive(true);

        buildingState = new RemovingState(grid, preview, floorData, furnitureData, objectPlacer);

        placementInputManager.OnClicked += PlaceStructure;
        placementInputManager.OnExit += StopPlacement;
    }
    private void PlaceStructure()
    {
        if (placementInputManager.IsPointerOverUI()) return;

        Vector3 mousePosition = placementInputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        buildingState.OnAction(gridPosition);
    }

    //private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    //{
    //    GridData selectedData = database.objectsData[selectedObjectIndex].ID == 0 ? 
    //        floorData : 
    //        furnitureData;

    //    return selectedData.CanPlaceObjectAt(gridPosition, database.objectsData[selectedObjectIndex].Size);
    //}

    private void StopPlacement()
    {
        if (buildingState == null) return;
        gridVisualization.SetActive(false);
        buildingState.EndState();
        placementInputManager.OnClicked -= PlaceStructure;
        placementInputManager.OnExit -= StopPlacement;
        lastDetectedPosition = Vector3Int.zero;
        buildingState = null;
    }

    void Update()
    {
        if (buildingState == null) return;

        Vector3 mousePosition = placementInputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        if(lastDetectedPosition != gridPosition)
        {
            buildingState.UpdateState(gridPosition);
            lastDetectedPosition = gridPosition;
        }
    }

}
