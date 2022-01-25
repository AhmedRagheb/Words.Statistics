using System;
using Words.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Words.Services.Tests;
public abstract class InMemoryDbContext
{
	protected WordsDbContext DbContext { get; set; }

	protected InMemoryDbContext()
	{
		var options = new DbContextOptionsBuilder<WordsDbContext>()
			.UseInMemoryDatabase(Guid.NewGuid().ToString())
			.EnableSensitiveDataLogging()
			.Options;

		DbContext = new WordsDbContext(options);
	}
}