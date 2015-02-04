using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;

public interface IGridObjectMediator
{
    Signal<GridPosition> ModelMovedSignal { get; }
}
