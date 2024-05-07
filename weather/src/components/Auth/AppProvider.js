import React, { createContext, useReducer, useEffect } from "react";
import { CustomProvider } from "rsuite";

const LOCAL_STORAGE_KEY = "weather-app";

export const USER_SIGNOUT = "USER_SIGNOUT";
export const LOADING_USER = "LOADING_USER";
export const USER_FOUND = "USER_FOUND";

export const LIGHT_THEME = "LIGHT_THEME";
export const DARK_THEME = "DARK_THEME";
export const HIGH_CONTRAST_THEME = "HIGHCONTRAST_THEME";

let storedData = JSON.parse(localStorage.getItem(LOCAL_STORAGE_KEY));

const initialState = {
  user: null,
  token: null,
  endLogTime: null,
  loggedIn: false,
  isUserLoaging: false,
  theme: "dark",
};

const parseJwt = (token) => {
  const base64Url = token.split(".")[1];
  const base64 = base64Url.replace(/-/g, "+").replace(/_/g, "/");
  return JSON.parse(window.atob(base64));
};

const reducer = (state, action) => {
  switch (action.type) {
    case LIGHT_THEME:
      return {
        ...state,
        theme: "light",
      };
    case DARK_THEME:
      return {
        ...state,
        theme: "dark",
      };
    case HIGH_CONTRAST_THEME:
      return {
        ...state,
        theme: "high-contrast",
      };
    case USER_FOUND:
      return {
        ...state,
        user: action.payload.user,
        token: action.payload.token,
        endLogTime: parseJwt(action.payload.token).exp * 1000,
        loggedIn: true,
        isUserLoaging: false,
      };
    case LOADING_USER:
      return {
        ...state,
        isUserLoaging: true,
      };
    case USER_SIGNOUT:
      return {
        ...state,
        user: null,
        token: null,
        endLogTime: null,
        loggedIn: false,
      };
    default:
      return state;
  }
};

export const AppContext = createContext(initialState);
export const AppProvider = ({ children, ...rest }) => {
  const [store, dispatch] = useReducer(reducer, storedData || initialState);
  useEffect(() => {
    if (store.logTime < Date.now()) dispatch({ type: USER_SIGNOUT });
    localStorage.setItem(LOCAL_STORAGE_KEY, JSON.stringify(store));
  }, [store]);
  return (
    <AppContext.Provider value={{ store, dispatch }}>
      <CustomProvider theme={store.theme}>{children}</CustomProvider>
    </AppContext.Provider>
  );
};
