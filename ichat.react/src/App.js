import React from "react";
import "./App.css";
import Content from "./Content";
import Login from "./account/Login";
import { BrowserRouter, Route } from "react-router-dom";
import SignalRHub from "./SignalRHub";

function App() {
  return (
    <div id="container">
      <BrowserRouter>
        <Route path={`/login`} component={Login} />
        <Route path={`/:section/:id`} component={Content} />
      </BrowserRouter>
      <SignalRHub></SignalRHub>
    </div>
  );
}

export default App;
