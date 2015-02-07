using UnityEngine;
using System.Collections;
using NUnit.Framework;

[TestFixture]
public class GridPositionTest {

    [Test]
    public void EqualPositionsTest()
    {
        GridPosition a = new GridPosition(3, 3);
        GridPosition b = new GridPosition(3, 3);

        bool aEqualsB = a.Equals(b);

        Assert.True(aEqualsB);
    }

    [Test]
    public void NotEqualPositionsTest()
    {
        GridPosition a = new GridPosition(3, 3);
        GridPosition b = new GridPosition(4, 3);

        bool aEqualsB = a.Equals(b);
        Assert.False(aEqualsB);

        b = new GridPosition(3, 4);
        
        aEqualsB = a.Equals(b);
        Assert.False(aEqualsB);
    }

    [Test]
    public void PositionInDirectionTest()
    {
        GridPosition center = new GridPosition(10, 10);
        GridPosition left = new GridPosition(9, 10);
        GridPosition right = new GridPosition(11, 10);
        GridPosition up = new GridPosition(10, 11);
        GridPosition down = new GridPosition(10, 9);

        Assert.AreEqual(center.GetPositionInDirection(GridDirection.Left), left, "Left Direction Not Working");
        Assert.AreEqual(center.GetPositionInDirection(GridDirection.Right), right, "Right Direction Not Working");
        Assert.AreEqual(center.GetPositionInDirection(GridDirection.Up), up, "Up Direction Not Working");
        Assert.AreEqual(center.GetPositionInDirection(GridDirection.Down), down, "Down Direction Not Working");
    }

    [Test]
    public void DirectionOfPositionTest()
    {
        GridPosition center = new GridPosition(10, 10);
        GridPosition left = new GridPosition(9, 10);
        GridPosition right = new GridPosition(11, 10);
        GridPosition up = new GridPosition(10, 11);
        GridPosition down = new GridPosition(10, 9);

        GridPosition nonAdjacent = new GridPosition(4, 3);

        Assert.AreEqual(center.GetDirectionOfPosition(left), GridDirection.Left, "Left Direction Not Working");
        Assert.AreEqual(center.GetDirectionOfPosition(right), GridDirection.Right, "Left Direction Not Working");
        Assert.AreEqual(center.GetDirectionOfPosition(up), GridDirection.Up, "Left Direction Not Working");
        Assert.AreEqual(center.GetDirectionOfPosition(down), GridDirection.Down, "Left Direction Not Working");
        
        Assert.AreEqual(center.GetDirectionOfPosition(down), GridDirection.Invalid, "NonAdjacent Not Working");
    }

}
