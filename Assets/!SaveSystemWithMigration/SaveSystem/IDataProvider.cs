public interface IDataProvider 
{
    void Save();
    void Delete();
    bool TryLoad();
}
