class UserStatusService {
  getStatusList() {
    return [
      { name: "In A Meeting", value: "InAMeeting" },
      { name: "Commuting", value: "Commuting" },
      { name: "Out Sick", value: "OutSick" },
      { name: "Vacationing", value: "Vacationing" },
      { name: "Working Remotely", value: "WorkingRemotely" }
    ];
  }

  getStatusName(value) {
    let list = this.getStatusList();
    let status = list.find(s => s.value === value);
    return status ? status.name : "";
  }

  isNotActive(status) {
    return status !== "Active";
  }
}

export default UserStatusService;
