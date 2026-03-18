using Microsoft.VisualStudio.TestTools.UnitTesting;
using LibraryManagementSystem;
using System;

namespace LibraryManagementSystem.Tests;

[TestClass]
public class LibraryTests
{
    private Library _library = null!;

    [TestInitialize]
    public void Setup()
    {
        _library = new Library();
    }

    [TestMethod]
    public void AddBook_ShouldAddBookToLibrary()
    {
        var book = new Book("Atomic Habits", "James Clear", "101");

        _library.AddBook(book);

        Assert.HasCount(1, _library.Books, "Book count should be 1 after adding a book.");
    }

    [TestMethod]
    public void RegisterBorrower_ShouldAddBorrower()
    {
        var borrower = new Borrower("Diksha", "C1");

        _library.RegisterBorrower(borrower);

        Assert.HasCount(1, _library.Borrowers, "Borrower count should be 1 after registration.");
    }

    [TestMethod]
    public void BorrowBook_ShouldMarkBookAsBorrowed()
    {
        var book = new Book("Clean Code", "Robert Martin", "102");
        var borrower = new Borrower("Diksha", "C1");

        _library.AddBook(book);
        _library.RegisterBorrower(borrower);

        _library.BorrowBook("102", "C1");

        Assert.IsTrue(book.IsBorrowed);
        Assert.HasCount(1, borrower.BorrowedBooks, "Borrowed books count should be 1 after borrowing.");
    }
    [TestMethod]
    public void ReturnBook_ShouldMakeBookAvailable()
    {
        var book = new Book("Refactoring", "Martin Fowler", "103");
        var borrower = new Borrower("Diksha", "C1");
        _library.AddBook(book);
        _library.RegisterBorrower(borrower);
        _library.BorrowBook("103", "C1");
        _library.ReturnBook("103", "C1");
        Assert.IsFalse(book.IsBorrowed);
        Assert.IsEmpty(borrower.BorrowedBooks, "Borrowed books count should be 0 after returning.");
    }
    [TestMethod]
    public void ViewBooks_ShouldReturnAllBooks()
    {
        _library.AddBook(new Book("Book1", "Author1", "111"));
        var books = _library.ViewBooks();
        Assert.HasCount(1, books, "ViewBooks should return exactly one book.");
    }
    [TestMethod]
    public void BorrowBook_WithInvalidISBN_ShouldThrowException()
    {
        var borrower = new Borrower("Diksha", "C1");
        _library.RegisterBorrower(borrower);
        try
        {
            _library.BorrowBook("999", "C1");
            Assert.Fail("Expected InvalidOperationException was not thrown.");
        }
        catch (InvalidOperationException ex)
        {
            Assert.IsInstanceOfType(ex, typeof(InvalidOperationException));
        }
    }
    [TestMethod]
    public void ReturnBook_WithInvalidBorrower_ShouldThrowException()
    {
        var book = new Book("Test", "Author", "105");
        _library.AddBook(book);
        try
        {
            _library.ReturnBook("105", "INVALID");
            Assert.Fail("Expected InvalidOperationException was not thrown.");
        }
        catch (InvalidOperationException ex)
        {
            Assert.IsInstanceOfType(ex, typeof(InvalidOperationException));
        }
    }
}