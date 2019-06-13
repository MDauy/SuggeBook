﻿using System.Linq;
using System.Threading.Tasks;
using SuggeBook.Domain.Model;
using SuggeBook.Domain.Repositories;
using SuggeBook.Infrastructure.Documents;
using SuggeBook.Infrastructure.Exceptions;
using SuggeBook.Framework;
using MongoDB.Bson;
using System.Linq.Expressions;
using System;

namespace SuggeBook.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        public UserRepository(IBaseRepository<UserDocument> baseRepo)
        {
            _baseRepository = baseRepo;
        }

        private readonly IBaseRepository<UserDocument> _baseRepository;

        public async Task<User> Create(User user)
        {
            var userDocument = await _baseRepository.Insert(new UserDocument(user));
            return userDocument.ToModel();
        }

        private async Task<User> BasicGetUser(Expression<Func<UserDocument, bool>> func, string userIdentifier)
        {
            var users = await _baseRepository.Get(func);
            if (users.Count > 1)
            {
                throw new ObjectNotUniqueException("User", userIdentifier);
            }

            return users.First().ToModel();
        }

        public async Task<User> GetFromUsername(string username)
        {
            return await BasicGetUser(x => x.UserName == username, username);
        }

        public async Task<User> Get(string userId)
        {
            return await BasicGetUser(x => x.Id == ObjectId.Parse(userId), userId);
        }

        public async Task<User> GetSimilarUsername(string username)
        {
            var existingUser =
                await _baseRepository.Get(u => string.Equals(u.UserName, username));

            if (existingUser.IsNullOrEmpty())
            {
                return null;
            }

            if (existingUser.Count > 1)
            {
                throw new ObjectNotUniqueException("User", $"{username}");
            }

            return existingUser.First().ToModel();
        }

        public async Task<User> GetSimilarMail(string mail)
        {
            var existingUser =
                await _baseRepository.Get(u => string.Equals(u.Email, mail));

            if (existingUser.IsNullOrEmpty())
            {
                return null;
            }

            if (existingUser.Count > 1)
            {
                throw new ObjectNotUniqueException("User", $"{mail}");
            }

            return existingUser.First().ToModel();
        }

        public async Task<User> Connect(string usernameOrEmail, string password)
        {
            var users = await _baseRepository.Get(u => u.UserName == usernameOrEmail);
            if (users.IsNullOrEmpty())
            {
                users = await _baseRepository.Get(u => u.Email == usernameOrEmail);
            }

            if (!users.IsNullOrEmpty())
            {
                if (users.Count > 1)
                {
                    throw new ObjectNotUniqueException("User", usernameOrEmail);
                }

                var user = users.First();

                if (string.Equals(user.Password, password))
                {

                    return user.ToModel();
                }
            }

            return null;
        }
    }
}