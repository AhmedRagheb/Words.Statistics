
using Microsoft.EntityFrameworkCore;

namespace Words.DataAccess;
internal static class ModelBuilderExtensions
{
	public static void Seed(this ModelBuilder modelBuilder)
	{
		var words = new List<Word>
			{
				new Word
				{
					Id = 1,
					Term = "my",
					Count = 3
				},
				new Word
				{
					Id = 2,
					Term = "what",
					Count = 1
				},
			};

		modelBuilder.Entity<Word>().HasData(words);
	}
}