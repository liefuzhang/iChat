import React from "react";
import "./App.css";
import MainPage from "./MainPage";
import Login from "./account/Login";
import AcceptInvitation from "./account/AcceptInvitation";
import ResetPassword from "./account/ResetPassword";
import { BrowserRouter, Route, Switch } from "react-router-dom";
import PrivateRoute from "./components/PrivateRoute";
import { toast, Slide } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";

toast.configure({
  position: "top-center",
  autoClose: 4000,
  hideProgressBar: true,
  newestOnTop: false,
  closeOnClick: true,
  pauseOnVisibilityChange: true,
  draggable: true,
  pauseOnHover: true,
  transition: Slide
});

function App() {
  return (
    <div id="container">
      <BrowserRouter>
        <Switch>
          <Route path={"/login"} component={Login} />
          <Route path={"/user/acceptinvitation"} component={AcceptInvitation} />
          <Route path={"/user/resetPassword"} component={ResetPassword} />
          <PrivateRoute path="/" exact component={MainPage} />
          <PrivateRoute path={"/:section/:id"} component={MainPage} />
        </Switch>
      </BrowserRouter>
    </div>
  );
}

export default App;
