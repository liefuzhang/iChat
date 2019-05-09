import React from "react";
import "./App.css";
import Content from "./Content";
import Login from "./account/Login";
import { BrowserRouter, Route } from "react-router-dom";
import PrivateRoute from "./PrivateRoute";
import Sidebar from "./Sidebar";

function App() {
  return (
    <div id="container">
      <BrowserRouter>
        <Sidebar />
        <Route path={`/:section/:id`} component={Content} />
        <Route path={"/login"} component={Login} />
      </BrowserRouter>
    </div>
  );
}

export default App;
