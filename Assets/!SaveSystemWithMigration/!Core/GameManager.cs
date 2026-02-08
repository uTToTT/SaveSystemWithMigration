using TToTT.SaveSystem;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int _currentSaveVersion;
    [SerializeField] private Logger _mainLogger;
    [SerializeField] private Logger _statsLogger;

    private IPersistentData _runtimePersistentData;
    private IDataProvider _dataProvider;

    public static GameManager Instance { get; private set; }

    public Logger MainLogger => _mainLogger;
    public Logger StatsLogger => _statsLogger;
    public SaveService SaveService { get; private set; }

    private void Awake()
    {
        Instance = this;

        InitDependecies();
        InitSaveSystem();
    }

    private void InitDependecies()
    {
        _mainLogger.Init();
    }

    private void InitSaveSystem()
    {
        _runtimePersistentData = new PersistentData();
        _dataProvider = new DataLocalProvider(MainLogger, _runtimePersistentData);

        SaveService = new SaveService(
           _currentSaveVersion,
           MainLogger,
           _runtimePersistentData,
           _dataProvider);
    }
}
