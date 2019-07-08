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

    urls.forEach(url => {
      var options = { url: url };
      openGraphScraper(options, function(error, results) {
        console.log("error:", error);
        console.log("results:", results);
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
    return <div className="message-item-url-preview-container" />;
  }
}

export default ContentMessageItemUrlPreview;
