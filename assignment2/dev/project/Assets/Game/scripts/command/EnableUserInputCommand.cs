using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;
using System.Collections.Generic;

public class EnableUserInputCommand : Command
{
    [Inject]
    public IResourceNameManager resourceNameManager { get; set; }

    public override void Execute()
    {
        GameObject boardGameObject = GameObject.Find(resourceNameManager.GetResourceName(ResourceID.BoardGO));

        IList<BoxCollider2D> boxColliders = boardGameObject.GetComponentsInChildren<BoxCollider2D>();

        foreach (BoxCollider2D box in boxColliders)
        {
            box.enabled = true;
        }
    }
}
