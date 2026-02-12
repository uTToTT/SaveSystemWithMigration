using TToTT.SaveSystem;
using UnityEngine;
using UnityEngine.UI;

public class SaveDebugger : MonoBehaviour
{
    [SerializeField] private Button _saveButon;
    [SerializeField] private Button _loadButon;
    [SerializeField] private Button _deleteButon;
    [SerializeField] private Button _corruptButon;
    [SerializeField] private Button _spamButon;
    [SerializeField] private Button _folderButon;

    public void Init()
    {
        _saveButon.Init(SaveClick);
        _loadButon.Init(LoadClick);
        _deleteButon.Init(DeleteClick);
        _corruptButon.Init(CorruptClick);
        _spamButon.Init(SpamClick);
        _folderButon.Init(FolderClick);
    }

    private void SaveClick() => GameManager.Instance.SaveService.Save();
    private void LoadClick() => GameManager.Instance.SaveService.Load();
    private void DeleteClick() => GameManager.Instance.SaveService.Delete();

    private void CorruptClick() { }
    private void SpamClick() { }
    private void FolderClick() => Application.OpenURL("file://" + DataLocalProvider.SavePath);
}
