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
        private Controls _playerInputActions;
        #pragma warning disable 0649
        [SerializeField] private GridManager _gridManager;
        [SerializeField] private List<Unit> _unitList;
        [SerializeField] private UnitProfileGui _unitProfile;
        [SerializeField] private TileSelector _tileSelector;
        [SerializeField] private UnitSelector _unitSelector;
        [SerializeField] private UnitMovementHandler _unitMovementHandler;
        [SerializeField] private TurnManager _turnManager;
        #pragma warning restore 0649

        private void Awake()
        {
            _playerInputActions=new Controls();
            _playerInputActions.Gameplay.Select.started  += ctx => SelectButtonPressed();
            _playerInputActions.Gameplay.EndTurn.started  += ctx => EndTurnButtonPressed();
            _playerInputActions.Gameplay.Cancel.started += ctx => CancelButtonPressed(); 
        }
        private void Start()
        {
            //currentUnit initialization in tiles
            foreach (var unit in _unitList)
            {
                var position = unit.transform.position;
                _gridManager.TileGrid[(int)position.x, (int)position.y].CurrentUnit=unit;
            }
        }
        private void Update()
        {
            var cursorTile = _tileSelector.CursorTile;
            Unit cursorUnit = null;
            if (!ReferenceEquals(cursorTile,null))
            {
                cursorUnit = cursorTile.CurrentUnit;
            }

            if (_tileSelector.CursorTileChanged)
            {
                //if the cursor is hovering over a unit, show the profile
                _unitProfile.ShowUnitProfile(cursorUnit);
                //cursor hovered over a tile within the selected units available moves
                if (!ReferenceEquals(_unitSelector.SelectedUnit, null))
                {
                    var selectedUnitPathfindingData = _unitSelector.SelectedUnit.PathfindingData;
                    //If hovering within the selected unit's movement, renders the path, otherwise removes the rendered path  
                    var hoveredTilePathData = selectedUnitPathfindingData.FirstOrDefault(x => x.DestinationGridTile == _tileSelector.CursorTile);
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
            var cursorTile = _tileSelector.CursorTile;
            Unit cursorUnit = null;
            if (!ReferenceEquals(cursorTile,null))
            {
                cursorUnit = cursorTile.CurrentUnit;
            }
            //Clicked on a unit
            if (!ReferenceEquals(cursorUnit,null))
            {
                if (cursorUnit.Faction != UnitFaction.Player) return;
                //clicked on a player unit
                await _unitSelector.ChangeSelectedUnitAsync(cursorUnit,_gridManager.TileGrid);
                TileRenderingHelper.RenderUnitAvailablePaths(cursorUnit);
            }
            //Clicked on an empty tile
            else if (!ReferenceEquals(cursorTile,null))
            {
                var selectedUnit = _unitSelector.SelectedUnit;
                _unitMovementHandler.MoveUnitToTile(cursorTile,selectedUnit);
                //to be moved to other class
                selectedUnit.CombatController.AttackableTiles = _gridManager.TileGrid.GetGridTilesOfRange(cursorTile,selectedUnit.CombatController.AttackRanges);
                TileRenderingHelper.RenderUnitAttackTiles(selectedUnit);
            }
        }
        
        /// <summary>
        /// The player clicked on the End Turn input button
        /// </summary>
        private async void EndTurnButtonPressed()
        {
            await _unitMovementHandler.MoveEnemyUnitsAsync(_unitList.Where(x => x.Faction == UnitFaction.Monster),
                _gridManager.TileGrid);
            _turnManager.EndTurn(_unitList);
        }
        
        /// <summary>
        /// The player clicked on the Cancel input button
        /// </summary>
        private void CancelButtonPressed()
        {
            _unitSelector.DeselectUnit();
        }
        private void OnEnable()
        {
            _playerInputActions.Enable();
        }

        private void OnDisable()
        {
            _playerInputActions.Disable();
        }

    }
}
