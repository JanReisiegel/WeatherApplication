import React, { useContext, useState } from "react";
import { Navbar, Nav } from "rsuite";
import { Link, NavLink } from "react-router-dom";
import {
  AppContext,
  DARK_THEME,
  HIGH_CONTRAST_THEME,
  LIGHT_THEME,
} from "../Auth/AppProvider";
import { MdContrast, MdDarkMode, MdLightMode } from "react-icons/md";
import MenuLogin, { LoginModal } from "../Users/Login";

const Navigation = (props) => {
  const { dispatch } = useContext(AppContext);
  const [login, setLogin] = useState(false);
  return (
    <Navbar>
      <Navbar.Brand as={Link} to={"/"}>
        Weather App
      </Navbar.Brand>
      <Nav>
        <Nav.Item eventKey={1} as={Link} to={"/"}>
          Home
        </Nav.Item>
        <Nav.Item eventKey={2} as={NavLink} to={"/weather"}>
          Aktuální počasí
        </Nav.Item>
        <Nav.Item eventKey={2} as={NavLink} to={"/forecats"}>
          Předpověď počasí
        </Nav.Item>
      </Nav>
      <Nav pullRight>
        <MenuLogin />
        <Nav.Menu title="Theme" trigger="hover">
          <Nav.Item
            icon={<MdLightMode />}
            onClick={(e) => dispatch({ type: LIGHT_THEME })}
            disabled={props.theme == "light"}
          >
            Light
          </Nav.Item>
          <Nav.Item
            icon={<MdDarkMode />}
            onClick={(e) => dispatch({ type: DARK_THEME })}
            disabled={props.theme == "dark"}
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
