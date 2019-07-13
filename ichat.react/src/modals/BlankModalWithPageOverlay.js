import React from "react";
import BlankModal from "modals/BlankModal";

class BlankModalWithPageOverlay extends React.Component {
  constructor(props) {
    super(props);

    this.closePopup = this.closePopup.bind(this);
  }

  closePopup() {
    this.props.onClose();
  }

  calculatePostion() {
    let gap = 10;
    let popup = document.querySelector(".page-overlay-content");
    let popupContent = popup.firstChild;
    let popupContentRect = popupContent.getBoundingClientRect();
    let targetRect = this.props.clickedTarget.getBoundingClientRect();
    let top = targetRect.bottom - popupContentRect.height;
    let left = targetRect.right + gap;
    let contentHeaderHeight = document.querySelector(".content-header")
      .offsetHeight;
    let translateY = 0;
    let translateX = 0;
    if (top < contentHeaderHeight) translateY = contentHeaderHeight - top + gap;
    if (left + popupContentRect.right > window.innerWidth)
      translateX = window.innerWidth - (left + popupContentRect.right) - gap;

    popup.setAttribute(
      "style",
      `top:${top}px; left: ${left}px; visibility: visible; transform: translate(${translateX}px, ${translateY}px)`
    );
  }

  componentDidMount() {
    this.calculatePostion();
  }

  render() {
    return (
      <BlankModal>
        <div>
          <div className="page-overlay" onClick={this.closePopup} />
          <div className="page-overlay-content invisible">
            {this.props.children}
          </div>
        </div>
      </BlankModal>
    );
  }
}

export default BlankModalWithPageOverlay;
