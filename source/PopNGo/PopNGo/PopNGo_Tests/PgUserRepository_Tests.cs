using Moq;
using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using PopNGo.DAL.Concrete;
using PopNGo.DAL.Abstract;
using PopNGo.Models;
using System.Collections.Generic;

namespace PopNGo_Tests;

public class PgUserRepositoryTests
{
    private static readonly string _seedFile = @"../../../Sql/SEED.sql";  // relative path from where the executable is: bin/Debug/net7.0
    private static readonly InMemoryDbHelper<PopNGoDB> _dbHelper = new(_seedFile, DbPersistence.OneDbPerTest);
    private static PgUserRepository _pgUserRepository = null!;
    private static PopNGoDB _context = null!;

    [SetUp]
    public void Setup()
    {
        _context = _dbHelper.GetContext();
        _pgUserRepository = new PgUserRepository(_context);
    }

    [Test]
    public void SetRecommendationsPreviouslyAtDate_ShouldSetDate()
    {
        // Arrange
        var userId = 50;
        var pgUser = new PgUser { Id = userId, AspnetuserId = "1" };
        _context.PgUsers.Add(pgUser);
        _context.SaveChanges();

        var newDate = DateTime.Now;

        // Act
        _pgUserRepository.SetRecommendationsPreviouslyAtDate(userId, newDate);

        // Assert
        Assert.That(_context.PgUsers.Find(userId).RecommendedPreviouslyAt, Is.EqualTo(newDate));
    }
}
