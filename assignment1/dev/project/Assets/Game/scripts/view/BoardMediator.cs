using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;

public class BoardMediator : Mediator {

    [Inject]
    BoardView view { get; set; }

}
