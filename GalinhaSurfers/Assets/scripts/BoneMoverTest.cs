using UnityEngine;

public class BoneMoverTest : MonoBehaviour
{
    public Transform bodyTip;    // Body_6
    public Transform neckTarget;

    void LateUpdate()
    {
        if (bodyTip != null && neckTarget != null)
        {
            bodyTip.position = neckTarget.position;
        }
    }
}
