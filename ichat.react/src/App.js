import React from "react";
import "./App.css";
import Content from "./Content";
import Login from "./account/Login";
import { BrowserRouter, Route } from "react-router-dom";
import PrivateRoute from "./PrivateRoute";

function App() {
  return (
    <div id="container">
      <BrowserRouter>
        <PrivateRoute path="/" exact component={Content} />
        <PrivateRoute path={"/:section/:id"} component={Content} />{" "}
        <Route path={"/login"} component={Login} />
      </BrowserRouter>
    </div>
  );
}

export default App;
