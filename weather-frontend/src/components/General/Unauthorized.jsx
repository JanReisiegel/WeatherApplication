import { Button, Message } from "rsuite";
import { Link } from "react-router-dom";

const Unauthorized = ({ redirectToUrl }) => {
  return (
    <Message showIcon type="warning" header={<h3>Neznám tě! Kdo jsi?</h3>}>
      <p>Aplikaci se nepodařilo vás rozeznat.</p>
      <p>Nezapoměli jste se přihlásit?</p>
      <Button color="yellow" appearance="primary" as={Link} to={redirectToUrl}>
        Přihlásit se
      </Button>
    </Message>
  );
};

export default Unauthorized;
