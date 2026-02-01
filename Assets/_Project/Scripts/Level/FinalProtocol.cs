using UnityEngine;

public class FinalProtocol : MonoBehaviour
{
    [SerializeField] NarrationSequence sequence;
    [SerializeField] NarrationManager narrationManager;
    [SerializeField] GameObject narrationEndCutScene;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Final Protocol Triggered");
            narrationManager.PlaySequence(sequence);
            narrationEndCutScene.SetActive(true);
        }

    }
}
