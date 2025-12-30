
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class PathRenderer : MonoBehaviour
{
    private LineRenderer lr;

    [Header("References")]
    [SerializeField] private GameObject TruckPrefab;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }

    void LineProperties()
    {
        // 1. Setup how the line looks
        lr.startWidth = 0.5f;
        lr.endWidth = 0.5f;

        // SMOOTHING SETTINGS:
        lr.numCapVertices = 5;     // Rounds the End Points
        lr.numCornerVertices = 5;  // Rounds the Bends/Corners 
        
    }

    void Start()
    {
        PathFinder pathFinder = new PathFinder();
        LineProperties();

        List<HexTile> path = pathFinder.ShortestPathData(
            CountryManager.Instance.GetCountry(1).ClaimedTiles[0], 
            CountryManager.Instance.GetCountry(2).ClaimedTiles[0]);
        
        List<Vector3> vectors = new List<Vector3>();
                // 2. Define our points (Connect the dots)

        foreach (HexTile tile in path)
        {
            Vector3 worldPosition = HexGridRenderer.Instance.GetWorldPosition(tile);
            worldPosition.y += 0.1f;
            vectors.Add(worldPosition);
        }
        

        // 3. Tell the renderer we have x points
        lr.positionCount = vectors.Count;

        // 4. Assign them (Index, Position)
        for (int i = 0; i < vectors.Count; i++)
        {
            lr.SetPosition(i, vectors[i]);
        }
        //Logic to spawn truck
        if (vectors.Count > 0 && TruckPrefab != null)
        {
            // Spawn at the start position
            GameObject truck = Instantiate(TruckPrefab, vectors[0], Quaternion.identity);
            
            // Get the script and give it the path
            TruckMover mover = truck.GetComponent<TruckMover>();
            if (mover != null)
            {
                mover.SetPath(vectors);
            }
        }
        
    }




}
