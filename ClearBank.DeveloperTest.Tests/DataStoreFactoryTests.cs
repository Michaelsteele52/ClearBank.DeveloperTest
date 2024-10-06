using System.Configuration;
using ClearBank.DeveloperTest.Data;
using FluentAssertions;
using Xunit;

namespace ClearBank.DeveloperTest.Tests;

public class DataStoreFactoryTests
{
    private readonly DataStoreFactory _sut = new();
    private const string DataStoreType = "DataStoreType";
    private const string Backup = "Backup";

    [Fact]
    public void GivenTheFactoryIsUsed_WhenTheAppSettingIsBackup_ThenTheBackupImplementationIsCreated()
    {
        ConfigurationManager.AppSettings[DataStoreType] = Backup;
        var result = _sut.GetDataStore();
        result.GetType().Should().Be(typeof(BackupAccountDataStore));
    }
    
    [Fact]
    public void GivenTheFactoryIsUsed_WhenTheAppSettingIsNotBackup_ThenTheAccountDataStoreImplementationIsCreated()
    {
        ConfigurationManager.AppSettings[DataStoreType] = string.Empty;
        var result = _sut.GetDataStore();
        result.GetType().Should().Be(typeof(AccountDataStore));
    }
}