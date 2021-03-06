﻿using System.Collections.Generic;
using AutoMapper;
using Bogus;
using SuggeBook.Dto.Models;
using SuggeBook.Framework;

namespace SuggeBook.Dto.Mocks
{
	public class FakeAuthorService : IFakeAuthorService
	{
		public IFakeBooksService _booksService { get; set; }

		public FakeAuthorService(IFakeBooksService booksService)
		{
			_booksService = booksService;
		}


		public List<Author> Generate(int howMany)
		{
			var books = _booksService.Generate(7);
            var authorBooks = CustomAutoMapper.MapLists<Book, Author.Book>(books);

			var bookstest = new Faker<Author>().StrictMode(true)
					.RuleFor(a => a.FirstName, f => f.Name.FirstName())
					.RuleFor(a => a.LastName, f => f.Name.LastName())
					.RuleFor(a => a.Books, f => authorBooks);

			var authors = bookstest.Generate(howMany);

			return authors;
		}
	}
}
