export default React.createClass({
    render: function () {
        var imageUrl = null;
        if (this.props.data.movieInfo == null) {
            var placeHolderUrl1 = "https://placeholdit.imgix.net/~text?txtsize=47&txt=";
            var placeHolderUrl2 = "&w=500&h=750&txttrack=0";
            var imageUrlName = this.props.data.name.replace(" ", "+");
            imageUrl = placeHolderUrl1 + imageUrlName + placeHolderUrl2;
        }
        else {
            imageUrl = this.props.data.movieInfo.imageUrl;
        }
        var style = {
            backgroundImage: "url(" + imageUrl + ")"
        };
        var corner = "";
        if(this.props.data.quality==1 || this.props.data.quality==-1){
            corner = "cornerRed";
        }
        else if(this.props.data.quality==2 || this.props.data.quality==-2){
            corner = "cornerYellow";
        }
        else if(this.props.data.quality==3 || this.props.data.quality==-4){
            corner = "cornerGreen";
        }
        return (
            <div className={"movie-image hand " + corner} style={style}
                 onClick={()=>this.props.expand(this.props.index)}/>
        );
    }
});