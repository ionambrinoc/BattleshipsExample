// <auto-generated />
namespace Battleships.Runner.Migrations
{
    using System.CodeDom.Compiler;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Migrations.Infrastructure;
    using System.Resources;
    
    [GeneratedCode("EntityFramework.Migrations", "6.1.1-30610")]
    public sealed partial class AddedForeignKeyToUsersTableInPlayerRecords : IMigrationMetadata
    {
        private readonly ResourceManager Resources = new ResourceManager(typeof(AddedForeignKeyToUsersTableInPlayerRecords));
        
        string IMigrationMetadata.Id
        {
            get { return "201407221259100_AddedForeignKeyToUsersTableInPlayerRecords"; }
        }
        
        string IMigrationMetadata.Source
        {
            get { return null; }
        }
        
        string IMigrationMetadata.Target
        {
            get { return Resources.GetString("Target"); }
        }
    }
}
