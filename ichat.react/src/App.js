import React from "react";
import "./App.css";
import Content from "./content/Content";
import Login from "account/Login";
import AcceptInvitation from "./account/AcceptInvitation";
import { BrowserRouter, Route, Switch } from "react-router-dom";
import PrivateRoute from "components/PrivateRoute";
import { library } from "@fortawesome/fontawesome-svg-core";
import {
  faTimes,
  faChevronDown,
  faPlusCircle
} from "@fortawesome/free-solid-svg-icons";
{
  /* usage: <FontAwesomeIcon icon="chevron-down" /> */
}

function App() {
  library.add(faTimes);
  library.add(faChevronDown);
  library.add(faPlusCircle);

  return (
    <div id="container">
      <BrowserRouter>
        <Switch>
          <Route path={"/login"} component={Login} />
          <Route path={"/user/acceptinvitation"} component={AcceptInvitation} />
          <PrivateRoute path="/" exact component={Content} />
          <PrivateRoute path={"/:section/:id"} component={Content} />
        </Switch>
      </BrowserRouter>
    </div>
  );
}

export default App;
