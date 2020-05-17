using System.Collections.Generic;
using System.Linq;
using Code.Grid;
using Code.GUI;
using Code.Helpers;
using Code.Turns;
using Code.Units;
using UnityEngine;

namespace Code.UserInput
{
    /// <summary>
    /// Handles the user input and calls the appropriate code
    /// </summary>
    public class UserInteractionManager : MonoBehaviour
    {
        private Controls PlayerInputActions;
        private bool InputSelect;
        private bool InputEndTurn;
        private bool InputCancel;
        private IEnumerable<Unit> AllUnits;
        #pragma warning disable 0649
        [SerializeField] private GridManager gridManager;
        [SerializeField] private List<Unit> playerUnits;
        [SerializeField] private List<Unit> enemyUnits;
        [SerializeField] private UnitProfileGui unitProfile;
        [SerializeField] private TileSelector tileSelector;
        [SerializeField] private UnitSelector unitSelector;
        [SerializeField] private UnitMovementHandler unitMovementHandler;
        [SerializeField] private TurnManager turnManager;
        #pragma warning restore 0649

        private void Awake()
        {
            PlayerInputActions=new Controls();
            PlayerInputActions.Gameplay.Select.started  += ctx => SelectButtonPressed();
            PlayerInputActions.Gameplay.EndTurn.started  += ctx => EndTurnButtonPressed();
            PlayerInputActions.Gameplay.Cancel.started += ctx => CancelButtonPressed(); 
        }
        private void Start()
        {
            AllUnits= playerUnits.Concat(enemyUnits);
            //currentUnit initialization in tiles
            foreach (var unit in AllUnits)
            {
                var position = unit.transform.position;
                gridManager.TileGrid[(int)position.x, (int)position.y].currentUnit=unit;
            }
        }
        private void Update()
        {
            var cursorTile = tileSelector.cursorTile;
            Unit cursorUnit = null;
            if (!ReferenceEquals(cursorTile,null))
            {
                cursorUnit = cursorTile.currentUnit;
            }

            if (tileSelector.cursorTileChanged)
            {
                //if the cursor is hovering over a unit, show the profile
                unitProfile.ShowUnitProfile(cursorUnit);
                //cursor hovered over a tile within the selected units available moves
                if (!ReferenceEquals(unitSelector.SelectedUnit, null))
                {
                    var selectedUnitPathfindingData = unitSelector.SelectedUnit.pathfindingData;
                    //If hovering within the selected unit's movement, renders the path, otherwise removes the rendered path  
                    var hoveredTilePathData = selectedUnitPathfindingData.FirstOrDefault(x => x.DestinationGridTile == tileSelector.cursorTile);
                    TileRenderingHelper.RenderPathToTile(hoveredTilePathData,selectedUnitPathfindingData);
                }
                //Hovers over a unit
                if (!ReferenceEquals(cursorUnit,null))
                {
                
                }
            }
        }
        
        /// <summary>
        /// The player clicked on the Select input button
        /// </summary>
        private async void SelectButtonPressed()
        {
            var cursorTile = tileSelector.cursorTile;
            Unit cursorUnit = null;
            if (!ReferenceEquals(cursorTile,null))
            {
                cursorUnit = cursorTile.currentUnit;
            }
            //Clicked on a unit
            if (!ReferenceEquals(cursorUnit,null))
            {
                if (playerUnits.All(x => x != cursorUnit)) return;
                //clicked on a player unit
                await unitSelector.ChangeSelectedUnitAsync(cursorUnit,gridManager.TileGrid);
                TileRenderingHelper.RenderUnitAvailablePaths(cursorUnit);
            }
            //Clicked on an empty tile
            else if (!ReferenceEquals(cursorTile,null))
            {
                unitMovementHandler.MoveUnitToTile(cursorTile,unitSelector.SelectedUnit);
            }
        }
        
        /// <summary>
        /// The player clicked on the End Turn input button
        /// </summary>
        private async void EndTurnButtonPressed()
        {
            await unitMovementHandler.MoveEnemyUnitsAsync(enemyUnits,gridManager.TileGrid);
            turnManager.EndTurn(AllUnits);
        }
        
        /// <summary>
        /// The player clicked on the Cancel input button
        /// </summary>
        private void CancelButtonPressed()
        {
            Debug.Log("Pressed Cancel!");
        }
        private void OnEnable()
        {
            PlayerInputActions.Enable();
        }

        private void OnDisable()
        {
            PlayerInputActions.Disable();
        }

    }
}
