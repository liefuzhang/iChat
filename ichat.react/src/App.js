import React from "react";
import "./App.css";
import Sidebar from "./Sidebar";
import Content from "./Content";
import { BrowserRouter, Route } from "react-router-dom";
import SignalRHub from "./SignalRHub";

function App() {
  return (
    <div id="container">
      <BrowserRouter>
        <Sidebar />
        <Route path={`/:section/:id`} component={Content} />
      </BrowserRouter>
      <SignalRHub></SignalRHub>
    </div>
  );
}

export default App;
