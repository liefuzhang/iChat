import React from "react";
import ReactDOM from "react-dom";
import "./Modal.css";
import CloseButton from "./components/CloseButton"

class Modal extends React.Component {
  constructor(props) {
    super(props);

    this.onOverlayClick = this.onOverlayClick.bind(this);

    this.modalRoot = document.getElementById("modal-root");
    this.el = document.createElement("div");
  }

  onOverlayClick(event) {
    if (!event.target.closest(".modal-panel")) {
      this.props.onClose();
    }
  }

  componentDidMount() {
    this.modalRoot.appendChild(this.el);

    document
      .querySelector(".modal-panel-overlay")
      .addEventListener("click", this.onOverlayClick);
  }

  componentWillUnmount() {
    document
      .querySelector(".modal-panel-overlay")
      .removeEventListener("click", this.onOverlayClick);

    this.modalRoot.removeChild(this.el);
  }

  render() {
    var modal = (
      <div className="modal-panel-container">
        <div className="modal-panel-overlay">
          <div className="modal-panel panel">
            <CloseButton onClose={this.props.onClose} />
            {this.props.children}
          </div>
        </div>
      </div>
    );

    return ReactDOM.createPortal(modal, this.el);
  }
}

export default Modal;
