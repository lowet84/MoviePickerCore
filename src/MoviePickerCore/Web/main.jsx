var MovieList = require('./MovieList.jsx');

ReactDOM.render(
    <MovieList url="/api/values/"/>,
    document.getElementById('content')
);