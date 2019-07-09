import React from "react";
import "./ContentMessageItem.UrlPreview.css";
import openGraphScraper from "open-graph-scraper";

class ContentMessageItemUrlPreview extends React.Component {
  constructor(props) {
    super(props);

    this.state = {
      openGraphObjects: []
    };
  }

  fetchOpenGraphObjects(props) {
    if (!props.content) return;
    let urls = [];
    let groups = [];
    let regex = props.aTagRegex;
    regex.lastIndex = 0;
    while ((groups = regex.exec(props.content)) !== null) {
      urls.push(groups[1]);
    }
    let urlCount = urls.length;
    let loadedCount = 0;
    let returnedObjects = [];

    urls.forEach(url => {
      var options = { url: url };
      openGraphScraper(options, (error, results) => {
        console.log(results);
        if (results.success) {
          returnedObjects.push(results.data);
        }
        if (++loadedCount === urlCount) {
          this.setState({ openGraphObjects: returnedObjects });
        }
      });
    });
  }

  componentDidMount() {
    this.fetchOpenGraphObjects(this.props);
  }

  componentDidUpdate(prevProps) {
    if (this.props.content !== prevProps.content)
      this.fetchOpenGraphObjects(this.props);
  }

  render() {
    return (
      <div className="message-item-url-preview-container">
        {/* {this.state.openGraphObjects.map(object => {
          <div className="url-preview-item">
            <div className="url-preview-item-site-name">object.ogSiteName</div>
            <div className="url-preview-item-title">object.ogTitle</div>
            <div className="url-preview-item-title">object.ogTitle</div>
          </div>;
        })} */}
      </div>
    );
  }
}

export default ContentMessageItemUrlPreview;
