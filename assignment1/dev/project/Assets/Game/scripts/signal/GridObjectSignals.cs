using UnityEngine;
using System.Collections;
using strange.extensions.signal.impl;

public class GridObjectMovedSignal : Signal<IGridObject, GridPosition> { }

public class MoveSnakeSignal : Signal<uint> { }

public class UpdateSnakeDirectionSignal : Signal<uint> { }

public class IncrementSnakeTailSignal : Signal<uint> { }

public class EatFoodSignal : Signal<GridPosition, uint> { }

public class RemoveGridObjectSignal : Signal<IGridObject> { }

public class AddWallSignal : Signal<GridPosition> { }

public class CreateSnakeSignal : Signal { }