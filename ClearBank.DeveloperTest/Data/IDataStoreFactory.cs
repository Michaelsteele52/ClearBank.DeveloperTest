namespace ClearBank.DeveloperTest.Data;

public interface IDataStoreFactory
{
    IAccountDataStore GetDataStore();
}