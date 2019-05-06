import React from "react";
import $ from "jquery"
import simplebar from "simplebar"
import logo from "./logo.svg";
import "./App.css";
import Sidebar from './Sidebar';
import signalR from "@aspnet/signalr"

function App() {
  return (
    <div id="container" className="page-loading">
      <Sidebar/>
    </div>
  );
}

export default App;
