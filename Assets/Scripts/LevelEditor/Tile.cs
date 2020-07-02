using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[SelectionBase]
public class Tile : MonoBehaviour
{

    public MMEnums.TileType ttTile = MMEnums.TileType.EMPTY;
    private MMEnums.TileType ttPrevTile = MMEnums.TileType.EMPTY;

    public Color clrObject = MMConstants.WHITE;

    private bool bValueChanged = false;

    public GameObject goTile;

    public bool bMovementOn = true;

    private void Update()
    {
        if (!bValueChanged && ttTile != ttPrevTile)
        {
            print("TILE CHANGED!");
            bValueChanged = true;

            GetComponentInParent<LevelEditor>().RefreshTiles();
        }
    }

    public bool HasChangedValue()
    {
        return bValueChanged;
    }

    public void SetNewTile()
    {
        ttPrevTile = ttTile;
        bValueChanged = false;
    }

    public void Rotate(float fAngle, Vector3 v3Direction)
    {
        this.transform.rotation = (this.transform.rotation * Quaternion.AngleAxis(fAngle, v3Direction));
    }

    [ContextMenu("Rotate")]
    private void Rotate22D()
    {
        Rotate(22.5f, Vector3.up);
    }

}
