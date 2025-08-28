using UnityEngine;

public class BoneMoverTest : MonoBehaviour
{
    public Transform bodyTip;    // Body_6
    public Transform neckTarget;
    [Header("Controle")]
    public bool esticarPescoço = false;  // Só move quando necessário
    public float suavizacao = 5f;        // Quanto mais alto, mais rápido segue o alvo
    void LateUpdate()
    {
        if (!esticarPescoço || bodyTip == null || neckTarget == null)
            return;

        // Move suavemente em direção ao alvo, sem quebrar animação abruptamente
        bodyTip.position = Vector3.Lerp(bodyTip.position, neckTarget.position, Time.deltaTime * suavizacao);
    }
}
