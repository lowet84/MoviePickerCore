var MovieBox = require('./MovieBox.jsx');

export default React.createClass({
    getInitialState: function () {
        return {data: [], expanded: null, page: 0};
    },
    expand: function (index) {
        var state = this.state;
        state.expanded = index;
        this.setState(state);
    },
    close: function () {
        var state = this.state;
        state.expanded = null;
        this.setState(state);
    },
    componentDidMount: function () {
        this.updatePage();
    },
    updatePage: function () {
        $.ajax({
            url: this.props.url + this.state.page,
            dataType: 'json',
            cache: false,
            success: function (data) {
                var state = this.state;
                state.data = data;
                this.setState(state);
            }.bind(this),
            error: function (xhr, status, err) {
                console.error(this.props.url, status, err.toString());
            }.bind(this)
        });
    },
    changePage: function (change) {
        var state = this.state;
        state.page += change;
        if (state.page < 0) {
            state.page = 0;
        }
        this.setState(state);
        this.updatePage();
    },
    download: function (link) {
        $.ajax({
            url: this.props.url,
            type: 'POST',
            data: {link: link},
            success: function (data) {

            }.bind(this),
            error: function (xhr, status, err) {
                console.error(this.props.url, status, err.toString());
            }.bind(this)
        });
    },
    render: function () {
        if (this.state.expanded == null) {
            var movies = this.state.data.map(function (item, index) {
                var movie = (<MovieBox key={"moviebox_" + index} data={item} expand={this.expand} index={index}/>);
                var items = [];
                if (index % 2 == 0) {
                    items.push((<div key={"clearfix_2" + index} className="clearfix visible-sm-block"></div>))
                }
                if (index % 3 == 0) {
                    items.push((<div key={"clearfix_3" + index} className="clearfix visible-md-block"></div>))
                }
                if (index % 4 == 0) {
                    items.push((<div key={"clearfix_4" + index} className="clearfix visible-lg-block"></div>))
                }
                items.push(movie);
                return (
                    <div key={index} className="col-lg-2 col-md-3 col-sm-4 col-xs-6 bottom-padding">
                        {items}
                    </div>
                );
            }, this);
            return (
                <div className="container">
                    <h3>Top movies</h3>
                    <div className="row">
                        {movies}
                    </div>
                    <div className="row">
                        <div className="col-xs-4"></div>
                        <div className="col-xs-1">
                            <button onClick={()=>this.changePage(-1)}>Prev</button>
                        </div>
                        <div className="col-xs-1">
                            <button onClick={()=>this.changePage(1)}>Next</button>
                        </div>
                        <div className="col-xs-4"></div>
                    </div>
                </div>
            );
        }
        else {
            var data = this.state.data[this.state.expanded];
            var overview = data.movieInfo == null ? "" : data.movieInfo.overview;
            var releases = data.results.map(function (item, index) {
                return (
                    <div className="row">
                        <a key={"release_" + index} className="hand col-xs-12" onClick={()=>{this.download(item.link)}}>
                            {item.releaseTitle}
                        </a>
                    </div>);
            },this);
            return (
                <div className="container">
                    <div className="row">
                        <h2 className="col-xs-12 col-md-2 hand underline" onClick={()=>this.expand(null)}>back</h2>
                        <h1 className="col-xs-12 col-md-10">
                            {this.state.data[this.state.expanded].name}
                        </h1>
                    </div>
                    <div className="row bottom-padding">
                        <div className="col-md-3 clearfix visible-md-block visible-lg-block">
                            <MovieBox data={data} expand={()=> {
                            }} index={null}/>
                        </div>
                        <div className="col-xs-12 col-md-9">{overview}</div>
                    </div>
                    {releases}
                </div>)
        }
    }
});
