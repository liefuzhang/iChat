import React from "react";
import AuthService from "services/AuthService";
import "./SetStatusForm.css";
import { stat } from "fs";

class SetStatusForm extends React.Component {
  constructor(props) {
    super(props);

    this.onSetStatusFormSubmit = this.onSetStatusFormSubmit.bind(this);
    this.authService = new AuthService(props);
    this.statusList = [
      { name: "Active", value: "Active" },
      { name: "In A Meeting", value: "InAMeeting" },
      { name: "Commuting", value: "Commuting" },
      { name: "Out Sick", value: "OutSick" },
      { name: "Vacationing", value: "Vacationing" },
      { name: "Working Remotely", value: "WorkingRemotely" }
    ];

    this.state = {
      selectedStatus: undefined
    };
  }

  onSetStatusFormSubmit(event) {
    event.preventDefault();
    if (!this.state.selectedStatus) return;

    this.authService
      .fetch(`/api/users/status`, {
        method: "POST",
        body: this.state.selectedStatus
      })
      .then(id => {
        alert("status set!")
        this.props.onClose();
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
