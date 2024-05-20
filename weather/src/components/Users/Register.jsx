import React, { useState } from "react";
import { Button, Form, Message, Modal, Nav } from "rsuite";
import FormGroup from "rsuite/esm/FormGroup";
import axios from "axios";
import { UserApi } from "../../configuration/API";

export const MenuRegister = () => {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [phoneNo, setPhoneNo] = useState("");
  const [username, setUsername] = useState("");
  const [visible, setVisible] = useState(false);
  const [response, setResponse] = useState(Number);

  const registerAction = () => {
    console.log(email, password, phoneNo, username);
    axios
      .post(
        UserApi.register,
        {
          email: email,
          password: password,
          phoneNumber: phoneNo,
          userName: username,
        },
        {
          headers: {
            "Content-Type": "application/json",
          },
        }
      )
      .then((response) => {
        console.log(response);
        if (response.status === 200) {
          setResponse(200);
          setEmail("");
          setPassword("");
          setPhoneNo("");
          setUsername("");
        }
      })
      .catch((error) => {
        console.error(error);
      });
  };

  return (
    <>
      <Nav.Item onClick={() => setVisible(!visible)}>Registrovat</Nav.Item>

      <Modal
        title="Registrace"
        open={visible}
        backdrop={true}
        onClose={() => setVisible(false)}
        keyboard={true}
      >
        <Modal.Header>
          {response === 200 ? (
            <Message showIcon type="success">
              {response}
            </Message>
          ) : null}
          <Modal.Title>Registrace</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <Form>
            <FormGroup controlId="email">
              <Form.ControlLabel>Email</Form.ControlLabel>
              <Form.Control
                type="email"
                placeholder="Email"
                name="email"
                value={email}
                onChange={(e) => setEmail(e)}
              />
              <Form.HelpText tooltip>Required</Form.HelpText>
            </FormGroup>
            <FormGroup controlId="password">
              <Form.ControlLabel>Heslo</Form.ControlLabel>
              <Form.Control
                type="password"
                placeholder="Heslo"
                name="password"
                value={password}
                onChange={(e) => setPassword(e)}
              />
              <Form.HelpText tooltip>Required</Form.HelpText>
            </FormGroup>
            <FormGroup controlId="phoneNo">
              <Form.ControlLabel>Telefonní číslo</Form.ControlLabel>
              <Form.Control
                type="tel"
                placeholder="Telefonní číslo"
                name="phoneNo"
                value={phoneNo}
                onChange={(e) => setPhoneNo(e)}
              />
              <Form.HelpText tooltip>Required</Form.HelpText>
            </FormGroup>
            <FormGroup controlId="username">
              <Form.ControlLabel>Uživatelské jméno</Form.ControlLabel>
              <Form.Control
                type="text"
                placeholder="Uživatelské jméno"
                name="username"
                value={username}
                onChange={(e) => setUsername(e)}
              />
              <Form.HelpText tooltip>Required</Form.HelpText>
            </FormGroup>
          </Form>
        </Modal.Body>
        <Modal.Footer>
          <Button onClick={() => setVisible(false)} appearance="subtle">
            Zavřít
          </Button>
          <Button onClick={() => registerAction()} appearance="primary">
            Registrovat
          </Button>
        </Modal.Footer>
      </Modal>
    </>
  );
};
