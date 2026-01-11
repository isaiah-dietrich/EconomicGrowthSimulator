using System.Collections.Generic;
using UnityEngine;

public class HexGridRenderer : MonoBehaviour
{
    public static HexGridRenderer Instance { get; private set; }
    [Header("References")]
    [SerializeField] private WorldGenerator worldGenerator;
    [SerializeField] private GameObject TilePreFab;

    [Header("Grid Settings")]
    [Range(0.1f, 10f)]
    [SerializeField] public float HexSize = 1f;

    private readonly Dictionary<HexTile, SpriteRenderer> tileToRenderer = new();

    // Cache the square root of 3 for performance
    private readonly float SQRT3 = Mathf.Sqrt(3f);

    /// <summary>
    /// Calculates the 3D world position based on Odd-Q Flat-Top Hex Math.
    /// </summary>
    public Vector3 GetWorldPosition(HexTile tile)
    {
        // width = size * 2
        // height = sqrt(3) * size
        float width = HexSize * 2f;
        float height = SQRT3 * HexSize;

        // Horizontal spacing: columns are 3/4 of a width apart
        float horizontalSpacing = width * 0.75f;

        // Vertical spacing: rows are the full height apart
        float verticalSpacing = height;

        float xPos = tile.X * horizontalSpacing;
        float zPos = tile.Y * verticalSpacing;

        // The "HoneyComb" Offset: shift odd columns by half a height
        if (tile.X % 2 != 0)
        {
            zPos += verticalSpacing / 2f;
        }

        return new Vector3(xPos, 0, zPos);
    }

    /// <summary>
    /// Clears the grid and spawns new tiles. 
    /// Right-click the component in Inspector to call this manually via "Refresh Grid".
    /// </summary>
    [ContextMenu("Refresh Grid")]
    public void GenerateTiles()
    {
        ClearGrid();

        if (worldGenerator == null)
        {
            Debug.LogError("WorldGenerator reference missing on HexGridRenderer!");
            return;
        }

        worldGenerator.Generate();

        // Loop through the 2D array data
        foreach (HexTile tile in worldGenerator.WorldGrid.GridData)
        {
            if (tile == null) continue;

            Vector3 worldPosition = GetWorldPosition(tile);

            // Instantiating with 90-degree X rotation so 2D sprites lie flat on the 3D floor
            GameObject go = Instantiate(TilePreFab, worldPosition, Quaternion.Euler(90, 0, 0), transform);

            //Creates a clickable tile
            var view = go.GetComponent<HexTileView>();
            if (view != null) view.Init(tile);

            SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
            tileToRenderer[tile] = sr;
            ApplyColor(tile);

        }
    }

    /// <summary>
    /// Safely removes all existing tile GameObjects.
    /// </summary>
    private void ClearGrid()
    {
        // Move children to a list first to avoid collection errors
        List<GameObject> children = new List<GameObject>();
        foreach (Transform child in transform)
        {
            children.Add(child.gameObject);
        }

        foreach (GameObject child in children)
        {
            // DestroyImmediate is only used when NOT playing to avoid editor memory leaks
            if (Application.isPlaying)
                Destroy(child);
            else
                DestroyImmediate(child);
        }
    }

    /// <summary>
    /// OnValidate is useful for live-tweaking, but we check if we are in Play Mode 
    /// to avoid crashing Unity's editor while dragging sliders.
    /// </summary>
    private void OnValidate()
    {
        // Check for nulls to prevent errors during script recompilation
        if (worldGenerator != null && Application.isPlaying)
        {
            GenerateTiles();
        }
    }

    private void Awake()
    {
        Instance = this;
        GenerateTiles();
    }

    // Helper method to map your TerrainType class to Unity Colors
    private Color GetColorForTerrain(TerrainType type)
    {
        if (type == TerrainType.Plains) return new Color(0.4f, 0.8f, 0.4f); // Grass Green
        if (type == TerrainType.Mountains) return new Color(0.5f, 0.5f, 0.5f); // Stone Gray
        if (type == TerrainType.Woods) return new Color(0.1f, 0.5f, 0.1f); // Dark Forest Green

        return Color.white; // Default fallback
    }

    public void ApplyColor(HexTile tile)
    {
        if (tile == null) return;
        if (!tileToRenderer.TryGetValue(tile, out var sr) || sr == null) return;

        Color baseColor = GetColorForTerrain(tile.Terrain);

        if (tile.OwnerCountryId.HasValue && CountryManager.Instance != null)
        {
            Country owner = CountryManager.Instance.GetCountry(tile.OwnerCountryId.Value);
            if (owner != null)
            {
                sr.color = Color.Lerp(baseColor, owner.Color, 0.9f);
                return;
            }
        }

        sr.color = baseColor;
    }





}