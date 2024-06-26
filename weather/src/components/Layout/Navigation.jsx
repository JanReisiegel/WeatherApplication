import React, { useContext, useEffect } from "react";
import { Navbar, Nav } from "rsuite";
import { Link, NavLink } from "react-router-dom";
import {
  AppContext,
  DARK_THEME,
  HIGH_CONTRAST_THEME,
  LIGHT_THEME,
} from "../Auth/AppProvider";
import { MdContrast, MdDarkMode, MdLightMode } from "react-icons/md";
import { MenuLogin } from "../Users/Login";
import { MenuRegister } from "../Users/Register";

const Navigation = (props) => {
  const { store, dispatch } = useContext(AppContext);
  useEffect(() => {}, [store]);
  return (
    <Navbar>
      <Navbar.Brand as={Link} to={"/"}>
        Weather App
      </Navbar.Brand>
      <Nav>
        <Nav.Item eventKey={1} as={NavLink} to={"/actual"}>
          Aktuální počasí
        </Nav.Item>
        <Nav.Item eventKey={2} as={NavLink} to={"/forecast"}>
          Předpověď počasí
        </Nav.Item>
        {store.loggedIn ? (
          <Nav.Item eventKey={3} as={NavLink} to={"/history"}>
            Historické počasí
          </Nav.Item>
        ) : null}
      </Nav>
      <Nav pullRight>
        {store.loggedIn ? (
          <>
            <Nav.Menu title={store.user.userName} trigger="click">
              <Nav.Item as={Link} to={"/locations"}>
                Uložená místa
              </Nav.Item>
            </Nav.Menu>
            <Nav.Item onClick={() => dispatch({ type: "USER_SIGNOUT" })}>
              Logout
            </Nav.Item>
          </>
        ) : (
          <>
            <MenuLogin />
            <MenuRegister />
          </>
        )}
        <Nav.Menu title="Theme" trigger="hover">
          <Nav.Item
            icon={<MdLightMode />}
            onClick={(e) => dispatch({ type: LIGHT_THEME })}
            disabled={props.theme === "light"}
          >
            Light
          </Nav.Item>
          <Nav.Item
            icon={<MdDarkMode />}
            onClick={(e) => dispatch({ type: DARK_THEME })}
            disabled={props.theme === "dark"}
          >
            Dark
          </Nav.Item>
          <Nav.Item
            icon={<MdContrast />}
            onClick={(e) => dispatch({ type: HIGH_CONTRAST_THEME })}
          >
            High Contrast
          </Nav.Item>
        </Nav.Menu>
      </Nav>
    </Navbar>
  );
};

export default Navigation;
