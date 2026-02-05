public sealed class SaveService
{
    private IPersistentData _persistentData;
    private IDataProvider _provider;



    public SaveService(IDataProvider dataProvider)
    {
        _provider = dataProvider;
    }
}
