using Code.Scripts;

namespace Code.Models
{
    public class TilePathfindingData
    {
        public Tile DestinationTile { get; }
        /// <summary>
        /// The pathfinding data of the connected tile with the lowest MoveCost
        /// </summary>
        public TilePathfindingData ClosestSourceTilePathfindingData { get; }
        /// <summary>
        /// The lowest cost we must pay to move to reach the DestinationTile from the beginning of the path
        /// </summary>
        public int MoveCost { get; }
        /// <summary>
        /// The DestinationTile's transform distance from the goal
        /// </summary>
        private float TransformDistanceFromGoal { get; }
        /// <summary>
        /// The Sum of the MoveCost and DistanceFromGoal of the DestinationTile
        /// </summary>
        public float TotalTilePathCost => MoveCost+TransformDistanceFromGoal;

        public TilePathfindingData(Tile destinationTile,TilePathfindingData closestSourceTilePathfindingData, int moveCost, float transformDistanceFromGoal)
        {
            DestinationTile = destinationTile;
            ClosestSourceTilePathfindingData = closestSourceTilePathfindingData;
            MoveCost = moveCost;
            TransformDistanceFromGoal = transformDistanceFromGoal;
        }

    }
}
