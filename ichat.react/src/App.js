import React from "react";
import "./App.css";
import Content from "./Content";
import Login from "./account/Login";
import { BrowserRouter, Route } from "react-router-dom";
import PrivateRoute from "./PrivateRoute";
import { library } from "@fortawesome/fontawesome-svg-core";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faTimes, faChevronDown  } from "@fortawesome/free-solid-svg-icons";
{/* usage: <FontAwesomeIcon icon="chevron-down" /> */}

function App() { 
  library.add(faTimes);
  library.add(faChevronDown);

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
