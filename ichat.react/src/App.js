import React from "react";
import "./App.css";
import MainPage from "./MainPage";
import Login from "account/Login";
import AcceptInvitation from "./account/AcceptInvitation";
import { BrowserRouter, Route, Switch } from "react-router-dom";
import PrivateRoute from "components/PrivateRoute";
import { library } from "@fortawesome/fontawesome-svg-core";
import {
  faTimes,
  faChevronDown,
  faPlusCircle,
  faFlag
} from "@fortawesome/free-solid-svg-icons";
{
  /* usage: <FontAwesomeIcon icon="chevron-down" /> */
}

function App() {
  library.add(faTimes);
  library.add(faChevronDown);
  library.add(faPlusCircle);
  library.add(faFlag);

  return (
    <div id="container">
      <BrowserRouter>
        <Switch>
          <Route path={"/login"} component={Login} />
          <Route path={"/user/acceptinvitation"} component={AcceptInvitation} />
          <PrivateRoute path="/" exact component={MainPage} />
          <PrivateRoute path={"/:section/:id"} component={MainPage} />
        </Switch>
      </BrowserRouter>
    </div>
  );
}

export default App;
