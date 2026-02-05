using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Logger _logger;

    public Logger Logger => _logger;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;

        _logger.Clear();
    }
}
