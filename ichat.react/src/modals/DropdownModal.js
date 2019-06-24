import React from "react";
import ReactDOM from "react-dom";

class DropdownModal extends React.Component {
  constructor(props) {
    super(props);

    this.onOverlayClick = this.onOverlayClick.bind(this);

    this.dropdownModalRoot = document.getElementById("dropdown-modal-root");
    this.el = document.createElement("div");
  }

  onOverlayClick(event) {
    if (!event.target.closest(".dropdown-modal-panel")) {
      this.props.onClose();
    }
  }

  componentDidMount() {
    this.dropdownModalRoot.appendChild(this.el);
  }

  componentWillUnmount() {
    this.dropdownModalRoot.removeChild(this.el);
  }

  render() {
    var modal = (
      <div className="dropdown-modal-panel-container">
        <div
          className="dropdown-modal-panel-overlay page-overlay"
          onClick={this.onOverlayClick}
        >
          <div className="dropdown-modal-panel">{this.props.children}</div>
        </div>
      </div>
    );

    return ReactDOM.createPortal(modal, this.el);
  }
}

export default DropdownModal;
