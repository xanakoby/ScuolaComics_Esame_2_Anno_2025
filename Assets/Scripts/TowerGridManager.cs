using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerGridManager : MonoBehaviour, ISubscriber
{
    public enum CellType { Empty, Road, Tree, Tower }

    [Header("Griglia")]
    public int width = 10;
    public int height = 10;
    public float cellSize = 1f;

    [Header("Impostazioni manuali")]
    public List<Vector2Int> roadCells = new List<Vector2Int>();
    public List<Vector2Int> treeCells = new List<Vector2Int>();

    [Header("Editor Brush")]
    public CellType brushType = CellType.Road;

    [Header("Prefab e Torre")]
    public GameObject cellPrefab;
    public Transform gridParent;
    public Transform towerParent;

    private CellType[,] grid;
    private GameObject[,] gridVisual;

    private TurretButton selectedTurretButton;

    private bool canPlaceTurret;
    private bool turretHit;

    [Header("Selezioni")]
    public GameObject selectedTurret;
    public TurretController selectedTurretController;

    private void OnValidate()
    {
        InitGrid();
    }

    private void Start()
    {
        InitGrid();

        Publisher.Subscribe(this, typeof(SellTurretMessage));
    }

    private void InitGrid()
    {
        grid = new CellType[width, height];

        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                grid[x, y] = CellType.Empty;

        foreach (var c in roadCells)
            if (InBounds(c)) grid[c.x, c.y] = CellType.Road;

        foreach (var c in treeCells)
            if (InBounds(c)) grid[c.x, c.y] = CellType.Tree;
    }

    private void Update()
    {
        if (canPlaceTurret)
        {
            if (selectedTurretButton == null) return;

            UpdateCellHighlights();

            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2Int cell = WorldToCell(mouseWorldPos);
                TryPlaceTower(cell);
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Debug.Log(mouseWorldPos);

                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    //faccio prima un check se colpisco un qualcosa di ui, se si return
                    turretHit = TryCheckTower(mouseWorldPos);
                    if (turretHit)
                    {
                        //prima la deseleziono e poi
                        if (selectedTurretController != null)
                        {
                            selectedTurretController.TurretDeselected();
                            selectedTurretController = null;
                        }

                        //torre selezionata
                        selectedTurretController = selectedTurret.GetComponent<TurretController>();

                        selectedTurretController.TurretSelected();
                    }
                    else
                    {
                        //deseleziono la torretta
                        if (selectedTurretController != null)
                        {
                            selectedTurretController.TurretDeselected();
                        }

                        selectedTurret = null;
                        selectedTurretController = null;
                    }
                }
            }
        }
    }

    private void CreateVisualGrid()
    {
        ClearVisualGrid();

        gridVisual = new GameObject[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 cellPos = transform.position + new Vector3(x * cellSize, y * cellSize);
                GameObject cell = Instantiate(cellPrefab, cellPos, Quaternion.identity, gridParent);
                gridVisual[x, y] = cell;
            }
        }
    }

    private void ClearVisualGrid()
    {
        if (gridVisual == null) return;

        foreach (var go in gridVisual)
            if (go != null) Destroy(go);

        gridVisual = null;
    }

    private void UpdateCellHighlights()
    {
        if (gridVisual == null) return;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var rend = gridVisual[x, y].GetComponentInChildren<SpriteRenderer>();
                if (grid[x, y] == CellType.Empty)
                    rend.color = new Color(0, 1, 0, 0.5f);
                else
                    rend.color = new Color(1, 0, 0, 0.5f);
            }
        }
    }

    public void SelectTurret(TurretButton turretButton)
    {
        selectedTurretButton = turretButton;
        canPlaceTurret = true;
        CreateVisualGrid();
    }

    public void CancelTurretSelection()
    {
        selectedTurretButton = null;
        canPlaceTurret = false;
        ClearVisualGrid();
    }

    private void TryPlaceTower(Vector2Int cell)
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (!InBounds(cell)) return;
        if (grid[cell.x, cell.y] != CellType.Empty) return;

        if (GameManager.Instance.SpendCoins(selectedTurretButton.cost))
        {
            Vector3 spawnPos = transform.position + new Vector3(cell.x * cellSize + cellSize / 2f, cell.y * cellSize + cellSize / 2f);

            //tolgo instantiate e piazzo la torre col pooler
            GameManager.Instance.SpawnTurret(selectedTurretButton.turretPrefab, spawnPos, Quaternion.identity);
            //Instantiate(selectedTurretButton.TurretPrefab, spawnPos, Quaternion.identity, towerParent);

            grid[cell.x, cell.y] = CellType.Tower;
        }
    }

    private bool TryCheckTower(Vector2 _mousePoint)
    {
        //eventualmente da modificare con un boxoverlap nella cella cliccata
        RaycastHit2D hit = Physics2D.Raycast(_mousePoint, _mousePoint, Mathf.Infinity);
        if (hit.collider == null)
            return false;

        TurretController turretController = hit.collider.transform.gameObject.GetComponent<TurretController>();
        if (turretController != null)
        {
            selectedTurret = hit.collider.gameObject;
            return true;
        }
        else
            return false;
    }

    private bool InBounds(Vector2Int c) =>
        c.x >= 0 && c.x < width && c.y >= 0 && c.y < height;

    public Vector2Int WorldToCell(Vector3 worldPos)
    {
        int x = Mathf.FloorToInt((worldPos.x - transform.position.x) / cellSize);
        int y = Mathf.FloorToInt((worldPos.y - transform.position.y) / cellSize);
        return new Vector2Int(x, y);
    }

    public void ToggleCell(Vector2Int cell)
    {
        if (!InBounds(cell)) return;

        switch (brushType)
        {
            case CellType.Road:
                if (roadCells.Contains(cell)) roadCells.Remove(cell);
                else roadCells.Add(cell);
                break;

            case CellType.Tree:
                if (treeCells.Contains(cell)) treeCells.Remove(cell);
                else treeCells.Add(cell);
                break;
        }

        OnValidate();
    }

    public void SellSelectedTower()
    {
        Debug.Log("venduto3");
        if (selectedTurretController != null)
        {
            Vector2Int cellPoint = WorldToCell(selectedTurret.transform.position);
            if (InBounds(cellPoint))
            {
                grid[cellPoint.x, cellPoint.y] = CellType.Empty;
            }

            Debug.Log("venduto2");
            selectedTurretController.SellTurret();
        }
    }

    private void OnDrawGizmos()
    {
        if (grid == null) InitGrid();

        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
            {
                Vector3 cellCenter = transform.position + new Vector3(x + 0.5f, y + 0.5f) * cellSize;
                switch (grid[x, y])
                {
                    case CellType.Empty: Gizmos.color = Color.green; break;
                    case CellType.Road: Gizmos.color = Color.gray; break;
                    case CellType.Tree: Gizmos.color = new Color(0.4f, 0.2f, 0f); break;
                    case CellType.Tower: Gizmos.color = Color.blue; break;
                }
                Gizmos.DrawWireCube(cellCenter, Vector3.one * (cellSize * 0.9f));
            }
    }

    public void OnPublish(IPublisherMessage message)
    {
        if (message is SellTurretMessage _turretInfoMessage)
        {
            SellSelectedTower();
        }
    }

    public void OnDisableSubscriber()
    {
        Publisher.Unsubscribe(this, typeof(TurretInfoMessage));
    }
    private void OnDestroy()
    {
        OnDisableSubscriber();
    }
}
