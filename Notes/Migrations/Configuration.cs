using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

using Notes.Models;

namespace Notes.Migrations;

internal sealed class Configuration : DbMigrationsConfiguration
{
    public Configuration()
    {
        AutomaticMigrationsEnabled = true;
    }
}