﻿using System.Collections.Generic;
using Bogus.DataSets;
using Bogus;
using SuggeBook.Dto.Models;
using System;

namespace SuggeBook.Dto.Mocks
{
	public class FakeBooksService : IFakeBooksService
	{
		public FakeBooksService(IFakeSuggestionsService toto)
		{

		}


		public Randomizer Randomizer { get; set; }
		public List<Book> Generate(int howMany)
		{
			var Lorem = new Lorem(locale: "fr");
			var testBooks = new Faker<Book>().StrictMode(true).RuleFor(b => b.AuthorFullName, f => f.PickRandom(BooksSamples.BooksAuthors)).
					 RuleFor(b => b.Title, f => f.PickRandom(BooksSamples.BooksTitles))
					 .RuleFor(b => b.Categories, () => BooksSamples.GetCategories(3))
					 .RuleFor(b => b.AuthorFullName, f => f.Name.FullName())
					 .RuleFor(b => b.ISBN, f => Guid.NewGuid())
					 .RuleFor(b => b.NumberOfSuggestions, f => f.Random.Number(1, 100))
			.RuleFor(b => b.Year, f => f.Date.Past().Year);


			var books = testBooks.Generate(howMany);

			return books;
		}
	}
}
