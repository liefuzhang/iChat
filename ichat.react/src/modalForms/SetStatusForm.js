import React from "react";
import "./SetStatusForm.css";
import UserStatus from "services/UserStatusService";
import { toast } from "react-toastify";
import ApiService from "services/ApiService";

class SetStatusForm extends React.Component {
  constructor(props) {
    super(props);

    this.onSetStatusFormSubmit = this.onSetStatusFormSubmit.bind(this);
    this.apiService = new ApiService(props);
    this.statusList = new UserStatus().getStatusList();

    this.state = {
      selectedStatus: undefined
    };
  }

  onSetStatusFormSubmit(event) {
    event.preventDefault();
    if (!this.state.selectedStatus) return;

    event.currentTarget
      .querySelector("button[type='submit']")
      .classList.add("disabled-button");

    this.apiService
      .fetch(`/api/users/status`, {
        method: "POST",
        body: JSON.stringify(this.state.selectedStatus)
      })
      .then(id => {
        this.props.onClose();
        this.props.onSelect();
      })
      .catch(error => {
        toast.error(`Save status failed: ${error}`);
      });
  }

  onSelectStatus(status) {
    this.selectedStatus = status;
    this.setState({ selectedStatus: status });
  }

  render() {
    return (
      <div className="form-container">
        <h1 style={{ textAlign: "center" }}>Set a status</h1>
        <form
          id="setStatusForm"
          method="post"
          onSubmit={this.onSetStatusFormSubmit}
        >
          <p className="form-description">
            Set your status by clicking one of the following:
          </p>
          <ul className="status-list">
            {this.statusList.map(s => {
              let selected = false;
              if (s.value === this.state.selectedStatus) selected = true;
              return (
                <li
                  key={s.value}
                  className={selected ? "selected-status" : ""}
                  onClick={() => this.onSelectStatus(s.value)}
                >
                  {s.name}
                </li>
              );
            })}
          </ul>
          <button type="submit" className="btn form-control">
            Save
          </button>
        </form>
      </div>
    );
  }
}

export default SetStatusForm;
