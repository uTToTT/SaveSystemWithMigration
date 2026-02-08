using TToTT.SaveSystem;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int _currentSaveVersion;
    [SerializeField] private Logger _logger;

    private IPersistentData _runtimePersistentData;
    private IDataProvider _dataProvider;

    public static GameManager Instance { get; private set; }

    public Logger Logger => _logger;
    public SaveService SaveService { get; private set; }

    private void Awake()
    {
        Instance = this;

        InitSaveSystem();
        InitDependecies();
    }

    private void InitDependecies()
    {
        _logger.Init();
    }

    private void InitSaveSystem()
    {
        _runtimePersistentData = new PersistentData();
        _dataProvider = new DataLocalProvider(_runtimePersistentData);

        SaveService = new SaveService(
           _currentSaveVersion,
           Logger,
           _runtimePersistentData,
           _dataProvider);
    }
}
