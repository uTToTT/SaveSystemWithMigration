using TToTT.GameEvents;
using TToTT.SaveSystem;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int _currentSaveVersion;
    [SerializeField] private Logger _mainLogger;
    [SerializeField] private EventManager _eventManager;
    [SerializeField] private DataDisplay _dataDisplay;
    [SerializeField] private GodConsole _godConsole;

    private IPersistentData _runtimePersistentData;
    private IDataProvider _dataProvider;

    public static GameManager Instance { get; private set; }

    public Logger MainLogger => _mainLogger;
    public EventManager EventManager => _eventManager;
    public SaveService SaveService { get; private set; }

    private void Awake()
    {
        Instance = this;

        _mainLogger.Init();
        MainLogger.Log("Start system.");

        _dataDisplay.Init();
     
        InitSaveSystem();
        _godConsole.Init();
    }

    private void InitSaveSystem()
    {
        MainLogger.Log("Initialize save service...");

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
        MainLogger.Log("Save service initialized.");

        SaveService.Load();
        SaveService.Save();
    }
}
