using UnityEngine;
using System.Collections;
using strange.extensions.signal.impl;

public class GameStartSignal : Signal { }

public class GameEndSignal : Signal { }

public class StartTurnSignal : Signal<DiscColour> { }
public class PlayTurnSignal : Signal<GridPosition, DiscColour> { }
public class SkipTurnSignal : Signal { }

public class MakeUserInputMoveSignal : Signal<DiscColour> { }

public class MakeAIMoveSignal : Signal<DiscColour> { }
