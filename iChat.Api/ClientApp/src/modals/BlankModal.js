import React from "react";
import ReactDOM from "react-dom";

class BlankModal extends React.Component {
  constructor(props) {
    super(props);

    this.modalRoot = document.getElementById("blank-modal-root");
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
      <div className="blank-modal-panel">
        {this.props.children}
      </div>
    );

    return ReactDOM.createPortal(modal, this.el);
  }
}

export default BlankModal;
