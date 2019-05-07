import React from "react";
import * as signalR from '@aspnet/signalr';

class SignalRHub extends React.Component {
  constructor(props) {
    super(props);

    // let connection = new signalR.HubConnectionBuilder()
    //   .withUrl("/chatHub")
    //   .build();

    // connection
    //   .start()
    //   .then(function() {
    //     document.querySelector(".page-loading").classList.remove("page-loading");
    //   })
    //   .catch(function(err) {
    //     return console.error(err.toString());
    //   });
  }

  render() {
    return <div className="page-loading" />;
  }
}

export default SignalRHub;
