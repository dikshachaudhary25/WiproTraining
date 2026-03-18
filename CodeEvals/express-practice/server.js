const express = require('express');
const app = express();

app.use(express.json());
app.use((req, res, next) => {
    const time = new Date().toISOString();
    console.log(`[${req.method}] ${req.url} - ${time}`);
    next();
});

app.get('/', (req, res) => {
    res.send("Welcome to Express Server");
});

app.get('/status', (req, res) => {
    res.json({ server: "running", uptime: "OK" });
});

app.get('/products', (req, res) => {
    const name = req.query.name;

    if (name) {
        res.json({ query: name });
    } else {
        res.send("Please provide a product name");
    }
});

const bookRouter = require('./routes/books');
app.use('/books', bookRouter);

const courseRouter = require('./routes/courses');
app.use('/courses', courseRouter);


app.use((req, res) => {
    res.status(404).json({ message: "Route not found" });
});


app.use((err, req, res, next) => {
    console.error(err.stack);
    res.status(500).json({ error: "Internal Server Error" });
});


app.listen(4000, () => {
    console.log("🚀 Server running on http://localhost:4000");
});