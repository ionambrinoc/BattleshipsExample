// <auto-generated />
namespace Battleships.Core.Migrations
{
    using System.CodeDom.Compiler;
    using System.Data.Entity.Migrations.Infrastructure;
    using System.Resources;

    [GeneratedCode("EntityFramework.Migrations", "6.1.1-30610")]
    public sealed partial class RemoveFileNameFromPlayerRecord : IMigrationMetadata
    {
        private readonly ResourceManager Resources = new ResourceManager(typeof(RemoveFileNameFromPlayerRecord));
        
        string IMigrationMetadata.Id
        {
            get { return "201407210933271_RemoveFileNameFromPlayerRecord"; }
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