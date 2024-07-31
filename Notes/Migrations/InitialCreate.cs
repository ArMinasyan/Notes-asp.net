using System.Data.Entity.Migrations;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Notes.Migrations;

public class InitialCreate : DbMigration
{
    public override void Up()
    {
        CreateTable("users", table => new
        {
            Id = table.Int(nullable: false),
            Username = table.String(nullable: false),
            Password = table.String(nullable: false)

        }).PrimaryKey(t => t.Id);
    }
    
    public override void Down()
    {
       
    }
    
}