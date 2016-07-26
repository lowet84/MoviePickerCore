var MovieBox = require('./MovieBox.jsx');

export default React.createClass({
    render: function () {
        var data = this.props.data;
        var overview = data.movieInfo == null ? "" : data.movieInfo.overview;
        var releases = data.results.map(function (item, index) {
            var status = undefined;
            switch (item.downloading) {
                case 1:
                    status = (<span>&nbsp;[Downloading]</span>);
                    break;
                case 2:
                    status = (<span>&nbsp;[OK]</span>);
                    break;
                case 3:
                    status = (<span>&nbsp;[Error!]</span>);
                    break;
            }
            return (
                <div className="hand col-xs-12" key={"release_" + index}>
                    <a onClick={()=> {
                        this.props.parent.download(index)
                    }}>{item.releaseTitle}</a>{status}
                </div>);
        }, this);
        return (
            <div className="container">
                <div className="row">
                    <h2 className="col-xs-12 col-md-2 hand underline"
                        onClick={()=>this.props.parent.expand(null)}>back</h2>
                    <h1 className="col-xs-12 col-md-10">
                        {data.name}
                    </h1>
                </div>
                <div className="row bottom-padding">
                    <div className="col-md-3 clearfix visible-md-block visible-lg-block">
                        <MovieBox data={data} expand={()=> {
                        }} index={null}/>
                    </div>
                    <div className="col-xs-12 col-md-9">{overview}</div>
                </div>
                <div className="row">
                    {releases}
                </div>
            </div>)
    }
});