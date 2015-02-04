using UnityEngine;
using System.Collections;

public interface IUpdateManager 
{
    void Initialize();
    void Shutdown();

    void StartCycle();
    void EndCycle();
    void Reset();
}
