import React, { useState, useEffect } from 'react';
import './FilmsTable.css';

const FilmsTable = () => {
    const [films, setFilms] = useState([]);
    const [filteredFilms, setFilteredFilms] = useState([]);
    const [searchQuery, setSearchQuery] = useState('');
    const [sortOption, setSortOption] = useState('title');
    const [sortOrder, setSortOrder] = useState('asc');
    const [filterGenre, setFilterGenre] = useState('');
    const [newFilm, setNewFilm] = useState(initialFilmState());

    function initialFilmState() {
        return {
            title: '',
            genre: '',
            director: '',
            releaseYear: '',
            rating: '',
            description: '',
        };
    }

    const fetchFilms = async () => {
        try {
            const response = await fetch(`http://localhost:8080/api/films?sortBy=${sortOption}&sortOrder=${sortOrder}`);
            if (!response.ok) throw new Error('Failed to fetch films');
            const data = await response.json(); // Now you can parse the response
            setFilms(data);
        } catch (error) {
            console.error('Error fetching films:', error);
        }
    };

    const handleFilterAndSort = (filmsData) => {
        let filtered = filmsData;

        if (searchQuery) {
            filtered = filtered.filter((film) =>
                film.title.toLowerCase().includes(searchQuery.toLowerCase())
            );
        }

        if (filterGenre) {
            filtered = filtered.filter((film) => film.genre === filterGenre);
        }

        filtered.sort((a, b) => {
            if (sortOrder === 'asc') {
                return a[sortOption] > b[sortOption] ? 1 : -1;
            } else {
                return a[sortOption] < b[sortOption] ? 1 : -1;
            }
        });

        setFilteredFilms(filtered);  
    };

    useEffect(() => {
        fetchFilms();
    }, [sortOption, sortOrder]);

    useEffect(() => {
        handleFilterAndSort(films);  
    }, [films, searchQuery, filterGenre, sortOption, sortOrder]);

    const handleAddFilm = async () => {
        const filmToAdd = {
            ...newFilm,
            releaseYear: newFilm.releaseYear.toString(),
            rating: parseFloat(newFilm.rating),
        };

        try {
            const response = await fetch('http://localhost:8080/api/films', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(filmToAdd),
            });
            if (!response.ok) throw new Error('Failed to add film');
            fetchFilms();
            setNewFilm(initialFilmState());
        } catch (error) {
            console.error('Error adding film:', error);
        }
    };

    const handleDeleteFilm = async (id) => {
        try {
            const response = await fetch(`http://localhost:8080/api/films/${id}`, {
                method: 'DELETE',
            });
            if (!response.ok) throw new Error('Failed to delete film');
            fetchFilms();
        } catch (error) {
            console.error('Error deleting film:', error);
        }
    };

    const handleUpdateFilm = async () => {
        const filmToUpdate = {
            ...newFilm,
            releaseYear: newFilm.releaseYear.toString(),
            rating: parseFloat(newFilm.rating),
        };

        try {
            const response = await fetch(`http://localhost:8080/api/films/${newFilm.id}`, {
                method: 'PUT',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(filmToUpdate),
            });
            if (!response.ok) throw new Error('Failed to update film');
            fetchFilms();
            setNewFilm(initialFilmState());  
        } catch (error) {
            console.error('Error updating film:', error);
        }
    };

    const handleInputChange = (field, value) => {
        setNewFilm({ ...newFilm, [field]: value });
    };

    return (
        <div className="container">
            <h1>Film Management</h1>

            <div className="form">
                <h2>{newFilm.id ? 'Edit Film' : 'Add New Film'}</h2>
                {['title', 'genre', 'director', 'description'].map((field) => (
                    <input
                        key={field}
                        type="text"
                        placeholder={capitalize(field)}
                        value={newFilm[field]}
                        onChange={(e) => handleInputChange(field, e.target.value)}
                    />
                ))}
                <input
                    type="number"
                    placeholder="Release Year"
                    value={newFilm.releaseYear}
                    onChange={(e) => handleInputChange('releaseYear', e.target.value)}
                />
                <input
                    type="number"
                    step="0.1"
                    placeholder="Rating"
                    value={newFilm.rating}
                    onChange={(e) => handleInputChange('rating', e.target.value)}
                />
                <button onClick={newFilm.id ? handleUpdateFilm : handleAddFilm}>
                    {newFilm.id ? 'Update Film' : 'Add Film'}
                </button>
            </div>

            <div className="controls">
                <input
                    type="text"
                    placeholder="Search by Title"
                    value={searchQuery}
                    onChange={(e) => setSearchQuery(e.target.value)}
                />
                <select
                    value={filterGenre}
                    onChange={(e) => setFilterGenre(e.target.value)}
                >
                    <option value="">All Genres</option>
                    {[...new Set(films.map((film) => film.genre))].map((genre) => (
                        <option key={genre} value={genre}>
                            {genre}
                        </option>
                    ))}
                </select>
                <select
                    value={sortOption}
                    onChange={(e) => setSortOption(e.target.value)}
                >
                    <option value="title">Sort by Title</option>
                    <option value="rating">Sort by Rating</option>
                    <option value="releaseYear">Sort by Release Year</option>
                </select>
                <select
                    value={sortOrder}
                    onChange={(e) => setSortOrder(e.target.value)}
                >
                    <option value="asc">Ascending</option>
                    <option value="desc">Descending</option>
                </select>
            </div>

            <table className="films-table">
                <thead>
                    <tr>
                        <th>Title</th>
                        <th>Genre</th>
                        <th>Director</th>
                        <th>Release Year</th>
                        <th>Rating</th>
                        <th>Description</th>
                        <th>Actions</th>
                    </tr>
                </thead>
                <tbody>
                    {filteredFilms.map((film) => (
                        <tr key={film.id}>
                            <td>{film.title}</td>
                            <td>{film.genre}</td>
                            <td>{film.director}</td>
                            <td>{film.releaseYear}</td>
                            <td>{film.rating}</td>
                            <td>{film.description}</td>
                            <td>
                                <button onClick={() => setNewFilm(film)}>Edit</button>
                                <button onClick={() => handleDeleteFilm(film.id)}>Delete</button>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
};

const capitalize = (text) => text.charAt(0).toUpperCase() + text.slice(1);

export default FilmsTable;
