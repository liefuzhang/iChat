import React from "react";
import "./Content.css";
import ContentHeader from "content/ContentHeader";
import ContentMessages from "content/ContentMessages";
import ContentFooter from "content/ContentFooter";
import { Loader, Image, Segment } from "semantic-ui-react";

class Content extends React.Component {
  constructor(props) {
    super(props);

    this.onFinishLoading = this.onFinishLoading.bind(this);

    this.state = {
      isPageLoading: true
    };
  }

  onFinishLoading() {
    setTimeout(() => {
      this.setState({ isPageLoading: false });
    }, 1000);
  }

  render() {
    return (
      <div id="content">
        {this.state.isPageLoading && (
          <Segment>
            <Loader active>Loading</Loader>
            <Image src="https://react.semantic-ui.com/images/wireframe/short-paragraph.png" />
            <Image src="https://react.semantic-ui.com/images/wireframe/short-paragraph.png" />
          </Segment>
        )}
        <ContentHeader isChannel={this.props.isChannel} id={this.props.id} />
        <ContentMessages
          section={this.props.section}
          id={this.props.id}
          hubConnection={this.props.hubConnection}
          userProfile={this.props.userProfile}
          onFinishLoading={this.onFinishLoading}
        />
        <ContentFooter
          isChannel={this.props.isChannel}
          section={this.props.section}
          id={this.props.id}
          hubConnection={this.props.hubConnection}
        />
      </div>
    );
  }
}

export default Content;
