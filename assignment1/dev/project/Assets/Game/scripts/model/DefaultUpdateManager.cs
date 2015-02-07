using UnityEngine;
using System.Collections;
using System.Timers;

public class DefaultUpdateManager : IUpdateManager
{
    [Inject]
    public MoveAllSnakesSignal moveSnakeSignal { get; set; }

    [Inject]
    public UpdateAllSnakesSignal updateSnakesSignal { get; set; }

    private const uint MILLISECONDS_BETWEEN_MOVE = 250;
    private Timer moveTimer;

    public void Initialize()
    {
        Debug.Log("DefaultUpdateManager::Initialize");
        moveTimer = new Timer(MILLISECONDS_BETWEEN_MOVE);
        moveTimer.Elapsed += moveTimerElapsed;
    }

    public void Shutdown()
    {
        Debug.Log("DefaultUpdateManager::Shutdown");
        moveTimer.Stop();
        moveTimer.Dispose();
    }

    public void StartCycle()
    {
        moveTimer.Start();
    }

    public void EndCycle()
    {
        moveTimer.Stop();
    }

    public void Reset()
    {
        EndCycle();
        StartCycle();
    }

    void moveTimerElapsed(object sender, ElapsedEventArgs args)
    {
        //Debug.Log("Move Timer Elapsed");
        moveSnakeSignal.Dispatch();
        updateSnakesSignal.Dispatch();
        StartCycle();
    }
}
