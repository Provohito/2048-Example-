using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    public static Field Instance;

    [Header("Field Properties")]
    public float CellSize;
    public float Spacing;
    public int FieldSize;
    public int InitCellsCount;

    [Space(10)]
    [SerializeField]
    private Cell cellPref;
    [SerializeField]
    private RectTransform rt;

    private Cell[,] field;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void CreateField()
    {
        field = new Cell[FieldSize, FieldSize];

        float fieldWidth = FieldSize * (CellSize + Spacing) + Spacing;
        rt.sizeDelta = new Vector2(fieldWidth, fieldWidth);

        float startX = -(fieldWidth / 2) + (CellSize / 2) + Spacing;
        float startY = (fieldWidth / 2) - (CellSize / 2) - Spacing;

        for (int x = 0; x < FieldSize; x++)
        {
            for (int y = 0; y < FieldSize; y++)
            {
                var cell = Instantiate(cellPref, transform, false);
                var position = new Vector2(startX + (x * (CellSize + Spacing)), startY - (y * (CellSize + Spacing)));
                cell.transform.localPosition = position;

                field[x, y] = cell;

                cell.SetValue(x, y, 0);
            }
        }
    }

    public void GenerateField()
    {
        if (field == null)
            CreateField();

        for (int x = 0; x < FieldSize; x++)
            for (int y = 0; y < FieldSize; y++)
                field[x, y].SetValue(x, y, 0);

        for (int i = 0; i < InitCellsCount; i++)
            GenerateRandomCell();
    }

    private void GenerateRandomCell()
    {
        var emptyCells = new List<Cell>();

        for (int x = 0; x < FieldSize; x++)
            for (int y = 0; y < FieldSize; y++)
                if (field[x, y].IsEmpty)
                    emptyCells.Add(field[x, y]);

        if (emptyCells.Count == 0)
            throw new System.Exception("There is no any empty cell!");

        int value = Random.Range(0, 10) == 0 ? 2 : 1;

        var cell = emptyCells[Random.Range(0, emptyCells.Count)];
        cell.SetValue(cell.X, cell.Y, value);
    }
}
