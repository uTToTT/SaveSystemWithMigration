using TToTT.GameEvents;
using TToTT.SaveSystem;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int _currentSaveVersion;
    [SerializeField] private Logger _mainLogger;
    [SerializeField] private EventManager _eventManager;
    [SerializeField] private DataDisplay _dataDisplay;

    private IPersistentData _runtimePersistentData;
    private IDataProvider _dataProvider;

    public static GameManager Instance { get; private set; }

    public Logger MainLogger => _mainLogger;
    public SaveService SaveService { get; private set; }

    private void Awake()
    {
        Instance = this;

        InitDependecies();
        InitSaveSystem();

        SaveService.Load();
    }

    private void InitDependecies()
    {
        _mainLogger.Init();
        _dataDisplay.Init();
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

        SaveService.OnSaved += _eventManager.GameDataSaved;
        SaveService.OnDeleted += _eventManager.GameDataDeleted;
        SaveService.OnLoaded += _eventManager.GameDataLoaded;
    }
}
