namespace Code
{
    public enum TerrainType
    {
        Normal = 1,
        Difficult = 2,
        Impassable = 3
    }

    public enum UnitState
    {
        Idle=0,
        Selected=1,
        Moving=2,
        OutOfActions=3
    }

    public enum TileState
    {
        Idle = 0,
        Selected = 1,
        Active = 2,
        Attackable = 3
    }

    public enum UnitFaction
    {
        Player = 0,
        Monster = 1,
        NPC = 2
    }
}
