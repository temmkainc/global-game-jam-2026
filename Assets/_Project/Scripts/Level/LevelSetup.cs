using UnityEngine;

public class LevelSetup : MonoBehaviour
{
    [SerializeField] protected Door _door;
    [SerializeField] protected WordCubeReceiver _objective;

    protected virtual void Awake()
    {
        _door.Subscribe(_objective);
    }
}