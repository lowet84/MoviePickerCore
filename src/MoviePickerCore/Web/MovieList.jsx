var MovieInfo = require('./MovieInfo.jsx');
var MovieBlock = require('./MovieBlock.jsx');

export default class MovieList extends React.Component {
    constructor(options) {
        super(options);
        var data = [{}, {}, {}, {}, {}, {}, {}, {}, {}, {}, {}, {}];
        this.state = {data: data, expanded: null, page: 0};
        this.updatePage()
    };

    componentDidMount() {
        this.setState(this.state);
    };

    expand(index) {
        var state = this.state;
        if(index == null || state.data[index].name!=undefined) {
            state.expanded = index;
            this.setState(state);
        }
    };

    updatePage() {
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
    };

    changePage(change) {
        var state = this.state;
        state.page += change;
        state.data= [{}, {}, {}, {}, {}, {}, {}, {}, {}, {}, {}, {}];
        if (state.page < 0) {
            state.page = 0;
        }
        this.setState(state);
        this.updatePage();
    };

    download(index) {
        var state = this.state;
        var link = state.data[state.expanded].results[index].link;
        state.data[state.expanded].results[index].downloading = 1;
        this.setState(state);
        $.ajax({
            url: this.props.url,
            type: 'POST',
            data: {link: link},
            success: function () {
                var state = this.state;
                state.data[state.expanded].results[index].downloading = 2;
                this.setState(state);
            }.bind(this, index),
            error: function (xhr, status, err) {
                var state = this.state;
                state.data[state.expanded].results[index].downloading = 3;
                this.setState(state);
                console.error(this.props.url, status, err.toString());
            }.bind(this,index)
        });
    };

    render() {
        if (this.state.expanded == null) {
            return <MovieBlock data={this.state.data} parent={this}/>;
        }
        else {
            return <MovieInfo data={this.state.data[this.state.expanded]} parent={this}/>;
        }
    }
};
