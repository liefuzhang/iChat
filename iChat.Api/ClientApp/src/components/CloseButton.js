import React from "react";
import "./CloseButton.css";
import { Icon } from "semantic-ui-react";

class CloseButton extends React.Component {
  constructor(props) {
    super(props);

    this.onKeyup = this.onKeyup.bind(this);
  }

  componentDidMount() {
    document.addEventListener("keyup", this.onKeyup);
  }

  componentWillUnmount() {
    document.removeEventListener("keyup", this.onKeyup);
  }

  onKeyup(e) {
    if (e.key === "Escape") {
      this.props.onClose();
    }
  }

  render() {
    return (
      <div>
        <button className="close-button" onClick={this.props.onClose}>
          <div style={{ fontSize: "1.5rem" }}>
            <Icon name="times" />
          </div>
          <div>esc</div>
        </button>
      </div>
    );
  }
}

export default CloseButton;
