using UnityEngine;
using System.Collections;
using strange.extensions.signal.impl;

public class CreateBoardSquareViewSignal : Signal<GridPosition> {}

public class CreateDiscSignal : Signal<GridPosition, DiscColour> { }

public class BoardSquarePressedSignal : Signal<GridPosition> { }

public class BoardSquareSelectedSignal : Signal<GridPosition> { }

