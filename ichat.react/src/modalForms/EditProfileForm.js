import React from "react";
import { toast } from "react-toastify";
import ApiService from "services/ApiService";

class EditProfileForm extends React.Component {
  constructor(props) {
    super(props);

    this.onEditProfileFormSubmit = this.onEditProfileFormSubmit.bind(this);
    this.apiService = new ApiService(props);
  }

  onEditProfileFormSubmit(event) {
    event.preventDefault();
    // if (!this.selectedChannelId) return;

    // let button = event.currentTarget.querySelector("button[type='submit']");
    // button.classList.add("disabled-button");

    // this.apiService
    //   .fetch(`/api/channels/join`, {
    //     method: "POST",
    //     body: JSON.stringify(this.selectedChannelId)
    //   })
    //   .then(() => {
    //     this.props.onChannelJoined();
    //     this.props.history.push(`/channel/${this.selectedChannelId}`);
    //   })
    //   .catch(error => {
    //     toast.error(`Join channel failed: ${error}`);
    // button.classList.remove("disabled-button");
    //   });
  }

  render() {
    return (
      <div className="form-container edit-profile-form">
        <h1 style={{ textAlign: "center" }}>Edit profile</h1>

        <form
          id="EditProfileForm"
          method="post"
          onSubmit={this.onEditProfileFormSubmit}
        >
          <div className="form-group">
            <label htmlFor="displayName">Name</label>
            <input
              className="form-control"
              type="text"
              id="displayName"
              name="name"
              placeholder="e.g. display name"
              value={}
              required
            />
          </div>
          <button type="submit" className="btn form-control">
            Join
          </button>
        </form>
      </div>
    );
  }
}

export default EditProfileForm;
