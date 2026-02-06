using TToTT.SaveSystem;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int _currentSaveVersion;
    [SerializeField] private Logger _logger;


    public static GameManager Instance { get; private set; }

    public Logger Logger => _logger;
    public SaveService SaveService { get; private set; }

    private void Awake()
    {
        Instance = this;

        CreateDependecies();
        InitDependecies();
    }

    private void CreateDependecies()
    {
        SaveService = new SaveService(_currentSaveVersion);
    }

    private void InitDependecies()
    {
        _logger.Init();
    }
}
