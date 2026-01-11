using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


public class TerritoryEditorClickHandler : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private TerritoryEditorController editor;   // your UI controller (mode + selected country)
    [SerializeField] private CountryManager countryManager;
    [SerializeField] private TerritoryManager territoryManager;

    // Optional but recommended: set your tile prefab layer to "HexTile"
    [SerializeField] private LayerMask tileLayerMask = ~0;

    private void Awake()
    {
        if (mainCamera == null) mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
            HandleClick();

    }

    private void HandleClick()
    {
        if (editor == null || countryManager == null || territoryManager == null) return;

        // prevent placing when clicking UI
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;


        if (editor.CurrentMode == TerritoryEditorController.ToolMode.None)
            return;

        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray ray = mainCamera.ScreenPointToRay(mousePos);

        if (!Physics.Raycast(ray, out RaycastHit hit, 10000f, tileLayerMask))
            return;

        HexTileView view = hit.collider.GetComponent<HexTileView>();
        if (view == null || view.Tile == null) return;

        HexTile tile = view.Tile;

        // ensure runtime Country exists
        CountryDefinition def = editor.SelectedCountryDef;
        if (def == null) return;

        Country country = countryManager.GetCountry(def.Id);
        if (country == null) country = countryManager.CreateFromDefinition(def);

        // apply tool
        switch (editor.CurrentMode)
        {
            case TerritoryEditorController.ToolMode.SpawnCapital:
                // IMPORTANT: change this line to match your TerritoryManager spawn method name/signature
                territoryManager.SpawnCountryAtTile(country, tile);
                break;

            case TerritoryEditorController.ToolMode.Claim:
                territoryManager.ClaimTile(tile, country);
                break;

            case TerritoryEditorController.ToolMode.Unclaim:
                territoryManager.UnclaimTile(tile, country);
                break;
        }

        // repaint tile after change
        HexGridRenderer.Instance.ApplyColor(tile);
    }
}
