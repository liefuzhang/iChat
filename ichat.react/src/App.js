import React from "react";
import simplebar from "simplebar";
import logo from "./logo.svg";
import "./App.css";
import Sidebar from "./Sidebar";
import Content from "./Content";
import signalR from "@aspnet/signalr";
import { BrowserRouter, Route, Link } from "react-router-dom";

function App() {
  return (
    <div id="container" className="page-loading">
      <BrowserRouter>
        <Sidebar />
        <Route path={`/:section/:id`} component={Content} />
      </BrowserRouter>
    </div>
  );
}

export default App;
