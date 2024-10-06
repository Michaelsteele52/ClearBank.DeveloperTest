using System.Configuration;

namespace ClearBank.DeveloperTest.Data;

public class DataStoreFactory : IDataStoreFactory
{
    private const string DataStoreType = "DataStoreType";
    private const string Backup = "Backup";
    public IAccountDataStore GetDataStore()
    {
        
        var dataStoreType = ConfigurationManager.AppSettings[DataStoreType];
        if(string.Equals(dataStoreType, Backup)) return new BackupAccountDataStore();
        return new AccountDataStore();
    }
}