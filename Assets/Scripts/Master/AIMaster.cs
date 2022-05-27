using UnityEngine;

public class AIMaster : Master
{
    protected override void Init()
    {
    }

    public override void BeginTurn()
    {
        Invoke("EndTurn", 0.5f);
    }

    private void EndTurn()
    {
        GameManager.current.NextTurn();
    }
}