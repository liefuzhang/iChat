'use strict';

class LikeButton extends React.Component {
    constructor(props) {
        super(props);
        this.state = { liked: false };
    }

    render() {
        if (this.state.liked) {
            return 'You liked this.';
        }

        return (
            <div>
                <button onClick={() => this.setState({ liked: true })}>
                    Like
                </button>
            </div>
            <p>
                <span className="ds">staRT
                    </span>
                <a>asdf</a>
                <a className="App-link"
                    href="https://reactjs.org"
                    target="_blank"
                    rel="noopener noreferrer">
                    Learn React</a>
            </p>
                    );
    }
}

const domContainer = document.querySelector('#like_button_container');
ReactDOM.render(<LikeButton />, domContainer);