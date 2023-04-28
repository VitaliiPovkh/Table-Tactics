public enum Layers
{
    DEFAULT = 1,
    MAP_OBJECTS = 1 << 3,
    UI = 1 << 5,
    MAP_BORDERS = 1 << 6, //for selfwritten A*
    UNITS = 1 << 6,
    ENEMY = 1 << 7
}
