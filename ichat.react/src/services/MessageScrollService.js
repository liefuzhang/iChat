class MessageScrollService {
  constructor(params) {
    this.params = params;
    this.lastGroupIndex = -1;
    this.offsetTops = [];
    this.messageGroupAnchors = [];
    this.scrolled = false;

    this.onScrollHandler = this.onScrollHandler.bind(this);
    this.calculateMessageGroupOffsetTops = this.calculateMessageGroupOffsetTops.bind(
      this
    );
    this.calculateCurrentGroup = this.calculateCurrentGroup.bind(this);
  }

  reset() {
    this.scrollableElement = document.querySelector(
      this.params.scrollableElementSelector
    );
    this.scrollableElement.onscroll = this.onScrollHandler;
    this.lastGroupIndex = -1;
    this.calculateMessageGroupOffsetTops();
    this.calculateCurrentGroup();
  }

  onScrollHandler() {
    if (this.scrollableElement.scrollTop === 0) {
      this.savedScrollHeight = this.scrollableElement.scrollHeight;
      this.params.onScrollToTop();
    }
    this.calculateCurrentGroup();
  }

  resumeScrollPosition() {
    if (this.savedScrollHeight) {
      this.scrollableElement.scrollTop =
        this.scrollableElement.scrollHeight - this.savedScrollHeight;
      this.savedScrollHeight = undefined;
    }
  }

  calculateMessageGroupOffsetTops() {
    this.messageGroupAnchors = Array.from(
      this.scrollableElement.querySelectorAll(this.params.anchorSelector)
    );
    this.offsetTops = this.messageGroupAnchors.map(function(a) {
      return a.offsetTop;
    });
  }

  calculateCurrentGroup() {
    if (this.messageGroupAnchors.length === 0) return;
    var adjustHeight = this.messageGroupAnchors[0].offsetHeight / 2;
    var currentGroupIndex = -1;
    var startIndex = this.lastGroupIndex - 1;
    startIndex = startIndex < 0 ? 0 : startIndex;
    for (var i = startIndex; i < this.offsetTops.length; i++) {
      if (
        this.scrollableElement.scrollTop <
        this.offsetTops[i] + adjustHeight
      ) {
        break;
      }
      currentGroupIndex = i;
    }
    if (this.lastGroupIndex !== currentGroupIndex) {
      let currentAnchor = this.scrollableElement.querySelector(
        `.${this.params.stickyItemClassName}`
      );
      currentAnchor &&
        currentAnchor.classList.remove(this.params.stickyItemClassName);
      if (currentGroupIndex !== -1) {
        this.messageGroupAnchors[currentGroupIndex].classList.add(
          this.params.stickyItemClassName
        );
      }
      this.lastGroupIndex = currentGroupIndex;
    }
  }

  scrollToBottom() {
    this.scrollableElement ||
      (this.scrollableElement = document.querySelector(
        this.params.scrollableElementSelector
      ));
    this.scrollableElement.scrollTop = this.scrollableElement.scrollHeight;
  }
}

export default MessageScrollService;
