using UnityEngine;

public class GrassFabric : MonoBehaviour
{
    [SerializeField] private Grass _grassTemplate;
    private GrassStart[] _grassStartPoints;

    private void Awake()
    {
        _grassStartPoints = GetComponentsInChildren<GrassStart>();
    }
    private void Start()
    {
        foreach (GrassStart _grassPoint in _grassStartPoints)
        {
            Instantiate(_grassTemplate, _grassPoint.transform.position, Quaternion.identity);
        }
    }
}
