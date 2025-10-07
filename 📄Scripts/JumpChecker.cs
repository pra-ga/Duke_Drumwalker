using UnityEngine;

public class JumpChecker : MonoBehaviour
{
    string jumpCheckerTag = "";

    void OnTriggerEnter(Collider other)
    {
        jumpCheckerTag = other.tag;
        
    }

    public bool CanPlayerJump()
    {
        if (jumpCheckerTag == "Tile" || jumpCheckerTag == "Forks" || jumpCheckerTag == "ExitDoor")
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
