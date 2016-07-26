var MovieBox = require('./MovieBox.jsx');

export default class MovieBlock extends React.Component {
    render(){
        var movies = this.props.data.map(function (item, index) {
            var movie = (<MovieBox key={"moviebox_" + index} data={item} index={index} parent={this.props.parent}/>);
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
                        <button onClick={()=>this.props.parent.changePage(-1)}>Prev</button>
                    </div>
                    <div className="col-xs-1">
                        <button onClick={()=>this.props.parent.changePage(1)}>Next</button>
                    </div>
                    <div className="col-xs-4"></div>
                </div>
            </div>
        );
    }
}