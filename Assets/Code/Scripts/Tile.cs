using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class Tile : MonoBehaviour
{
    public TerrainType TerrainType;
    public Unit CurrentUnit;
    public Vector3 PositionInGrid;
    [SerializeField]
    private SpriteRenderer gridSpriteRenderer = null;

    public void Start()
    {
    }

    public void ChangeGridSprite(Sprite newGridSprite)
    {
        gridSpriteRenderer.sprite = newGridSprite;
    }
}
