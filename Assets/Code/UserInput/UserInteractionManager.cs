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
        private IEnumerable<Unit> _allUnits;
        #pragma warning disable 0649
        [SerializeField] private GridManager _gridManager;
        [SerializeField] private List<Unit> _playerUnits;
        [SerializeField] private List<Unit> _enemyUnits;
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
            _allUnits= _playerUnits.Concat(_enemyUnits);
            //currentUnit initialization in tiles
            foreach (var unit in _allUnits)
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
                    var selectedUnitPathfindingData = _unitSelector.SelectedUnit._pathfindingData;
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
                if (_playerUnits.All(x => x != cursorUnit)) return;
                //clicked on a player unit
                await _unitSelector.ChangeSelectedUnitAsync(cursorUnit,_gridManager.TileGrid);
                TileRenderingHelper.RenderUnitAvailablePaths(cursorUnit);
            }
            //Clicked on an empty tile
            else if (!ReferenceEquals(cursorTile,null))
            {
                _unitMovementHandler.MoveUnitToTile(cursorTile,_unitSelector.SelectedUnit);
            }
        }
        
        /// <summary>
        /// The player clicked on the End Turn input button
        /// </summary>
        private async void EndTurnButtonPressed()
        {
            await _unitMovementHandler.MoveEnemyUnitsAsync(_enemyUnits,_gridManager.TileGrid);
            _turnManager.EndTurn(_allUnits);
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
            _playerInputActions.Enable();
        }

        private void OnDisable()
        {
            _playerInputActions.Disable();
        }

    }
}
