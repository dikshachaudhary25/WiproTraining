const fs = require('fs').promises;
const path = require('path');
const EventEmitter = require('events');

const filePath = path.join(__dirname, '../data/books.json');

const eventEmitter = new EventEmitter();

// Event Listeners
eventEmitter.on('bookAdded', () => console.log('📘 Book Added'));
eventEmitter.on('bookUpdated', () => console.log('✏️ Book Updated'));
eventEmitter.on('bookDeleted', () => console.log('🗑️ Book Deleted'));

// Read Books
const readBooks = async () => {
    const data = await fs.readFile(filePath, 'utf-8');
    return JSON.parse(data);
};

// Write Books
const writeBooks = async (books) => {
    await fs.writeFile(filePath, JSON.stringify(books, null, 2));
};

// Get All Books
const getAllBooks = async () => {
    return await readBooks();
};

// Get Book by ID
const getBookById = async (id) => {
    const books = await readBooks();
    return books.find(b => b.id === id);
};

// Add Book
const addBook = async (book) => {
    const books = await readBooks();
    books.push(book);
    await writeBooks(books);
    eventEmitter.emit('bookAdded');
};

// Update Book
const updateBook = async (id, updatedData) => {
    const books = await readBooks();
    const index = books.findIndex(b => b.id === id);

    if (index === -1) return null;

    books[index] = { ...books[index], ...updatedData };
    await writeBooks(books);
    eventEmitter.emit('bookUpdated');

    return books[index];
};

// Delete Book
const deleteBook = async (id) => {
    const books = await readBooks();
    const newBooks = books.filter(b => b.id !== id);

    if (books.length === newBooks.length) return false;

    await writeBooks(newBooks);
    eventEmitter.emit('bookDeleted');

    return true;
};

module.exports = {
    getAllBooks,
    getBookById,
    addBook,
    updateBook,
    deleteBook
};