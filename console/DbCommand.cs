﻿using System;
using System.Data.SqlClient;
using ManyConsole;
using model;
using NDesk.Options;

namespace console
{
	public abstract class DbCommand : ConsoleCommand
	{

		protected string Server { get; set; }
		protected string DbName { get; set; }
		protected string User { get; set; }
		protected string Pass { get; set; }
		protected string ScriptDir { get; set; }
		protected bool Overwrite { get; set; }

		protected DbCommand(string command, string oneLineDescription)
		{
			this.IsCommand(command, oneLineDescription);
			this.Options = new OptionSet();
			this.SkipsCommandSummaryBeforeRunning();
			this.HasRequiredOption("s|server=", "server", o => this.Server = o);
			this.HasRequiredOption("b|database=", "database", o => this.DbName = o);
			this.HasOption("u|user=", "user", o => this.User = o);
			this.HasOption("p|pass=", "pass", o => this.Pass = o);
			this.HasRequiredOption(
				"d|scriptDir=",
				"Path to database script directory.",
				o => this.ScriptDir = o);
			this.HasOption(
				"o|overwrite=",
				"Overwrite existing target without prompt.",
				o => this.Overwrite = o != null);
		}

		protected Database CreateDatabase()
		{
			var builder = new SqlConnectionStringBuilder()
			{
				DataSource = this.Server,
				InitialCatalog = this.DbName,
				IntegratedSecurity = string.IsNullOrEmpty(this.User)
			};
			if (!builder.IntegratedSecurity)
			{
				builder.UserID = this.User;
				builder.Password = this.Pass;
			}
			return new Database()
			{
				Connection = builder.ToString(),
				Dir = this.ScriptDir
			};

		}

	}
}
