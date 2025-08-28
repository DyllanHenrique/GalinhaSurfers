using UnityEngine;

public class BoneMoverTest : MonoBehaviour
{
    public Transform bodyTip;    // Body_6
    public Transform neckTarget;
    [Header("Controle")]
    public bool esticarPesco�o = false;  // S� move quando necess�rio
    public float suavizacao = 5f;        // Quanto mais alto, mais r�pido segue o alvo
    void LateUpdate()
    {
        if (!esticarPesco�o || bodyTip == null || neckTarget == null)
            return;

        // Move suavemente em dire��o ao alvo, sem quebrar anima��o abruptamente
        bodyTip.position = Vector3.Lerp(bodyTip.position, neckTarget.position, Time.deltaTime * suavizacao);
    }
}
