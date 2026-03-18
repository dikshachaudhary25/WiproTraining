const express = require('express');
const {
    getAllBooks,
    getBookById,
    addBook,
    updateBook,
    deleteBook
} = require('./services/bookService');

const app = express();
app.use(express.json());

// Root Route
app.get('/', (req, res) => {
    res.json({ message: "Welcome to Book Management API" });
});

// GET all books
app.get('/books', async (req, res) => {
    const books = await getAllBooks();
    res.json(books);
});

// GET book by ID
app.get('/books/:id', async (req, res) => {
    const book = await getBookById(parseInt(req.params.id));

    if (!book)
        return res.status(404).json({ message: "Book not found" });

    res.json(book);
});

// POST add book
app.post('/books', async (req, res) => {
    const { title, author } = req.body;

    if (!title || !author)
        return res.status(400).json({ message: "Title and Author required" });

    const newBook = {
        id: Date.now(),
        title,
        author
    };

    await addBook(newBook);
    res.status(201).json(newBook);
});

// PUT update book
app.put('/books/:id', async (req, res) => {
    const updated = await updateBook(parseInt(req.params.id), req.body);

    if (!updated)
        return res.status(404).json({ message: "Book not found" });

    res.json(updated);
});

// DELETE book
app.delete('/books/:id', async (req, res) => {
    const deleted = await deleteBook(parseInt(req.params.id));

    if (!deleted)
        return res.status(404).json({ message: "Book not found" });

    res.json({ message: "Book deleted successfully" });
});

// Start Server
app.listen(3000, () => {
    console.log("🚀 Server running on http://localhost:3000");
});