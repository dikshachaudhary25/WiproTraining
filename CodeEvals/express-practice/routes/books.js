const express = require('express');
const router = express.Router();

let books = [
    { id: 1, title: "1984", author: "Orwell" },
    { id: 2, title: "The Alchemist", author: "Coelho" }
];

// GET all books
router.get('/', (req, res) => {
    res.json(books);
});

// POST add book
router.post('/', (req, res) => {
    const { title, author } = req.body;

    if (!title || !author) {
        return res.status(400).json({ message: "Title and Author required" });
    }

    const newBook = {
        id: Date.now(),
        title,
        author
    };

    books.push(newBook);
    res.status(201).json(newBook);
});

// PUT update book
router.put('/:id', (req, res) => {
    const id = parseInt(req.params.id);
    const book = books.find(b => b.id === id);

    if (!book) {
        return res.status(404).json({ message: "Book not found" });
    }

    Object.assign(book, req.body);
    res.json(book);
});

// DELETE book
router.delete('/:id', (req, res) => {
    const id = parseInt(req.params.id);
    const index = books.findIndex(b => b.id === id);

    if (index === -1) {
        return res.status(404).json({ message: "Book not found" });
    }

    books.splice(index, 1);
    res.json({ message: "Book deleted successfully" });
});

module.exports = router;