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
    [SerializeField] private SaveDebugger _saveDebugger;
    [SerializeField] private SceneLoader _sceneLoader;

    private IPersistentData _runtimePersistentData;
    private IDataProvider _dataProvider;

    public static GameManager Instance { get; private set; }

    public Logger MainLogger => _mainLogger;
    public EventManager EventManager => _eventManager;
    public SaveService SaveService { get; private set; }

    private void Awake()
    {
        // TODO: export init system to Bootstrap.cs

        Instance = this;

        _mainLogger.Init();
        MainLogger.Log("Start system.");

        InitSaveSystem();

        _dataDisplay.Init();
        _godConsole.Init();
        _saveDebugger.Init();
        _sceneLoader.Init();
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
    }
}
