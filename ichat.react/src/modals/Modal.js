import React from "react";
import ReactDOM from "react-dom";
import "./Modal.css";
import CloseButton from "components/CloseButton"
import SimpleBar from "simplebar-react";
import "lib/simplebar.css";

class Modal extends React.Component {
  constructor(props) {
    super(props);

    this.modalRoot = document.getElementById("modal-root");
    this.el = document.createElement("div");
  }

  componentDidMount() {
    this.modalRoot.appendChild(this.el);
  }

  componentWillUnmount() {
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
