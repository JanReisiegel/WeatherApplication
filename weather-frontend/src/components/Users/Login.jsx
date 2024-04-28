import React, { useState } from "react";
import { Button, Form, Modal, Nav, Popover, Whisper } from "rsuite";

const MenuLogin = () => {
  const [dialog, setDialog] = useState(false);
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [visible, setVisible] = useState(false);

  return (
    <>
      <Nav.Item
        onClick={(e) => {
          setVisible(!visible);
        }}
      >
        Přihlásit se
      </Nav.Item>

      <Modal title="Přihlášení" open={visible}>
        <Modal.Header>
          <Modal.Title>Přihlášení</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <Form>
            <Form.Group controlId="email">
              <Form.ControlLabel>Email</Form.ControlLabel>
              <Form.Control
                type="email"
                placeholder="Email"
                name="email"
                value={email}
                onChange={(e) => setEmail(e)}
              />
              <Form.HelpText tooltip>Required</Form.HelpText>
            </Form.Group>
            <Form.Group controlId="password">
              <Form.ControlLabel>Heslo</Form.ControlLabel>
              <Form.Control
                type="password"
                placeholder="Heslo"
                name="password"
                value={password}
                onChange={(e) => setPassword(e)}
              />
              <Form.HelpText tooltip>Required</Form.HelpText>
            </Form.Group>
          </Form>
        </Modal.Body>
        <Modal.Footer>
          <Button onClick={() => setVisible(false)} appearance="subtle">
            Zavřít
          </Button>
          <Button
            onClick={() => console.log(email, password)}
            appearance="primary"
          >
            Přihlásit se
          </Button>
        </Modal.Footer>
      </Modal>
    </>
  );
};
export default MenuLogin;
