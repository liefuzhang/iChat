import React from "react";
import { toast } from "react-toastify";
import ApiService from "services/ApiService";

class EditProfileForm extends React.Component {
  constructor(props) {
    super(props);

    this.onEditProfileFormSubmit = this.onEditProfileFormSubmit.bind(this);
    this.apiService = new ApiService(props);
    this.displayName = undefined;
    this.phoneNumber = undefined;
    this.state = {
      displayName: props.userProfile.displayName || "",
      phoneNumber: props.userProfile.phoneNumber || ""
    };
  }

  onEditProfileFormSubmit(event) {
    event.preventDefault();

    let button = event.currentTarget.querySelector("button[type='submit']");
    button.classList.add("disabled-button");

    let name = event.target.elements["displayName"].value;
    let phoneNumber = event.target.elements["phoneNumber"].value;

    this.apiService
      .fetch(`/api/users/editProfile`, {
        method: "POST",
        body: JSON.stringify({ displayName: name, phoneNumber: phoneNumber })
      })
      .then(() => {
        this.props.onClose();
        this.props.onProfileUpdated();
      })
      .catch(error => {
        toast.error(`Edit profile failed: ${error}`);
        button.classList.remove("disabled-button");
      });
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
            <label htmlFor="displayName">Display Name</label>
            <input
              className="form-control"
              type="text"
              id="displayName"
              name="displayName"
              placeholder="Display name"
              value={this.state.displayName}
              onChange={event =>
                this.setState({ displayName: event.target.value })
              }
              required
            />
          </div>
          <div className="form-group">
            <label htmlFor="telephone">Phone Number</label>
            <input
              className="form-control"
              type="text"
              id="phoneNumber"
              name="phoneNumber"
              placeholder="Phone Number"
              pattern="[0-9-]*"
              title="Phone number can only contain numbers and '-'"
              value={this.state.phoneNumber}
              onChange={event =>
                this.setState({ phoneNumber: event.target.value })
              }
              required
            />
          </div>
          <button type="submit" className="btn form-control">
            Save Changes
          </button>
        </form>
      </div>
    );
  }
}

export default EditProfileForm;
