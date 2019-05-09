import * as signalR from "@aspnet/signalr";
import AuthService from "./AuthService";

class SignalRHubService {
  constructor(props) {
    this.connection = new signalR.HubConnectionBuilder()
      .withUrl("/chatHub", {
        accessTokenFactory: () => new AuthService().getToken()
      })
      .build();
  }

  connect() {
    return this.connection.start().catch(function(err) {
      return console.error(err.toString());
    });
  }

  addUserToChannelGroup(channelID) {
    this.connection.invoke("AddToChannelGroup", channelID).catch(function(err) {
      return console.error(err.toString());
    });
  }
}

export default SignalRHubService;
