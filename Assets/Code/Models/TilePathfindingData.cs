using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePathfindingData
{
    public Tile DestinationTile { get; set; }
    /// <summary>
    /// The pathfinding data of the connected tile with the lowest MoveCost
    /// </summary>
    public TilePathfindingData ClosestSourceTilePathfindingData { get; set; }
    /// <summary>
    /// The lowest cost we must pay to move to reach the DestinationTile from the begining of the path
    /// </summary>
    public int MoveCost { get; set; }
    /// <summary>
    /// The DestinationTile's transform distance from the goal
    /// </summary>
    public float TransformDistanceFromGoal { get; set; }
    /// <summary>
    /// The Sum of the MoveCost and DistanceFromGoal of the DestinationTile
    /// </summary>
    public float TotalTilePathCost { get { return MoveCost+TransformDistanceFromGoal; } }
}
