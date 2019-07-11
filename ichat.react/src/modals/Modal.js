import React from "react";
import ReactDOM from "react-dom";
import "./Modal.css";
import CloseButton from "components/CloseButton";
import SimpleBar from "simplebar-react";
import "lib/simplebar.css";

class Modal extends React.Component {
  constructor(props) {
    super(props);

    this.onKeyup = this.onKeyup.bind(this);

    this.modalRoot = document.getElementById("modal-root");
    this.el = document.createElement("div");
  }

  onKeyup(e) {
    if (e.key === "Enter") {
      let button = this.modalRoot.querySelector("button[type='submit']");
      button && button.click();
    }
  }

  componentDidMount() {
    this.modalRoot.appendChild(this.el);
    document.addEventListener("keyup", this.onKeyup);
  }

  componentWillUnmount() {
    document.removeEventListener("keyup", this.onKeyup);
    this.modalRoot.removeChild(this.el);
  }

  render() {
    var modal = (
      <div className="modal-panel-container">
        <div className="modal-panel-overlay">
          <SimpleBar className="modal-panel panel">
            <CloseButton onClose={this.props.onClose} />
            {this.props.children}
          </SimpleBar>
        </div>
      </div>
    );

    return ReactDOM.createPortal(modal, this.el);
  }
}

export default Modal;
