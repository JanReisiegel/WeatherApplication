import axios from "axios";
import { LocationApi } from "../../configuration/API";
import { useNavigate } from "react-router-dom";
import { Button, Form } from "rsuite";
import { useContext, useState } from "react";
import { AppContext } from "../Auth/AppProvider";

export const AddLocation = () => {
  const { store } = useContext(AppContext);
  const [customName, setCustomName] = useState("");
  const [cityName, setCityName] = useState("");
  const navigate = useNavigate();

  const saveLocation = () => {
    axios
      .post(
        LocationApi.basic +
          "?cityName=" +
          cityName +
          "&customName=" +
          customName,
        {},
        {
          headers: {
            "Content-Type": "application/json",
            "Access-Control-Allow-Origin": "http://localhost:3000",
            userToken: store.token ? store.token : "",
          },
        }
      )
      .then((response) => {
        if (response.status === 200) {
          setCustomName("");
          setCityName("");
        }
      })
      .catch((error) => {
        console.error(error);
      })
      .finally(() => {
        return navigate("/saved-locations");
      });
  };

  return (
    <Form>
      <Form.Group controlId="customName">
        <Form.ControlLabel>Vlastní název</Form.ControlLabel>
        <Form.Control
          type="text"
          placeholder="Vlastní název"
          name="customName"
          value={customName}
          onChange={(e) => setCustomName(e)}
        />
        <Form.HelpText tooltip>Required</Form.HelpText>
      </Form.Group>
      <Form.Group controlId="cityName">
        <Form.ControlLabel>Název města</Form.ControlLabel>
        <Form.Control
          type="text"
          placeholder="Název města"
          name="cityName"
          value={cityName}
          onChange={(e) => setCityName(e)}
        />
        <Form.HelpText tooltip>Required</Form.HelpText>
      </Form.Group>
      <Form.Group>
        <Button onClick={saveLocation}>Uložit</Button>
      </Form.Group>
    </Form>
  );
};
