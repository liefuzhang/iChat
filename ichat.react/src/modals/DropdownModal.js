import React from "react";
import ReactDOM from "react-dom";
import "./DropdownModal.css";

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

    document
      .querySelector(".dropdown-modal-panel-overlay")
      .addEventListener("click", this.onOverlayClick);
  }

  componentWillUnmount() {
    document
      .querySelector(".dropdown-modal-panel-overlay")
      .removeEventListener("click", this.onOverlayClick);

    this.dropdownModalRoot.removeChild(this.el);
  }

  render() {
    var modal = (
      <div className="dropdown-modal-panel-container">
        <div className="dropdown-modal-panel-overlay">
          <div className="dropdown-modal-panel">
            {this.props.children}
          </div>
        </div>
      </div>
    );

    return ReactDOM.createPortal(modal, this.el);
  }
}

export default DropdownModal;
