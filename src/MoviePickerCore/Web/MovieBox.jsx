export default React.createClass({
    getInitialState: function () {
        return {data: []};
    },
    expand: function (index) {
        var data = this.state.data;
        data[index].expanded = data[index].expanded != true;

        this.setState({data: data});
    },
    componentDidMount: function () {
        $.ajax({
            url: this.props.url+"/20",
            dataType: 'json',
            cache: false,
            success: function (data) {
                this.setState({data: data});
            }.bind(this),
            error: function (xhr, status, err) {
                console.error(this.props.url, status, err.toString());
            }.bind(this)
        });
    },
    download: function (link) {
        $.ajax({
            url: this.props.url,
            type: 'POST',
            data: {link: link},
            success: function (data) {
                var x = 0;
                var y = 0;
            }.bind(this),
            error: function (xhr, status, err) {
                console.error(this.props.url, status, err.toString());
            }.bind(this)
        });
    },
    render: function () {
        var movies = this.state.data.map(function (item, index) {
            if (item.expanded == true) {
                var children = item.results.map(function (item, index) {
                    return (
                        <div key={this.index + "_" + index}>
                            <a onClick={()=>this.download(item.link)} href="/#">[{item.releaseTitle}]</a>
                        </div>
                    );
                }, this);
                return (
                    <div key={index}>
                        <p onClick={()=>this.expand(index)}>
                            {item.name} ({item.year}) {item.seeders}
                        </p>
                        {children}
                    </div>
                );
            }
            else {
                return (
                    <p key={index} onClick={()=>this.expand(index)}>
                        {item.name} ({item.year}) {item.seeders} {item.quality}
                    </p>
                );
            }
        }, this);
        return (
            <div>
                <h1>Top Movies!</h1>
                <div>{movies}</div>
            </div>
        );
    }
});
