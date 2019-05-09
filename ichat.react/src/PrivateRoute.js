import { Redirect, Route } from "react-router-dom";
import React from "react";
import AuthService from "./services/AuthService";

function PrivateRoute({ component: Component, ...rest }) {
  let authService = new AuthService();

  return (
    <Route
      {...rest}
      render={props =>
        authService.isLoggedIn() ? (
          <Component {...props} />
        ) : (
          <Redirect
            to={{
              pathname: "/login",
              state: { from: props.location }
            }}
          />
        )
      }
    />
  );
}

export default PrivateRoute